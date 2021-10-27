using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace avifencodergui.lib
{
    public class ConfigValue<T>
    {
        public T Value { get; set; }
        public bool Active { get; set; }
        public string Comment { get; set; }
    }



    public class Config
    {
        public static Config Load()
        {
            if (File.Exists(Constants.ConfigPath))
            {
                var j = File.ReadAllText(Constants.ConfigPath);
                var config = JsonSerializer.Deserialize<Config>(j);
                return config;
            }

            return null;
        }

        public static void Save(Config config)
        {
            if (!File.Exists(Constants.AppFolder))
            {
                Directory.CreateDirectory(Constants.AppFolder);
            }
            var jsonConfig = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(config, jsonConfig);
            File.WriteAllText(Constants.ConfigPath, jsonString);
        }

        public string CreateProgArgs(string input, string output)
        {
            return null;
        }

        public static Config CreateEmpty()
        {
            return new Config
            {
                Jobs = new ConfigValue<int>
                {
                    Comment = "Number of jobs (worker threads, default: 1)"
                },
                Lossless = new ConfigValue<bool>()
                {
                    Value = false,
                    Comment = "Set all defaults to encode losslessly, and emit warnings when settings/input don't allow for it",
                },
                Depth = new ConfigValue<int>()
                {
                    Value = 8,
                    Comment = "Output depth [8,10,12]. (JPEG/PNG only; For y4m or stdin, depth is retained)",
                },
                Yuv = new ConfigValue<string>()
                {
                    Value = "",
                    Comment = "Output format [default=444, 422, 420, 400]. (JPEG/PNG only; For y4m or stdin, format is retained)",
                },
                Premultiply = new ConfigValue<bool>
                {
                    Value = false,
                    Comment = "Premultiply color by the alpha channel and signal this in the AVIF",
                },
                Range = new ConfigValue<string>
                {
                    Comment = "YUV range [limited or l, full or f]. (JPEG/PNG only, default: full; For y4m or stdin, range is retained)"
                },
                Min = new ConfigValue<int>
                {
                    Comment = "Set min quantizer for color (0-63, where 0 is lossless)"
                },
                Max = new ConfigValue<int>
                {
                    Comment = "Set max quantizer for color (0-63, where 0 is lossless)"
                },
                MinAlpha = new ConfigValue<int>
                {
                    Comment = "Set min quantizer for alpha (0-63, where 0 is lossless)"
                },
                MaxAlpha = new ConfigValue<int>
                {
                    Comment = "Set max quantizer for alpha (0-63, where 0 is lossless)"
                },
                TileRowsLog2 = new ConfigValue<int>
                {
                    Comment = "Set log2 of number of tile rows (0-6, default: 0)"
                },
                TileColsLog2 = new ConfigValue<int>
                {
                    Comment = "Set log2 of number of tile columns (0-6, default: 0)"
                },
                Grid = new ConfigValue<int>
                {
                    Comment = @"Encode a single-image grid AVIF with M cols & N rows. Either supply MxN identical W/H/D images, or a single
image that can be evenly split into the MxN grid and follow AVIF grid image restrictions.The grid will adopt
the color profile of the first image supplied."
                },
                Speed = new ConfigValue<int>
                {
                    Comment = "Encoder speed (0-10, slowest-fastest, 'default' or 'd' for codec internal defaults. default speed: 6) "
                },
                Codec = new ConfigValue<string>
                {
                    Comment = "AV1 codec to use"
                },
                AdvancedSwitches = new AdvancedSwitches
                {
                    AdaptiveQuantizationMode = new ConfigValue<int>
                    {
                        Comment = "Adaptive quantization mode (0: off (default), 1: variance, 2: complexity, 3: cyclic refresh)"
                    },
                    ConstantOrConstrainedQualityLevel = new ConfigValue<int>
                    {
                        Comment = "Constant/Constrained Quality level (0-63, end-usage must be set to cq or q)"
                    },
                    EnableDeltaQuantizationInChromaPlanes = new ConfigValue<int>
                    {
                        Comment = "Enable delta quantization in chroma planes (0: disable (default), 1: enable)"
                    },
                    RateControlMode = new ConfigValue<string>
                    {
                        Comment = "Rate control mode (vbr, cbr, cq, or q)"
                    },
                    LoopFilterSharpness = new ConfigValue<int>
                    {
                        Comment = "Loop filter sharpness (0-7, default: 0)"
                    },
                    Tune = new ConfigValue<string>
                    {
                        Comment = "Tune the encoder for distortion metric (psnr or ssim, default: psnr)"
                    },
                }
            };
        }

        public static Config CreateSample1()
        {
            var config = CreateEmpty();
            // encoder speed. 99% of the time, lower is slower, but higher efficiency. My recommendation is 6 for speed, 3 for optimal quality. Anything lower is not very useful.
            config.Speed.Value = 6;
            config.Speed.Active = true;
            // how many threads you let the encoder use. Above 1, this will activate row threading, with a small efficiency loss.
            config.Jobs.Value = 16;
            config.Jobs.Active = true;
            //  is the minimum and maximum Q range used for the encoder per SB. Lets the encoder breath as much as possible. A smaller search space is faster, but lowers peak efficiency and quality.
            config.Min.Value = 0;
            config.Min.Active = true;
            config.Max.Value = 63;
            config.Max.Active = true;
            // Chooses the "Quality" toggle, using quantizer modulation, with a certain quality level set by cq-level
            config.AdvancedSwitches.RateControlMode.Value = "q";
            config.AdvancedSwitches.RateControlMode.Active = true;
            config.AdvancedSwitches.ConstantOrConstrainedQualityLevel.Value = 63;
            config.AdvancedSwitches.ConstantOrConstrainedQualityLevel.Active = true;
            // Sets how much detail retention you want vs artifacts. 0 is the default, and a bit blurry. 1 deactivates some RD optimizations regarding artifact prevention. 2 is the highest I'd go, as going higher doesn't change much of anything, but provides the most detail/bpp.
            config.AdvancedSwitches.LoopFilterSharpness.Value = 2;
            config.AdvancedSwitches.LoopFilterSharpness.Active = true;
            // changes the RD(rate distortion) tune from psnr to butteraugli. You need to have a recent version of aomenc compiled with butteraugli support. Provides good detail retention and best color. A bit slower than PSNR tuning. Only works with 8b images(so no -d 10). If you want 10-bit(16-bit processing) and better detail retention(even in 8-bit) in exchange for worse color handling, consider using -a tune=ssim
            config.AdvancedSwitches.Tune.Value = "butteraugli";
            config.AdvancedSwitches.Tune.Active = false; // Not yes supported
            // Enables chroma Q variation per SB. Free quality increase. Not activated by default.
            config.AdvancedSwitches.EnableDeltaQuantizationInChromaPlanes.Value = 1;
            config.AdvancedSwitches.EnableDeltaQuantizationInChromaPlanes.Active = true;
            // -a color:qm-min=0 The default min quantization matrix flatness is 8, which is too high in my opinion: it restricts how low the quantizer can go per SB. May be less efficient, but provides a higher quality ceiling.
            // Missing Config
            // Default is objective Q variation per superblock(1), which is not optimal for intra-only psychov-visual quality. Very recently, a intra quality Q variation mode made for psycho-visual quality has been introduced, and it actuallys works well.
            // Missing Config
            // Is a variance based AQ mode. By default, it doesn't work very well for video, but it works well for intra-only photographic images, particularly when combined with the -a color:sharpness=2 flag.
            config.AdvancedSwitches.AdaptiveQuantizationMode.Value = 1;
            config.AdvancedSwitches.AdaptiveQuantizationMode.Active = true;


            return config;
        }

        /// <summary>
        /// Number of jobs (worker threads, default: 1)
        /// </summary>
        public ConfigValue<int> Jobs { get; set; }

        /// <summary>
        /// Set all defaults to encode losslessly, and emit warnings when settings/input don't allow for it
        /// </summary>
        public ConfigValue<bool> Lossless { get; set; }

        /// <summary>
        /// Output depth [8,10,12]. (JPEG/PNG only; For y4m or stdin, depth is retained)
        /// </summary>
        public ConfigValue<int> Depth { get; set; }

        /// <summary>
        /// Output format [default=444, 422, 420, 400]. (JPEG/PNG only; For y4m or stdin, format is retained)
        /// </summary>
        public ConfigValue<string> Yuv { get; set; }

        /// <summary>
        /// Premultiply color by the alpha channel and signal this in the AVIF
        /// </summary>
        public ConfigValue<bool> Premultiply { get; set; }

        /// <summary>
        /// YUV range [limited or l, full or f]. (JPEG/PNG only, default: full; For y4m or stdin, range is retained)
        /// </summary>
        public ConfigValue<string> Range { get; set; }

        /// <summary>
        /// Set min quantizer for color (0-63, where 0 is lossless)
        /// </summary>
        public ConfigValue<int> Min { get; set; }

        /// <summary>
        /// Set max quantizer for color (0-63, where 0 is lossless)
        /// </summary>
        public ConfigValue<int> Max { get; set; }

        /// <summary>
        /// Set min quantizer for alpha (0-63, where 0 is lossless)
        /// </summary>
        public ConfigValue<int> MinAlpha { get; set; }

        /// <summary>
        /// Set max quantizer for alpha (0-63, where 0 is lossless)
        /// </summary>
        public ConfigValue<int> MaxAlpha { get; set; }

        /// <summary>
        /// Set log2 of number of tile rows (0-6, default: 0)
        /// </summary>
        public ConfigValue<int> TileRowsLog2 { get; set; }

        /// <summary>
        /// Set log2 of number of tile columns (0-6, default: 0)
        /// </summary>
        public ConfigValue<int> TileColsLog2 { get; set; }

        /// <summary>
        /// Encode a single-image grid AVIF with M cols & N rows. Either supply MxN identical W/H/D images, or a single
        /// image that can be evenly split into the MxN grid and follow AVIF grid image restrictions.The grid will adopt
        /// the color profile of the first image supplied.
        /// </summary>
        public ConfigValue<int> Grid { get; set; }

        /// <summary>
        /// Encoder speed (0-10, slowest-fastest, 'default' or 'd' for codec internal defaults. default speed: 6) 
        /// </summary>
        public ConfigValue<int> Speed { get; set; }

        /// <summary>
        /// AV1 codec to use (choose from versions list below)
        /// </summary>
        public ConfigValue<string> Codec { get; set; }

        /// <summary>
        /// Pass an advanced, codec-specific key/value string pair directly to the codec. avifenc will warn on any not used by the codec.
        /// </summary>
        public AdvancedSwitches AdvancedSwitches { get; set; }

    }


    public class AdvancedSwitches
    {
        /// <summary>
        /// Adaptive quantization mode (0: off (default), 1: variance, 2: complexity, 3: cyclic refresh)
        /// </summary>
        public ConfigValue<int> AdaptiveQuantizationMode { get; set; }

        /// <summary>
        /// Constant/Constrained Quality level (0-63, end-usage must be set to cq or q)
        /// </summary>
        public ConfigValue<int> ConstantOrConstrainedQualityLevel { get; set; }

        /// <summary>
        /// Enable delta quantization in chroma planes (0: disable (default), 1: enable)
        /// </summary>
        public ConfigValue<int> EnableDeltaQuantizationInChromaPlanes { get; set; }

        /// <summary>
        /// Rate control mode (vbr, cbr, cq, or q)
        /// </summary>
        public ConfigValue<string> RateControlMode { get; set; }

        /// <summary>
        /// Loop filter sharpness (0-7, default: 0)
        /// </summary>
        public ConfigValue<int> LoopFilterSharpness { get; set; }

        /// <summary>
        /// Tune the encoder for distortion metric (psnr or ssim, default: psnr)
        /// </summary>
        public ConfigValue<string> Tune { get; set; }
    }
}
