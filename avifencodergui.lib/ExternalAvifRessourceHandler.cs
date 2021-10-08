using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace avifencodergui.lib
{
    public class ExternalAvifRessourceHandler
    {
        private static string DecoderFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "avifencodergui", "avifdec.exe");
        private static string EcoderFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "avifencodergui", "avifenc.exe");

        public class AvifFileResult
        {
            public AvifFileResultEnum Result {  get; init; }
            public string? Version { get; init; }
        }

        public enum AvifFileResultEnum
        {
            OK,
            FileNotFound,
            VersionNotReadable,
        }


        public static AvifFileResult GetDecoderInformation()
        {
            return GetFileInformation(DecoderFilePath);
        }

        public static AvifFileResult GetEncoderInformation()
        {
            return GetFileInformation(EcoderFilePath);

        }

        private static AvifFileResult GetFileInformation(string path)
        {
            if (!File.Exists(path))
            {
                return new AvifFileResult
                {
                    Result = AvifFileResultEnum.FileNotFound,
                };
            }

            var version = GetExecutableVersion(path);

            if (version == null)
            {
                return new AvifFileResult
                {
                    Result = AvifFileResultEnum.VersionNotReadable,
                };
            }

            return new AvifFileResult
            {
                Result = AvifFileResultEnum.OK,
                Version = version
            };
        }



        private static string? GetExecutableVersion(string path)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            string line="";
            while (!proc.StandardOutput.EndOfStream)
            {
                line+= proc.StandardOutput.ReadLine();
            }
                       
            return ParseVersion(line);
        }

        private static string? ParseVersion(string input)
        {
            var r = Regex.Match(input, @"Version: (\d{1,}.\d{1,}.\d{1,})");
            if (r.Groups.Count != 2 )
            {
                return null;
            }

            return r.Groups[1].Value;
        }

    }
}
