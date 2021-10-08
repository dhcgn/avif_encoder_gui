﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var filename = GetFileName(job);
            var arguments = GetArguments(job);

            var r = await RunProcessAsync(filename, arguments);

            return new ExecuteImageOperationResult()
            {
                State = Job.JobStateEnum.Done
            };
        }

        private static string GetArguments(Job job)
        {
            return $"--jobs 16 --speed 6 \"{job.FilePath}\" \"{Path.Combine(new FileInfo(job.FilePath).DirectoryName,job.FileName)}.avif\"";
        }

        private static string GetFileName(Job job)
        {
            return ExternalAvifRessourceHandler.EcoderFilePath;
        }

        // TODO Error Handling and output
        static Task<(int returnCode, string output)> RunProcessAsync(string fileName, string arguments)
        {
            var tcs = new TaskCompletionSource<(int returnCode, string output)>();

            var process = new Process
            {
                // EnableRaisingEvents = true,
                StartInfo = { 
                    FileName = fileName, 
                    Arguments = arguments,
                    WindowStyle= ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true,
            };

            
            process.Exited += (sender, args) =>
            {
                string line = "";
                while (!process.StandardOutput.EndOfStream)
                {
                    line += process.StandardOutput.ReadLine() + Environment.NewLine;
                }

                tcs.SetResult((process.ExitCode, line));
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
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
