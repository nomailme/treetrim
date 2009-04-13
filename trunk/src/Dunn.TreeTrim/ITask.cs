namespace Dunn.TreeTrim
{
    /// <summary>
    /// Represents a type that performs a task.  A Task is something that needs to be done.
    /// The <see cref="IPlugin"/> is used to do the work and the <see cref="IPluginRuntimeSettings"/>
    /// specifies the run settings to use.
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Gets the name of the moniker.  Each task is associated a moniker.  The client
        /// using these tasks associates a moniker, usually a command line parameter, with
        /// a task.  Such monikers could be 'deleteFromDisk', 'email', '7zip' etc.
        /// </summary>
        /// <value>The name of the moniker.</value>
        string Moniker { get ; }

        /// <summary>
        /// Gets the plugin used to perform the task.
        /// </summary>
        /// <value>The plugin used to perform the task.</value>
        IPlugin Plugin { get ; }

        /// <summary>
        /// Gets the settings associated with this task.  These settings are passed to
        /// the <see cref="IPlugin"/>.
        /// </summary>
        /// <value>The settings.</value>
        IPluginRuntimeSettings Settings { get; }

        /// <summary>
        /// Gets the settings value.  The <see cref="IPluginRuntimeSettings"/> types
        /// exposes data in key/value pairs.  This property is the whole string
        /// for those pairs.  
        /// <example>-zip:writeTo:"c:\temp\my.zip"+dontCleanUp</example>
        /// </summary>
        /// <value>The settings value.</value>
        string SettingsValue { get ; }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <value>The value of the property.</value>
        string this[ string propertyName ]
        {
            get;
        }

        /// <summary>
        /// Cleans up the work done by this task.  An example would be deleting
        /// files that were created during the running of the task.  Some tasks allow the user
        /// to override this, normally with the 'dontCleanUp' runtime parameter.
        /// </summary>
        void Cleanup( ) ;

        /// <summary>
        /// Runs the specified task.
        /// </summary>
        /// <param name="lastTask">The task that was last run. This is useful
        /// as the working file or folder is exposed by the last plugin, and 
        /// it's normally this that this plugin works with</param>
        void Run( ITask lastTask ) ;
    }
}