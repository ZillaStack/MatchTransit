# Packages

This directory contains third party dependencies.

Currently managing these using git submodules where I can
since Unity's package management is still a hot mess.

`git submodule [--quiet] add [<options>] [--] <repository> [<path>]`

eg `git submodule add https://github.com/microsoft/MinIoC MatchTransit/Assets/ZillaStack/Packages/MinIoC`

## Post checkout

These can be done manually or by creating a git post checkout hook.

- MinIoC
  - Delete the Tests folder under MinIoC. 
    It's not needed and generates errors due to missing dependencies. 
  - Delete the .git folder under MinIoC. This will stop git from 
    alerting you to commit the changes you made above by deleting 
	the Tests folder. Since this is not a submodule we maintain, we don't 
	need this .git folder either.
