using System.ComponentModel.Composition ;
using System.IO ;
using System.Text.RegularExpressions ;

namespace Dunn.TreeTrim.DeleteFromDiskPlugin
{
    /// <summary>
    /// The Delete From Disk Plugin.  Based on the excellent CleanSourcesPlus
    /// tool (http://www.codinghorror.com/blog/archives/000368.html).
    /// </summary>
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        static readonly Regex _directoryDeletionPattern =
            new Regex(Settings.FolderPattern, RegexOptions.IgnoreCase);

        static readonly Regex _fileDeletionPattern = 
            new Regex(Settings.FilePattern, RegexOptions.IgnoreCase);

        readonly IDisk _disk;

        string _workingPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        public Plugin()
            : this( new Disk( ) )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="disk">The disk.  Injection point for DI containers.</param>
        Plugin(IDisk disk)
        {
            _disk = disk;
        }

        /// <summary>
        /// Gets the moniker associated with this plugin.  A plugin is associated with a moniker,
        /// which is essentially a friendly name to identifiy the plugin.
        /// </summary>
        /// <value>The moniker.</value>
        public string Moniker
        {
            get { return @"deleteFromDisk" ; }
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
        }

        /// <summary>
        /// Runs the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="lastPlugin">The last plugin run.</param>
        public void Run(IPluginRuntimeSettings settings, IPlugin lastPlugin)
        {
            _workingPath = lastPlugin.WorkingPath ;
            
            analyse(lastPlugin.WorkingPath);
        }

        static bool isRecognisedDebugFolder(FileSystemInfo folderInfo)
        {
            return _directoryDeletionPattern.IsMatch(folderInfo.Name);
        }

        static bool shouldDeleteThisFile(string name)
        {
            return _fileDeletionPattern.IsMatch(name);
        }

        void analyse(string pathToDirectory)
        {
            var directoryInfo = new DirectoryInfo(pathToDirectory);

            deleteFilesFromDirectory(directoryInfo);

            deleteChildDirectories(directoryInfo);
        }

        void deleteChildDirectories(DirectoryInfo directoryInfo)
        {
            foreach (DirectoryInfo eachDirectoryInfo in directoryInfo.GetDirectories())
            {
                if (isRecognisedDebugFolder(eachDirectoryInfo))
                {
                    _disk.DeleteFileOrDirectory( eachDirectoryInfo.FullName + @"\" ) ;
                }
                else
                {
                    analyse( eachDirectoryInfo.FullName ) ;
                }
            }
        }

        void deleteFilesFromDirectory(DirectoryInfo directoryInfo)
        {
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach ( var eachFileInfo in fileInfos )
            {
                if(shouldDeleteThisFile( eachFileInfo.Name ))
                {
                    _disk.DeleteFileOrDirectory( eachFileInfo.FullName ); 
                }
            }
        }
    }
}