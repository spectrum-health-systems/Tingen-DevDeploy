<!-- u250403 -->

<div align="center">

  ![logo](./.github/image/logo/TingenDevDeploy_logo_320x420.png)

  ![BranchWarning](https://img.shields.io/badge/Version-1.4-seagreen?style=for-the-badge)

</div>

# About Tingen-DevDeploy

Tingen DevDeploy simple command-line utility that deploys the ***development branch*** of the ***Tingen-WebService*** repository.

Tingen DevDeploy was created to make deploying quick iterations of the Tingen Web Services for Spectrum Health Systems, so it won't work for other organizations without modification (which is fine, since I'm the only person developing Tingen anyway).

## The Good

- A single, portable file.
- Logs everything.
- Gets the job done!

## The Bad

- Not customizable (everything is hardcoded).
- Is specifically for the ***development branch*** of the ***Tingen-WebService*** repository.
- Only works in Windows.

## The Ugly

- Source code isn't elegant, and doesn't follow best practices.

# Installation

Since Tingen DevDeploy is a single, portable file, all you need to do to "install" it is:

1. Download the [latest release](https://github.com/spectrum-health-systems/Tingen-DevDeploy/releases)
2. Extract Tingen DevDeploy to a location where it can be executed

# Usage

To use Tingen DevDeploy:

1. Open a command line where you extracted Tingen DevDeploy
2. Type `TingenDevDeploy`

## What Tingen-DevDeploy does

When you execute `TingenDevDeploy.exe`, it:

1. Verifies that a log directory exists
2. Verifies the Tingen-DevDeploy framework
3. Downloads the ***development branch*** of the ***Tingen-WebService*** repository
4. Extracts the downloaded repository data
5. Removes/recreates the directory where the Tingen Web Service is hosted
6. Copies the necessary web service files to the web service directory
