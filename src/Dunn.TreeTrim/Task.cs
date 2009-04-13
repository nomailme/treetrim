namespace Dunn.TreeTrim
{
    /// <summary>
    /// Represents an object that performs a task.  A task is composed of an
    /// <see cref="IPlugin"/> and <see cref="IPluginRuntimeSettings"/>.
    /// </summary>
    public class Task : ITask
    {
        /// <summary>
        /// Gets the name of the moniker.  Each task is associated a moniker.  The client
        /// using these tasks associates a moniker, usually a command line parameter, with
        /// a task.  Such monikers could be 'deleteFromDisk', 'email', '7zip' etc.
        /// </summary>
        /// <value>The name of the moniker.</value>
        public string Moniker
        {
            get { return Plugin.Moniker ; }
        }

        /// <summary>
        /// Gets or sets the plugin used to perform the task.
        /// </summary>
        /// <value>The plugin used to perform the task.</value>
        public IPlugin Plugin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the settings associated with this task.  
        /// These settings are passed to the <see cref="IPlugin"/>.
        /// </summary>
        /// <value>The settings.</value>
        public IPluginRuntimeSettings Settings
        {
            get ;
            set ;
        }

        /// <summary>
        /// Gets the settings value.  The <see cref="IPluginRuntimeSettings"/> types
        /// exposes data in key/value pairs.  This property is the whole string
        /// for those pairs.
        /// <example>-zip:writeTo:"c:\temp\my.zip"+dontCleanUp</example>
        /// </summary>
        /// <value>The settings value.</value>
        public string SettingsValue
        {
            get { return Settings.Value ; }
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified property name.
        /// </summary>
        /// <param name="propertyName">The property's name.</param>
        /// <value>The value of the property.</value>
        public string this[ string propertyName ]
        {
            get
            {
                return Settings[ propertyName ];
            }
        }

        /// <summary>
        /// Cleans up the work done by this task.  An example would be deleting
        /// files that were created during the running of the task.  Some tasks allow the user
        /// to override this, normally with the 'dontCleanUp' runtime parameter.
        /// </summary>
        public void Cleanup( )
        {
            Plugin.Cleanup( );
        }

        /// <summary>
        /// Runs the specified task.
        /// </summary>
        /// <param name="lastTask">The task that was last run. This is useful
        /// as the working file or folder is exposed by the last plugin, and
        /// it's normally this that this plugin works with</param>
        public void Run( ITask lastTask )
        {
            Plugin.Run( Settings, lastTask.Plugin );
        }
    }
}