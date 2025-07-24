# AssemblyVersionCompare

A command-line utility for comparing assembly versions between two directories. This tool helps identify version differences, missing assemblies, and deployment discrepancies.

## Usage

```bash
AssemblyVersionCompare.exe <source-path> <compare-path>
```

**Parameters:**
- `source-path` - The first directory to scan for assemblies
- `compare-path` - The second directory to compare against

## What It Does

The tool recursively scans both directories for `.dll` files and:

1. **Identifies Missing Assemblies** - Reports assemblies present in one directory but not the other
2. **Compares Versions** - Highlights assemblies with different versions between directories
3. **Provides Clear Output** - Shows relative paths and version information for easy identification

## Example Usage

### Basic Comparison
```bash
AssemblyVersionCompare.exe "C:\MyApp\v1.0" "C:\MyApp\v2.0"
```

### Deployment Verification
```bash
AssemblyVersionCompare.exe "C:\Build\Release" "C:\Deploy\Production"
```

### Environment Comparison
```bash
AssemblyVersionCompare.exe "\\server1\app" "\\server2\app"
```

## Sample Output

```
Assembly Version Comparer
Assembly in source folder but missing from compare: Libraries\CustomLibrary.dll
MyApp.Core.dll version 1.2.3.0 is different to compare version of 1.2.4.0
MyApp.Data.dll version 2.1.0.0 is different to compare version of 2.0.0.0
Assembly in compare folder but missing from source: Plugins\NewPlugin.dll
```

## Output Scenarios

### Missing Assemblies
```
Assembly in source folder but missing from compare: Libraries\MissingLib.dll
Assembly in compare folder but missing from source: NewComponents\AddedLib.dll
```

### Version Differences
```
MyApplication.dll version 1.0.0.0 is different to compare version of 1.1.0.0
ThirdParty.Library.dll version 3.2.1.0 is different to compare version of 3.2.2.0
```

### No Differences
When directories contain identical assemblies with matching versions, the tool produces no output (silent success).

## Common Use Cases

### 1. Deployment Verification
Verify that a deployment contains the expected assembly versions:
```bash
# Compare local build against deployed version
AssemblyVersionCompare.exe "C:\Build\Output" "\\server\deployment\myapp"
```

### 2. Environment Synchronization
Ensure multiple environments have consistent assembly versions:
```bash
# Compare staging vs production
AssemblyVersionCompare.exe "\\staging-server\app" "\\prod-server\app"
```

### 3. Release Validation
Validate what changed between releases:
```bash
# Compare v1.0 vs v2.0 releases
AssemblyVersionCompare.exe "C:\Releases\v1.0" "C:\Releases\v2.0"
```

### 4. Dependency Analysis
Identify dependency version differences:
```bash
# Compare two different project builds
AssemblyVersionCompare.exe "C:\ProjectA\bin" "C:\ProjectB\bin"
```

### 5. CI/CD Pipeline Validation
```bash
# In build script - compare artifacts against reference
AssemblyVersionCompare.exe "%BUILD_OUTPUT%" "%REFERENCE_BUILD%"
if %ERRORLEVEL% NEQ 0 echo "Assembly differences detected"
```

## Integration with Build Scripts

### Batch Script Integration
```batch
@echo off
echo Comparing build outputs...
AssemblyVersionCompare.exe "C:\Build\Current" "C:\Build\Previous"

if %ERRORLEVEL% EQU 0 (
    echo No assembly differences found
) else (
    echo Assembly differences detected - check output above
    exit /b 1
)
```

### PowerShell Integration
```powershell
$sourcePath = "C:\Build\Release"
$comparePath = "C:\Deploy\Current"

Write-Host "Comparing assemblies..."
& "AssemblyVersionCompare.exe" $sourcePath $comparePath

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Assembly versions match" -ForegroundColor Green
} else {
    Write-Host "⚠ Assembly version differences found" -ForegroundColor Yellow
}
```
