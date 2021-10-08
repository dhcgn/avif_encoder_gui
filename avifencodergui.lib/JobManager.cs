using Microsoft.Toolkit.Mvvm.ComponentModel;
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
                job.State = Job.JobStateEnum.Working;
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
            var targetFilePath = "";
            switch (job.Operation)
            {
                case Job.OperationEnum.Encode:
                     targetFilePath = $"{ Path.Combine(new FileInfo(job.FilePath).DirectoryName, job.FileName) }.avif";
                    break;
                case Job.OperationEnum.Decode:
                     targetFilePath = $"{ Path.Combine(new FileInfo(job.FilePath).DirectoryName, job.FileName) }.png";
                    break;
                default:
                    throw new Exception($"{job.Operation} should be Encode or Decode");
            }

            job.TargetFilePath = targetFilePath;

            switch (job.Operation)
            {
                case Job.OperationEnum.Encode:
                    return $"--jobs 16 --speed 6 \"{job.FilePath}\" \"{job.TargetFilePath}\"";
                case Job.OperationEnum.Decode:
                    return $"--jobs 16 \"{job.FilePath}\" \"{job.TargetFilePath}\"";
                default:
                    throw new Exception($"{job.Operation} should be Encode or Decode");
            }
        }

        private static string GetFileName(Job job)
        {
            switch (job.Operation)
            {
                case Job.OperationEnum.Encode:
                    return ExternalAvifRessourceHandler.EcoderFilePath;
                case Job.OperationEnum.Decode:
                    return ExternalAvifRessourceHandler.DecoderFilePath;
                default:
                    throw new Exception($"{job.Operation} should be Encode or Decode");
            }

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
                Length = fi.Length,
                FileInfo = fi,
                FormattedLength = GetFormattedLength(fi.Length)
            };
        }

        private static string GetFormattedLength(double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            return result;
        }

        public string FilePath { get; init; }
        public string FileName { get; init; }
        public long Length { get; init; }
        private JobStateEnum state;
        private string targetFileFormattedLength;

        public JobStateEnum State
        {
            get => state; internal set
            {
                base.SetProperty(ref this.state, value);
                if (value == JobStateEnum.Done)
                {
                    var fi = new FileInfo(TargetFilePath);
                    if (fi.Exists)
                    {
                        TargetFileLength = fi.Length;
                        TargetFileFormattedLength = GetFormattedLength(fi.Length);
                    }
                }
            }
        }
        public FileInfo FileInfo { get; init; }

        public OperationEnum Operation => GetOperation(FileInfo);

        public string FormattedLength { get; init; }
        public string TargetFilePath { get; internal set; }
        public long TargetFileLength { get; internal set; }
        public string TargetFileFormattedLength { get => targetFileFormattedLength; internal set => base.SetProperty(ref this.targetFileFormattedLength, value); }

        private OperationEnum GetOperation(FileInfo fileInfo)
        {
            if (fileInfo == null)
                return OperationEnum.Undef;

            switch (fileInfo.Extension.ToLowerInvariant())
            {
                case ".avif":
                    return OperationEnum.Decode;
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".y4m":
                    return OperationEnum.Encode;
                default:
                    return OperationEnum.Undef;
            }
        }

        public static Job GetDesignDate()
        {
            return new Job
            {
                FileName = "pic1.png",
                FilePath = "C:\\Users\\User\\Pictures\\pic1.png",
                Length = 6604,
                FormattedLength = "132 KB",
                TargetFileFormattedLength = "80 KB",
            };
        }

        public enum OperationEnum
        {
            Undef,
            Encode,
            Decode
        }

        public enum JobStateEnum
        {
            Pending,
            Done,
            Error,
            Working,
        }
    }
}
