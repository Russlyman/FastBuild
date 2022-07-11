# FastBuild
[![Release](https://github.com/Russlyman/FastBuild/actions/workflows/release.yml/badge.svg)](https://github.com/Russlyman/FastBuild/actions/workflows/release.yml)

FastBuild is a console application designed to reduce the time between writing changes to C# based FiveM resource and testing them.

## Features
- In-built FXServer development server
- Symbolic linking between FiveM C# resource and development server
- Automatic resource restarts
- Updates resource with DLL files

## Installation
- Download the ZIP file from the assets section in the [latest release](https://github.com/Russlyman/FastBuild/releases/latest)
- Extract the ZIP into a folder
- Edit `config.json`
    - Set `resourcePath` to the path of your FiveM resource
    - Set `fxserverLicenseKey` to your FiveM server license key from [Cfx.re Keymaster](https://keymaster.fivem.net)
    - You can optionally update the `rcon*` settings but ensure they match `endpoint_add_*` and `rcon_password` in `fxserver.cfg`
- Open Command Prompt and navigate to the folder FastBuild was extracted to
- Execute command `fastbuild setupserver`
- Open the Visual Studio solution for your FiveM resource
    - For every project in Solution Explorer that is a FiveM script do
        - Right Click then `Properties`
        - Navigate to `Build` > `Events` section
            - Set `When to run the post-build event` to `When the build succeeds`
            - Set `Post-build event` to `if $(ConfigurationName) == Debug "<fastbuild_exepath>" build -p "$(TargetPath)"` but substitute `<fastbuild_exepath>` with the path of the FastBuild EXE file

Before developing, ensure the development server is started using the command `fastbuild startserver`. Now when you build your project in Visual Studio using the Debug configuration, FastBuild will perform its magic.

If you need to update the development server or the server data folder, you can run commands `fastbuild updateserver` and `fastbuild updatedata`.

If you ever want to develop a different resource and you configured it for use with FastBuild using the above steps, you should update the `resourcePath` in `config.json` and run the command `fastbuild link`.

## Command Line Parameters
### setupserver
Setup FXServer.

### startserver
Starts FXServer development server.

### build
Performs a build.

#### Options
| Option | Short | Long | Description
| --- | --- | --- | --- |
| DLL Path | -p | --path | The path of the new DLL file that should copied into the resource. |

### updateserver
Updates FXServer development server.

### updatedata
Updates FXServer server data folder.

### link
Links the resource specified in `config.json` to FastBuild.

## Authors
- [@Russlyman](https://www.github.com/Russlyman)
