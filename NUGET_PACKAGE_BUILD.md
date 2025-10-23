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

## Building and Publishing the NuGet Package

You have **two options** to build and publish the NuGet package:

### Option 1: Using GitHub Actions (Recommended - Cloud Build)

A GitHub Actions workflow has been configured to automatically build and package the NuGet package using GitHub's cloud infrastructure (Windows runners).

**To build the package:**
1. Go to the "Actions" tab in the GitHub repository
2. Select the "Build and Pack NuGet" workflow
3. Click "Run workflow" button
4. The workflow will build all platform-specific libraries and create the NuGet package
5. Download the package from the workflow artifacts

**To publish to NuGet.org:**
1. Add your NuGet API key as a repository secret named `NUGET_API_KEY`
   - Go to Settings → Secrets and variables → Actions → New repository secret
2. Create and push a git tag (e.g., `v1.6.9`)
   ```bash
   git tag v1.6.9
   git push origin v1.6.9
   ```
3. The workflow will automatically build and publish to NuGet.org

**Benefits of this approach:**
- No local Windows machine required
- Consistent build environment
- Automated process
- Works in GitHub Codespaces or any environment

### Option 2: Manual Build on Windows Machine

If you prefer to build locally, you need a **Windows machine** with the following tools installed:
- Visual Studio 2017 or later
- .NET Framework 4.0 SDK
- .NET Core 3.0 SDK

### Building the Libraries (Manual Option)

1. **Build WPF Libraries**
   ```cmd
   cd Solution
   dotnet build WriteableBitmapEx_All.sln -c Release /p:EnableWindowsTargeting=true
   ```
   This will create the following outputs in `Build\Release\`:
   - `net40\WriteableBitmapEx.Wpf.dll`
   - `netcoreapp3.0\WriteableBitmapEx.Wpf.dll`

### Creating the NuGet Package (Manual Option)

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

### Publishing to NuGet.org (Manual Option)

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

## Summary

✅ Version number updated in all necessary files
✅ Copyright year updated to 2025
✅ Release notes updated
✅ Assembly version updated
✅ NuGet specification updated
✅ **GitHub Actions workflow configured for cloud builds**

## GitHub Codespaces Compatibility

Yes! This project now works with GitHub Codespaces. While you cannot build the Windows-specific libraries directly in Codespaces (which runs on Linux), you can:

1. **Edit and update version numbers** - Already done in this PR
2. **Trigger cloud builds** - Use the GitHub Actions workflow to build on Windows runners
3. **Review and manage releases** - All from within Codespaces

The GitHub Actions workflow runs on `windows-latest` runners, which have all the necessary SDKs and tools pre-installed, making it possible to build the complete NuGet package without needing a local Windows machine.
