dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true  .\avifencodergui.wpf\
Get-ChildItem avifencodergui.wpf\bin\Release\net6.0-windows\win-x64\publish\ -File | Sort-Object Length | Select-Object -Last 1