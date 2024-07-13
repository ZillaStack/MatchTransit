# Packages

This directory contains third party dependencies.

Currently managing these using git submodules where I can
since Unity's package management is still a hot mess.

`git submodule [--quiet] add [<options>] [--] <repository> [<path>]`

eg `git submodule add https://github.com/microsoft/MinIoC MatchTransit/Assets/ZillaStack/Packages/MinIoC`
