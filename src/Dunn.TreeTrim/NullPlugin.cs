using System ;

namespace Dunn.TreeTrim
{
    /// <summary>
    /// A plugin that does nothing but to serve as a starting point for the source tree.
    /// </summary>
    public class NullPlugin : IPlugin
    {
        readonly string _path ;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullPlugin"/> class.
        /// </summary>
        /// <param name="path">The path with which this plugin reports as the working path.</param>
        public NullPlugin( string path )
        {
            _path = path ;
        }

        #region IPlugin Members

        /// <summary>
        /// Gets an exception as this property isn't used for this plugin.
        /// </summary>
        /// <value>Nothing is returned as this property isn't used.</value>
        /// <exception cref="NotSupportedException">Thrown every time!</exception>
        public string Moniker
        {
            get { throw new NotSupportedException(@"The null plugin doesn't respond to anything."); }
        }

        /// <summary>
        /// Gets the working path.  Plugins that run after this plugin will usually
        /// use this value to perform their work.  For instance, if a zip plugin
        /// was run, it would get the folder from the previous plugin.  The plugin that runs after
        /// the zip plugin would call this method and get the path to zip file.
        /// </summary>
        /// <value>The working path.</value>
        public string WorkingPath
        {
            get { return _path ; }
        }

        /// <summary>
        /// Cleans up this instance.  For instance, if the plug writes to a file,
        /// it gets a chance here to delete the file.
        /// </summary>
        public void Cleanup( )
        {
        }

        /// <summary>
        /// Runs the specified runtime settings.
        /// </summary>
        /// <param name="settings">The runtime settings.</param>
        /// <param name="lastPlugin">The last plugin.</param>
        public void Run( IPluginRuntimeSettings settings, IPlugin lastPlugin )
        {
        }

        #endregion
    }
}