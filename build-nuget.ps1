# Script to build and create NuGet package for ARPL

# Parameters
param(
    [string]$version = "1.0.0",
    [string]$configuration = "Release"
)

# Ensure we stop on any error
$ErrorActionPreference = "Stop"

Write-Host "Building ARPL version $version in $configuration configuration..."

# Clean previous builds
Write-Host "Cleaning previous builds..."
dotnet clean -c $configuration
if (Test-Path ".\artifacts") {
    Remove-Item -Path ".\artifacts" -Recurse -Force
}

# Create artifacts directory
New-Item -ItemType Directory -Path ".\artifacts" -Force | Out-Null

# Restore dependencies
Write-Host "Restoring dependencies..."
dotnet restore

# Build solution
Write-Host "Building solution..."
dotnet build -c $configuration /p:Version=$version

# Run tests
Write-Host "Running tests..."
dotnet test -c $configuration --no-build

# Create NuGet package
Write-Host "Creating NuGet package..."

dotnet pack .\arpl\arpl.csproj `
    -c $configuration `
    /p:Version=$version `
    /p:PackageVersion=$version `
    /p:NuspecFile=arpl.nuspec `
    /p:NuspecProperties="version=$version;configuration=$configuration" `
    --output .\artifacts

Write-Host "Build completed successfully!"
Write-Host "NuGet package created at .\artifacts\arpl.$version.nupkg"
