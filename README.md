# PartyNest
*Dancing desktop parrots*

### Installation

1. Build the 'Release' package from source using Visual Studio 2019.
2. Add the 'Release' output directory to your PATH environment variable.
3. Run `PartyNest.exe` from any terminal.

### Running

The program accepts no command line arguments... yet.

#### Running with Git

To run this program whenever you run a `git push` follow the instructions below (taken from https://coderwall.com/p/jp7d5q/create-a-global-git-commit-hook):

1. Enable git templates: `git config --global init.templatedir '~/.git-templates'`
2. Create the directory to hold the hooks: `mkdir -p ~/.git-templates/hooks`
3. Create the file at "mkdir -p ~/.git-templates/hooks/pre-push" with the following contents:
    ```bash
    #!/bin/sh
    PartyNest.exe &
    ```
4. Make the file executable: `chmod a+x ~/.git-templates/hooks/pre-push`
5. Re-initialise any existing repositories with `git init`
