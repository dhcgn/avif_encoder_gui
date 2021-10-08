using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace avifencodergui.lib
{
    public class JobManager
    {
        BufferBlock<Job> jobs = new BufferBlock<Job>();

        public JobManager()
        {
            var consumerTask = ConsumeAsync(jobs);
        }

        public void Add(Job job)
        {
            jobs.Post(job);
        }

        static async Task<int> ConsumeAsync(ISourceBlock<Job> source)
        {

            while (await source.OutputAvailableAsync())
            {
                var job = await source.ReceiveAsync();

                var result = await ExecuteImageOperationAsync(job);

                job.State = result.State;
            }

            return 0;
        }

        private class ExecuteImageOperationResult
        {
            public Job.JobStateEnum State { get; internal set; }
        }

        private static async Task<ExecuteImageOperationResult> ExecuteImageOperationAsync(Job job)
        {
            await Task.Delay(1000);

            return new ExecuteImageOperationResult()
            {
                State = Job.JobStateEnum.Done
            };
        }
    }

    /// <summary>
    /// TODO ObservableObject should not be here
    /// </summary>
    public class Job : ObservableObject
    {
        

        public static Job Create(string filepath)
        {
            var fi = new FileInfo(filepath);
            return new Job()
            {
                FilePath = fi.FullName,
                FileName = fi.Name,
                Length = fi.Length
            };
        }

        public string FilePath { get; init; }
        public string FileName { get; init; }
        public long Length { get; init; }
        private JobStateEnum state;
        public JobStateEnum State { get => state; internal set => base.SetProperty(ref this.state, value); }

        public enum JobStateEnum
        {
            Pending,
            Done,
            Error,
        }
    }
}
