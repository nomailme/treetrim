using System ;
using System.ComponentModel.Composition ;
using System.IO ;
using System.Text.RegularExpressions ;

namespace Dunn.TreeTrim.SourceControlBindingRemovalPlugin
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        static readonly Regex _projectOrSolutionPattern = 
            new Regex( Settings.Pattern, RegexOptions.IgnoreCase);

        string _workingPath ;
        readonly IDisk _disk ;

        public Plugin( ) : this( new Disk( ) )
        {
            
        }

        Plugin( IDisk disk )
        {
            _disk = disk ;
        }

        #region IPlugin Members

        public void Run(IPluginRuntimeSettings settings, IPlugin lastPlugin)
        {
            _workingPath = lastPlugin.WorkingPath ;
            
            analayse(lastPlugin.WorkingPath);
        }

        public void Cleanup( )
        {
            
        }

        public string WorkingPath
        {
            get { return _workingPath ; }
        }

        public string Moniker
        {
            get { return @"removeSourceControlBindings"; }
        }

        #endregion

        void analayse(string pathToDirectory)
        {
            var directoryInfo = new DirectoryInfo(pathToDirectory);

            getSolutionFilesFromDirectory(directoryInfo);

            searchThroughChildDirectories(directoryInfo);
        }

        void getSolutionFilesFromDirectory(DirectoryInfo directoryInfo)
        {
            FileInfo[] fileInfos = directoryInfo.GetFiles();

            foreach ( var eachFileInfo in fileInfos )
            {
                if(isSolutionFile( eachFileInfo.Name ))
                {
                    removeBindings( eachFileInfo.FullName ) ;
                }
            }
        }

        static bool isSolutionFile(string name)
        {
            return _projectOrSolutionPattern.IsMatch(name);
        }


        void removeBindings( string path)
        {
            string oldFileContents = _disk.ReadAllText(path);
            string newFileContents = oldFileContents;

            //-- remove any GlobalSection(SourceCodeControl) block
            newFileContents = Regex.Replace(
                newFileContents, "\\s+GlobalSection\\(SourceCodeControl\\)[\\w|\\W]+?EndGlobalSection", string.Empty);
            
            //-- remove TFS stuff
            newFileContents = Regex.Replace(
                newFileContents, "\\s+GlobalSection\\(TeamFoundationVersionControl\\)[\\w|\\W]+?EndGlobalSection", string.Empty);

            //-- remove any remaining lines that have keys beginning with 'Scc'
            newFileContents = Regex.Replace(
                newFileContents, @"^\s+Scc.*[\n\r\f]",
                string.Empty,
                RegexOptions.Multiline);

            //-- remove any remaining lines that have xml elements beginning with 'Scc'
            newFileContents = Regex.Replace(
                newFileContents, @"^\s+<Scc.*[\n\r\f]",
                string.Empty,
                RegexOptions.Multiline);

            if( newFileContents != oldFileContents )
            {
                _disk.WriteTextToFile( path, newFileContents ) ;
            }
        }

        void searchThroughChildDirectories(DirectoryInfo directoryInfo)
        {
            foreach (DirectoryInfo eachDirectoryInfo in directoryInfo.GetDirectories())
            {
                analayse(eachDirectoryInfo.FullName);
            }
        }
    }
}