using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avifencodergui.lib
{
    public class Constants
    {
        public static string[] ExtensionsEncode = new string[] { ".png", ".jpg", ".jpeg", ".y4m" };
        public static string[] ExtensionsDecode = new string[] { ".avif" };

        public static string[] Extensions = ExtensionsEncode.Concat(ExtensionsDecode).ToArray();

        public static string AppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "avifencodergui");

        public static string DecoderFilePath => Path.Combine(AppFolder, "avifdec.exe");
        public static string EncoderFilePath => Path.Combine(AppFolder, "avifenc.exe");
        public static string ConfigPath => Path.Combine(AppFolder, "config.json");
    }
}
