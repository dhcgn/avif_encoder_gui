# avif_encoder_gui

[![.NET Build](https://github.com/dhcgn/avif_encoder_gui/actions/workflows/dotnet_build.yml/badge.svg)](https://github.com/dhcgn/avif_encoder_gui/actions/workflows/dotnet_build.yml)
[![.NET Build and Test](https://github.com/dhcgn/avif_encoder_gui/actions/workflows/dotnet_build_test.yml/badge.svg)](https://github.com/dhcgn/avif_encoder_gui/actions/workflows/dotnet_build_test.yml)

**In progress**

A Windows GUI Application to convert Images to the [AV1 Image File Format (AVIF)](https://en.wikipedia.org/wiki/AV1#AV1_Image_File_Format_(AVIF)).

This is only a GUI Frontend for the official [libavif - Library for encoding and decoding .avif files](https://github.com/AOMediaCodec/libavif)


## Features

- Converting jpg, jpeg, png, y4m files to avif
- Converting avif to jpg, jpeg, png, y4m files
- Every Parameter to alter the encoding process from libavif can be used in the final app:

```
    -l,--lossless                     : Set all defaults to encode losslessly, and emit warnings when settings/input don't allow for it
    -d,--depth D                      : Output depth [8,10,12]. (JPEG/PNG only; For y4m or stdin, depth is retained)
    -y,--yuv FORMAT                   : Output format [default=444, 422, 420, 400]. (JPEG/PNG only; For y4m or stdin, format is retained)
    -p,--premultiply                  : Premultiply color by the alpha channel and signal this in the AVIF
    --cicp,--nclx P/T/M               : Set CICP values (nclx colr box) (3 raw numbers, use -r to set range flag)
                                        P = color primaries
                                        T = transfer characteristics
                                        M = matrix coefficients
                                        (use 2 for any you wish to leave unspecified)
    -r,--range RANGE                  : YUV range [limited or l, full or f]. (JPEG/PNG only, default: full; For y4m or stdin, range is retained)
    --min Q                           : Set min quantizer for color (0-63, where 0 is lossless)
    --max Q                           : Set max quantizer for color (0-63, where 0 is lossless)
    --minalpha Q                      : Set min quantizer for alpha (0-63, where 0 is lossless)
    --maxalpha Q                      : Set max quantizer for alpha (0-63, where 0 is lossless)
    --tilerowslog2 R                  : Set log2 of number of tile rows (0-6, default: 0)
    --tilecolslog2 C                  : Set log2 of number of tile columns (0-6, default: 0)
    -g,--grid MxN                     : Encode a single-image grid AVIF with M cols & N rows. Either supply MxN identical W/H/D images, or a single
                                        image that can be evenly split into the MxN grid and follow AVIF grid image restrictions. The grid will adopt
                                        the color profile of the first image supplied.
    -s,--speed S                      : Encoder speed (0-10, slowest-fastest, 'default' or 'd' for codec internal defaults. default speed: 6)
    -c,--codec C                      : AV1 codec to use (choose from versions list below)
    --exif FILENAME                   : Provide an Exif metadata payload to be associated with the primary item
    --xmp FILENAME                    : Provide an XMP metadata payload to be associated with the primary item
    --icc FILENAME                    : Provide an ICC profile payload to be associated with the primary item
    -a,--advanced KEY[=VALUE]         : Pass an advanced, codec-specific key/value string pair directly to the codec. avifenc will warn on any not used by the codec.
    --duration D                      : Set all following frame durations (in timescales) to D; default 1. Can be set multiple times (before supplying each filename)
    --timescale,--fps V               : Set the timescale to V. If all frames are 1 timescale in length, this is equivalent to frames per second (Default: 30)
                                        If neither duration nor timescale are set, avifenc will attempt to use the framerate stored in a y4m header, if present.
    -k,--keyframe INTERVAL            : Set the forced keyframe interval (maximum frames between keyframes). Set to 0 to disable (default).
    --ignore-icc                      : If the input file contains an embedded ICC profile, ignore it (no-op if absent)
    --pasp H,V                        : Add pasp property (aspect ratio). H=horizontal spacing, V=vertical spacing
    --clap WN,WD,HN,HD,HON,HOD,VON,VOD: Add clap property (clean aperture). Width, Height, HOffset, VOffset (in num/denom pairs)
    --irot ANGLE                      : Add irot property (rotation). [0-3], makes (90 * ANGLE) degree rotation anti-clockwise
    --imir AXIS                       : Add imir property (mirroring). 0=vertical axis ("left-to-right"), 1=horizontal axis ("top-to-bottom")

aom-specific advanced options:
    aq-mode=M                         : Adaptive quantization mode (0: off (default), 1: variance, 2: complexity, 3: cyclic refresh)
    cq-level=Q                        : Constant/Constrained Quality level (0-63, end-usage must be set to cq or q)
    enable-chroma-deltaq=B            : Enable delta quantization in chroma planes (0: disable (default), 1: enable)
    end-usage=MODE                    : Rate control mode (vbr, cbr, cq, or q)
    sharpness=S                       : Loop filter sharpness (0-7, default: 0)
    tune=METRIC                       : Tune the encoder for distortion metric (psnr or ssim, default: psnr)
    film-grain-test=TEST              : Film grain test vectors (0: none (default), 1: test-1  2: test-2, ... 16: test-16)
    film-grain-table=FILENAME         : Path to file containing film grain parameters
```

## GUI Wireframe

![image](https://user-images.githubusercontent.com/6566207/136546972-6c62e856-2d27-4d53-890c-31872407bd8f.png)

