# empty-service

This service is a template. Copypaste it wherever you need.

## Usage

Build it into docker container:
- `docker build --build-arg WATERMARK=MyCustomString -t empty_service --no-cache .`

Or build it locally, see the `_scripts` paragraph.

## Tech stack
- dotnet 3.0
- c# 7.3
- python 3
- Jenkins
- Docker

### Packages
- DI - `Autofac`
- Logger - `Serilog`
- Date/Time - `NodeTime`

## Features

- API endpoint `GET https://localhost:5001/api/ok` 
- A background `IHostedService` job that prints to console every 5 seconds
- Could be used in Jenkins pipeline by `Jenkinsfile`
- Could be packaged into a docker image by `Dockerfile`

### What to change after copypasting

Change
- Main folder name
- Solution name
- Open each project as text file and change the `AssemblyName` and `RootNamespace` tag values
- Open `/Test.Base/settings.json` and change the `assembly-name-prefix` and `namespace-prefix` values
- Open `/_configuration/build/dotnet/CommonAssemblyAttributes.cs` and change the attribute values

## Style guide

Tbd in another repository

## Implementation details

### Root
#### NuGet.Config

The file should exist on solution level due to a developer could not have a [default one](https://docs.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior).

#### Directory.Build.targets

Forces custom console output during build.

#### Directory.Build.props

Contains main information about the build like copyright, output directory, git hash commit, etc.

### _configuration
#### _configuration/build/dotnet/
##### CommonAssemblyAttributes.cs

The file should be included into each project as a link. Tests check this.

The file makes the current assembly friendly to the described assemblies, so internal modifiers become available.

#### _configuration/build/variables

Contains a variables for python build script.

#### _configuration/runtime

Copies recursively into output directory.

Contains settings that should be used in runtime.

### _scripts

Contains scripts to build and run the service.

Pathes below are solution related:
- Build - `python _scripts/build.py --config_file '_configuration/build/variables/Debug/build-settings.json' --watermark 'MyCustomString'`
    The watermark will be hardcoded into dll description and product name
- Run - `python _scripts/run.py --file _output/EmptyService.WebApi.dll` 
- Clean `python _scripts/clean.py`

