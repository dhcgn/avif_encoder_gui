using System;
using System.Collections.Generic;
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
    }
}
