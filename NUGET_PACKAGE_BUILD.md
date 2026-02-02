# NuGet Package Build Instructions

## Automated Versioning

The NuGet package version is **automatically generated** during the GitHub Actions build process. The version follows the pattern **1.6.XXX** where XXX is the GitHub Actions run number, ensuring each build has a unique, incrementing version number.

**Examples:**
- Build run #100 → Version 1.6.100
- Build run #101 → Version 1.6.101
- Build run #150 → Version 1.6.150

The version is automatically updated in:
1. **Source/Common/GlobalAssemblyInfo.cs** - Assembly version
2. **Nuget/WriteableBitmapEx.nuspec** - NuGet package version

**Note:** You do NOT need to manually update version numbers in these files. The workflow handles this automatically.

## Building and Publishing the NuGet Package

### Using GitHub Actions (Recommended - Cloud Build)

A GitHub Actions workflow has been configured to automatically build, version, and publish the NuGet package using GitHub's cloud infrastructure (Windows runners).

**Setup (One-time):**
1. Add your NuGet API key as a repository secret named `NUGET_API_KEY`
   - Go to Settings → Secrets and variables → Actions → New repository secret
   - Name: `NUGET_API_KEY`
   - Value: Your NuGet.org API key

**To build and publish:**
1. Go to the "Actions" tab in the GitHub repository
2. Select the "Build and Pack NuGet" workflow
3. Click "Run workflow" button
4. The workflow will:
   - Auto-generate version number (1.6.{run_number})
   - Update version in GlobalAssemblyInfo.cs and WriteableBitmapEx.nuspec
   - Build all platform-specific libraries
   - Create the NuGet package
   - Publish to NuGet.org automatically (if NUGET_API_KEY secret is configured)
   - Upload package as artifact for manual download if needed

**Alternative triggers:**
- The workflow also runs automatically when you push a tag starting with 'v' (e.g., `v1.6.100`)
- You can manually trigger it from the Actions tab

**Benefits of this approach:**
- **Automatic version management** - No manual version updates needed
- **Unique version numbers** - Each build gets a new version
- **Automatic publishing** - Direct push to NuGet.org
- No local Windows machine required
- Consistent build environment
- Works in GitHub Codespaces or any environment

### Manual Build on Windows Machine (Alternative)

If you prefer to build locally, you need a **Windows machine** with the following tools installed:
- Visual Studio 2017 or later
- .NET Framework 4.0 SDK
- .NET Core 3.0 SDK

**Note:** When building manually, you will need to manually manage version numbers in the files mentioned above.

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
   This will create `WriteableBitmapEx.1.6.11.nupkg` in the `Build\nuget\` directory.

### Publishing to NuGet.org (Manual Option)

**Note:** When using GitHub Actions (recommended), publishing is automatic. This section is only for manual builds.

1. Update the version in `Nuget/push.cmd` to match your build
2. Update the API key in `push.cmd` (replace `[APIKEY]` with your actual NuGet API key)
3. Run the push command:
   ```cmd
   push.cmd
   ```
   
   **Note**: The push.cmd script currently tries to delete the old version, which may not be necessary or desired. You may want to comment out that line.

### Verification

After publishing, verify the package at:
https://www.nuget.org/packages/WriteableBitmapEx

The new version 1.6.11 should appear with the updated metadata.

## Summary

✅ **Automated version management** - Version auto-increments with each build (1.6.{run_number})
✅ **Automatic NuGet publishing** - Publishes to NuGet.org on every successful build
✅ **No manual version updates needed** - Workflow handles everything
✅ Copyright year updated to 2026
✅ GitHub Actions workflow configured for cloud builds

## GitHub Codespaces Compatibility

Yes! This project fully works with GitHub Codespaces. While you cannot build the Windows-specific libraries directly in Codespaces (which runs on Linux), you can:

1. **Make code changes** - Edit source files in Codespaces
2. **Trigger cloud builds** - Use the GitHub Actions workflow to build on Windows runners with automatic versioning
3. **Review and manage releases** - All from within Codespaces

The GitHub Actions workflow runs on `windows-latest` runners, which have all the necessary SDKs and tools pre-installed, making it possible to build and publish the complete NuGet package without needing a local Windows machine.
