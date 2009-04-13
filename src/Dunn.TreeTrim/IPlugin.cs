namespace Dunn.TreeTrim
{
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
}