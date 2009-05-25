using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Globalization ;
using System.Text.RegularExpressions ;

namespace Dunn.TreeTrim
{
    /// <summary>
    /// Represents a collection of tasks.  This is created by providing
    /// a collection of plugins and the settings associated with each plugin.
    /// Together (the plugin and the settings), these represent a <see cref="ITask"/>.
    /// </summary>
    public class TaskCollection : ITaskCollection
    {
        readonly List< string > _monikers ;
        readonly Dictionary< string, Task > _taskLookup ;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCollection"/> class.
        /// </summary>
        /// <param name="plugins">The plugins. Each task has a plugin.</param>
        /// <param name="args">The arguments for each plugin.</param>
        public TaskCollection( IEnumerable< IPlugin > plugins, IEnumerable< string > args )
        {
            _monikers = new List< string >( ) ;
            
            _taskLookup = new Dictionary<string, Task>(StringComparer.CurrentCultureIgnoreCase);
            
            populateLookupWithDiscoveredPlugins( plugins ) ;

            buildSettingsForPluginsBasedOnCommandLineValues( args ) ;
        }

        #region ITaskCollection Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator< ITask > GetEnumerator( )
        {
            var l = new List< ITask >( ) ;
            foreach ( string eachMoniker in _monikers )
            {
                l.Add( _taskLookup[ eachMoniker ] ) ;
            }

            return l.GetEnumerator( ) ;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator( ) ;
        }

        #endregion

        static KeyValuePair<string, string> createArg( string commandLineArgument )
        {
            var regex = new Regex(
                @"\-(?<key>[^:]+)(?:\:(?<value>[^|]+))?", RegexOptions.Singleline); 
            
            Match match = regex.Match( commandLineArgument );
            string key = match.Groups[@"key"].Value;
            string value = match.Groups[ @"value" ].Value ;
            
            return new KeyValuePair< string, string >( key, value ) ;
        }

        void buildSettingsForPluginsBasedOnCommandLineValues( IEnumerable< string > args )
        {
            foreach(string eachPluginMoniker in args )
            {
                var monikerAndValue = createArg( eachPluginMoniker ) ;
                string moniker = monikerAndValue.Key ;
                string value = monikerAndValue.Value ;
                
                _monikers.Add( moniker ) ;
                if(!_taskLookup.ContainsKey( moniker ))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            @"Cannot find the plugin associated with '{0}'.", 
                            moniker ) ) ;
                }

                var pluginRuntimeSettings = new PluginRuntimeSettings( value );
                _taskLookup[ moniker ].Settings = pluginRuntimeSettings ;
            }
        }

        void populateLookupWithDiscoveredPlugins( IEnumerable< IPlugin > plugins )
        {
            foreach(IPlugin eachPlugin in plugins)
            {
                _taskLookup[ eachPlugin.Moniker ]
                    = new Task
                          {
                              Plugin = eachPlugin
                          } ;
            }
        }
    }
}