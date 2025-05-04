# Script to build and create NuGet package for ARPL.AspNetCore

# Parameters
param(
    [string]$version = "1.0.0",
    [string]$configuration = "Release",
    [string]$arplVersion = "1.0.0"
)

# Ensure we stop on any error
$ErrorActionPreference = "Stop"

Write-Host "Building ARPL.AspNetCore version $version in $configuration configuration..."

# Clean previous builds
Write-Host "Cleaning previous builds..."
dotnet clean .\arpl.aspnetcore\Arpl.AspNetCore.csproj -c $configuration
if (Test-Path ".\artifacts") {
    Remove-Item -Path ".\artifacts\Arpl.AspNetCore.*" -Force
}

# Create artifacts directory if it doesn't exist
if (-not (Test-Path ".\artifacts")) {
    New-Item -ItemType Directory -Path ".\artifacts" -Force | Out-Null
}

# Restore dependencies
Write-Host "Restoring dependencies..."
dotnet restore .\arpl.aspnetcore\Arpl.AspNetCore.csproj /p:ArplVersion=$arplVersion

# Build project
Write-Host "Building project..."
dotnet build .\arpl.aspnetcore\Arpl.AspNetCore.csproj -c $configuration /p:Version=$version /p:ArplVersion=$arplVersion

# Create NuGet package
Write-Host "Creating NuGet package..."
dotnet pack .\arpl.aspnetcore\Arpl.AspNetCore.csproj `
    -c $configuration `
    /p:Version=$version `
    /p:PackageVersion=$version `
    /p:ArplVersion=$arplVersion `
    --no-build `
    --output .\artifacts

Write-Host "Build completed successfully!"
Write-Host "NuGet package created at .\artifacts\ARPL.AspNetCore.$version.nupkg"
