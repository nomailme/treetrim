# Introduction #

Creating a plugin for Tree Trim is fairly easy.  There are just a couple of things to remember:

  * Create a class and derive from IPlugin.
  * Put the assembly containing your plugin into the same directory as Tree Trim

## Deriving from IPlugin ##
`IPlugin` is quite a simple interface:
```
    /// <summary>
    /// Represents a plugin.  A plugin performs some work, such as deleting certain files.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Gets the moniker associated with this plugin.  A plugin is associated with a moniker,
        /// which is essentially a friendly name to identifiy the plugin.
        /// </summary>
        /// <value>The moniker.</value>
        string Moniker { get ; }

        /// <summary>
        /// Gets the working path.  Plugins that run after this plugin will usually
        /// use this value to perform their work.  For instance, if a zip plugin
        /// was run, it would get the folder from the previous plugin.  The plugin that runs after 
        /// the zip plugin would call this method and get the path to zip file.
        /// </summary>
        /// <value>The working path.</value>
        string WorkingPath { get ; }

        /// <summary>
        /// Cleans up this instance.  For instance, if the plug writes to a file, 
        /// it gets a chance here to delete the file.
        /// </summary>
        void Cleanup( ) ;

        /// <summary>
        /// Performs some work based on the settings passed.
        /// </summary>
        /// <param name="settings">The settings.  These specify the settings that the plugin
        /// should use and normally contain key/value pairs of parameters.</param>
        /// <param name="lastPlugin">The last plugin run.  This is often useful as
        /// normally, plugins need to work with the output produced from the last plugin.</param>
        void Run(IPluginRuntimeSettings settings, IPlugin lastPlugin);
    }
```

Here I'll show how to create a plugin that opens up an Explorer window to show the output of the previous plugin.  This is useful for when Tree Trim is run from Explorer's context menus.

First, create a Visual Studio Class Library project and reference `Dunn.TreeTrim` and `System.ComponentModel.Composition` (both in the `lib` folder in the Tree Trim source or with the executables in the Tree Trim binary downloads).

Now, create a class called `MyPlugin` and derive from the `IPlugin` interface declared in the namespace `Dunn.TreeTrim`.
```
[Export(typeof(IPlugin))]
public class Plugin : IPlugin
{
}
```
Notice the attribute?  That's a [MEF (Managed Extensibility Framework)](http://www.codeplex.com/MEF) attribute.  This tells Tree Trim that this assembly exports types that implement `IPlugin`.  This attribute is declared in the `System.ComponentModel.Composition` assembly that you referenced earlier.

Fill in this newly created class with the methods declared in the `IPlugin` interface.  Implement the following:
```
public string Moniker
{
	get { return @"myPlugin"; }
}
```
This says that the plugin responds to 'myPlugin' from the command line (e.g. `treetrim.gui.exe c:\mytree -myPlugin`)

```
public void Run(IPluginRuntimeSettings settings, IPlugin lastPlugin)
{
	_workingPath = lastPlugin.WorkingPath;

	Process.Start( 
		@"explorer.exe", 
		@"/select," + _workingPath ) ;
}
```

This is the main method that Tree Trim calls to run your plugin.  For this simple plugin, we're just going to start up Explorer.exe and give it a command line parameter to select a file.  The file we're going to show is the _output from the last plugin that was run_ - so if the last plugin zipped up a folder, it'll be a zip file - or if the last plugin's output was a folder, it'll be a folder.  Notice that we store the path in a field named `_workingPath`.  You'll see why in the next bit...

```
public string WorkingPath
{
	get { return _workingPath; }
}
```
This method allows future plugins to get access to what your plugin produces - for instance, if your plugin zipped the contents of a folder, this property would tell others where it stored the zip file.

```
public void Cleanup()
{
}
```
This method gives your plugin a chance to clean up after itself.  For this simple plugin, we don't have much to clear up.  But if we did more, for instance created some temporary files, we'd want to clear them up here.

### Deploy it! ###
That's it!  Build your assembly and put it in the same folder as Tree Trim.

Run Tree Trim and specify your new plugin: `TreeTrim.gui.exe c:\my_tree -myPlugin`.