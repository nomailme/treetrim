using System ;
using System.ComponentModel.Composition ;
using System.Globalization ;
using System.IO ;
using SevenZip ;

namespace Dunn.TreeTrim._7ZipPlugin
{
    /// <summary>
    /// The 7 Zip plugin.  Uses the excellent SevenZipSharp library (http://sevenzipsharp.codeplex.com/)
    /// </summary>
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        readonly IDisk _disk ;
        IPluginRuntimeSettings _runtimeSettings ;
        string _workingPath ;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        public Plugin( )
            : this(new Disk( ) )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="disk">The disk to use.  Injection point for DI containers.</param>
        Plugin( IDisk disk )
        {
            _disk = disk ;
        }

        #region IPlugin Members

        /// <summary>
        /// Gets the moniker associated with this plugin.  A plugin is associated with a moniker,
        /// which is essentially a friendly name to identifiy the plugin.
        /// </summary>
        /// <value>The moniker.</value>
        public string Moniker
        {
            get { return @"7zip" ; }
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
            get { return _workingPath ; }
        }

        /// <summary>
        /// Cleans up this instance.  For instance, if the plug writes to a file,
        /// it gets a chance here to delete the file.
        /// </summary>
        public void Cleanup( )
        {
            if (_runtimeSettings.ContainsPropertyNamed(@"dontcleanup"))
            {
                return ;
            }

            _disk.DeleteFileOrDirectory( _workingPath );
        }

        /// <summary>
        /// Performs some work based on the settings passed.
        /// </summary>
        /// <param name="settings">The settings.  These specify the settings that the plugin
        /// should use and normally contain key/value pairs of parameters.</param>
        /// <param name="lastPlugin">The last plugin run.  This is often useful as
        /// normally, plugins need to work with the output produced from the last plugin.</param>
        public void Run(IPluginRuntimeSettings settings, IPlugin lastPlugin)
        {
            _runtimeSettings = settings ;
            
            string lastPath = lastPlugin.WorkingPath ;

            _workingPath = getPathToSaveZippedFileTo( settings, lastPath ) ;

            if(File.Exists( _workingPath ))
            {
                File.Delete( _workingPath );
            }

            var compressor = new SevenZipCompressor();

            SevenZipLibraryManager.LibraryFileName = Settings.PathTo7ZipBinary ;

            string password = getPasswordIfSpecified( settings ) ;

            if( Directory.Exists( lastPath ))
            {
                compressor.CompressDirectory( 
                    lastPath,
                    _workingPath, 
                    OutArchiveFormat.SevenZip,
                    password );
            }
            else
            {
                compressor.CompressFiles(
                    new[] { lastPath },
                    _workingPath,
                    OutArchiveFormat.SevenZip,
                    password );
            }
        }

        #endregion

        static string getPasswordIfSpecified( IPluginRuntimeSettings runtimeSettings )
        {
            if(runtimeSettings.ContainsPropertyNamed( @"password" ))
            {
                return runtimeSettings[ @"password" ] ;
            }
            
            if(string.IsNullOrEmpty( Settings.Password ) )
            {
                return Settings.Password ;
            }

            return string.Empty ;
        }

        static string getPathToSaveZippedFileTo( IPluginRuntimeSettings runtimeSettings, string lastPath )
        {
            string currentPath = string.Empty ;
            if(runtimeSettings.ContainsPropertyNamed( @"writeto" ))
            {
                currentPath = runtimeSettings[ @"writeto" ] ;
            }

            if (string.IsNullOrEmpty(currentPath))
            {
                currentPath = Settings.WriteTo ;
            }

            if (string.IsNullOrEmpty(currentPath))
            {
                currentPath = getTempLocation(lastPath);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(currentPath));
            return currentPath;
        }

        static string getTempLocation(string path)
        {
            string filename = Path.GetFileNameWithoutExtension( path ) ;

            string zipFilename = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}\{1}.7z",
                Guid.NewGuid( ), filename ) ;
            
            string tempPath = Path.Combine( Path.GetTempPath( ), zipFilename ) ;

            return tempPath;
        }
    }
}
