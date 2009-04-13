using System ;
using System.Collections.Generic ;
using System.ComponentModel.Composition ;
using System.IO ;

namespace Dunn.TreeTrim.SubversionRemovalPlugin
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        string _workingPath ;

        readonly IDisk _disk;

        public Plugin()
            : this( new Disk( ) )
        {

        }

        Plugin(IDisk disk)
        {
            _disk = disk;
        }


        #region IPlugin Members

        public void Run(IPluginRuntimeSettings settings, IPlugin lastPlugin)
        {
            _workingPath = lastPlugin.WorkingPath ;

            IEnumerable< string > childDirectories = _disk.GetChildDirectoriesRecursively( _workingPath ) ;
            foreach ( string eachDirectory in childDirectories )
            {
                if( isSubversionFolder(eachDirectory) )
                {
                    _disk.DeleteFileOrDirectory( eachDirectory );
                }
            }
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
            get { return @"removeSubversionDirectories" ; }
        }

        #endregion

        bool isSubversionFolder( string pathToDirectory )
        {
            if(!_disk.IsFolder( pathToDirectory ))
            {
                return false ;
            }

            if( StringComparer.InvariantCultureIgnoreCase.Compare( 
                Path.GetFileName( pathToDirectory ),
                @".svn" ) == 0 )
            {
                return true ;
            }

            if( StringComparer.InvariantCultureIgnoreCase.Compare( 
                Path.GetFileName( pathToDirectory ),
                @"_svn" ) == 0 )
            {
                return true ;
            }

            return false ;
        }
    }
}