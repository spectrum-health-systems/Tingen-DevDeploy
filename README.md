<!-- u240528 -->

# About TingenDevDeploy
TingenDevDeploy simple command-line utility to deploy the Tingen_development web service.

## Features

- Is a single, portable file
- Is not customizable (not really a feature)
- Verifies that all of the directories that Tingen requires exist, and creates them if they don't
- Deploys the development branch of Tingen_development
- Logs everything

# Installation

Since TingenDevDeploy is a single, portable file, all you need to do to "install" it is:

1. Download the latest release
2. Extract TingenDevDeploy to a location where it can be executed

# Usage

To use TingenDevDeploy:

1. Open a command line where you extracted TingenDevDeploy
2. Type `TingenDevDeploy`

## What TingenDevDeploy does

When you execute TingenDevDeploy, it:

1. Verifies that a log directory exists
2. Verifies the Tingen Framework (directories, data, etc.)
3. Downloads the Tingen_development development branch and extracts all of the contents
4. Removes/recreates the Tingen_development web service directory
5. Copies the necessary web service files to the web service directory
