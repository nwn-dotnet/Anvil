Anvil includes a plugin/package manager based on [Paket](https://fsprojects.github.io/Paket/index.html) that can automatically update plugins from authors who publish their work on [NuGet](https://www.nuget.org/).

This can be convenient for server administrators who are running lots of plugins and want to avoid the overhead of downloading and installing plugins.

To get started, first navigate to your server's home directory and find the `anvil/Paket/paket.dependencies.orig` file.

Rename this file from `paket.dependencies.orig` to `paket.dependencies`, then open the file in your favourite text editor.

By default, the file should look like this:

```
// Rename this file to "paket.dependencies" to enable support for loading plugins from NuGet.
// See https://fsprojects.github.io/Paket/dependencies-file.html for more information.

strategy: min

source https://api.nuget.org/v3/index.json

// ---- Add plugins below this line ----
```

To install a plugin, first search and find the [NuGet](https://www.nuget.org/) package page for the plugin.

Once you are on the package page, you will want to copy the install command listed at the top of the page. It should look something like:

```
Install-Package NWN.ExamplePlugin -Version 8193.33.0
```

Copy the name and version parts of the command into the `paket.dependencies` file:

```
// Rename this file to "paket.dependencies" to enable support for loading plugins from NuGet.
// See https://fsprojects.github.io/Paket/dependencies-file.html for more information.

strategy: min

source https://api.nuget.org/v3/index.json

// ---- Add plugins below this line ----
nuget NWN.ExamplePlugin 8193.33.0
```

Save and close the file. The plugin should now download and run the next time you launch the server!
