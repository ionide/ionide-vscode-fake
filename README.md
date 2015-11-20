#Ionide-VSCode: FAKE

It's part of [Ionide](http://ionide.io) plugin suite.
Using FAKE (F# Make) from VS Code.

[![Join the chat at https://gitter.im/ionide/ionide-project](https://img.shields.io/badge/gitter-join%20chat%20%E2%86%92-brightgreen.svg?style=flat-square)](https://gitter.im/ionide/ionide-project?utm_source=share-link&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) --  [Need Help? You can find us on Gitter](https://gitter.im/ionide/ionide-project)

## Features

- Run any build target defined in Your project's FAKE build script

## Configuration

`ionide-fake` allows the user to override the default conventions used to find and run FAKE builds. To do so You need to create an `.ionide` file in the root folder of Your project opened by Atom. The configuration file uses the [TOML](https://github.com/toml-lang/toml) language.

Here is the default configuration values used if the `.ionide` file doesn't exist or some entry is missing:

```TOML
[Fake]
linuxPrefix = "mono"
command = "build.cmd"
build = "build.fsx"
```

* Linux Prefix - command used as prefix on Linux / Mac - usually `sh` or `mono`

* Command - command executed as build taking build name as parameter - usually `build.cmd`, `build.sh`, `build.ps1`

* Build - FAKE build script, which is parsed to obtain list of possible builds - usually `build.fsx`, `fake.fsx`

## PATH settings

* In case of using Mono version, `mono` must be in PATH.

## Contributing and copyright

The project is hosted on [GitHub](https://github.com/ionide/ionide-vscode-fake) where you can [report issues](https://github.com/ionide/ionide-vscode-fake/issues), fork
the project and submit pull requests.

The library is available under [MIT license](https://github.com/ionide/ionide-vscode-fake/blob/master/LICENSE.md), which allows modification and
redistribution for both commercial and non-commercial purposes.
