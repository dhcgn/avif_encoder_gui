using System;
using System.IO;
using System.Linq;

namespace avifencodergui.lib
{
    public class Constants
    {
        public static string[] ExtensionsEncode = {".png", ".jpg", ".jpeg", ".y4m"};
        public static string[] ExtensionsDecode = {".avif"};

        public static string[] Extensions = ExtensionsEncode.Concat(ExtensionsDecode).ToArray();

        public static string AppFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "avifencodergui");

        public static string DecoderFilePath => Path.Combine(AppFolder, "avifdec.exe");
        public static string EncoderFilePath => Path.Combine(AppFolder, "avifenc.exe");
        public static string ConfigPath => Path.Combine(AppFolder, "config.json");
    }
}