# NuGet Package Build Instructions

## Version Update Complete

The version has been successfully updated from **1.6.8** to **1.6.9** in the following files:

1. **Source/Common/GlobalAssemblyInfo.cs**
   - AssemblyVersion: 1.6.9.0
   - AssemblyFileVersion: 1.6.9.0
   - Copyright year: 2025

2. **Nuget/WriteableBitmapEx.nuspec**
   - Version: 1.6.9
   - Release notes: Minor fixes and improvements
   - Copyright year: 2025

3. **Nuget/push.cmd**
   - VERSION: 1.6.9

## Next Steps to Build and Publish the NuGet Package

These steps must be performed on a **Windows machine** with the following tools installed:
- Visual Studio 2017 or later
- .NET Framework 4.0 SDK
- .NET Core 3.0 SDK
- Windows Phone SDK (for legacy targets)
- UWP SDK

### Building the Libraries

1. **Build WPF Libraries**
   ```cmd
   cd Solution
   dotnet build WriteableBitmapEx_All.sln -c Release
   ```
   This will create the following outputs in `Build\Release\`:
   - `net40\WriteableBitmapEx.Wpf.dll`
   - `netcoreapp3.0\WriteableBitmapEx.Wpf.dll`

2. **Build UWP Library**
   The UWP project should be built separately if needed.

3. **Build Legacy Libraries** (if needed)
   - Silverlight (WriteableBitmapEx.dll)
   - Windows Phone (WriteableBitmapExWinPhone.dll)
   - WinRT (WriteableBitmapEx.WinRT.dll)

### Creating the NuGet Package

Once all libraries are built and placed in the `Build\Release\` directory:

1. Navigate to the Nuget folder:
   ```cmd
   cd Nuget
   ```

2. Run the pack command:
   ```cmd
   pack.cmd
   ```
   This will create `WriteableBitmapEx.1.6.9.nupkg` in the `Build\nuget\` directory.

### Publishing to NuGet.org

1. Update the API key in `push.cmd` (replace `[APIKEY]` with your actual NuGet API key)

2. Run the push command:
   ```cmd
   push.cmd
   ```
   
   **Note**: The push.cmd script currently tries to delete the old version, which may not be necessary or desired. You may want to comment out that line.

### Verification

After publishing, verify the package at:
https://www.nuget.org/packages/WriteableBitmapEx

The new version 1.6.9 should appear with the updated metadata.

## What's Been Accomplished

✅ Version number updated in all necessary files
✅ Copyright year updated to 2025
✅ Release notes updated
✅ Assembly version updated
✅ NuGet specification updated

## What Still Needs to Be Done (Windows Only)

- Build all platform-specific libraries (WPF, UWP, Silverlight, Windows Phone)
- Run pack.cmd to create the .nupkg file
- Publish to NuGet.org using push.cmd (with valid API key)
