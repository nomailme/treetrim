using System ;
using System.ComponentModel.Composition ;
using System.Globalization ;
using System.IO;

namespace Dunn.TreeTrim.WorkingCopyPlugin
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        string _workingPath ;
        readonly Disk _disk ;
        IPluginRuntimeSettings _runtimeSettings ;

        public Plugin( ) : this( new Disk( ) )
        {
            
        }

        Plugin( Disk disk )
        {
            _disk = disk ;
        }

        #region IPlugin Members

        public void Run(IPluginRuntimeSettings settings, IPlugin lastPlugin)
        {
            _runtimeSettings = settings ;

            if(_runtimeSettings.ContainsPropertyNamed( @"writeTo" ))
            {
                _workingPath = settings[ @"writeTo" ];
            }
            else
            {
                _workingPath = getTempLocation( lastPlugin.WorkingPath) ;
            }

            _disk.CopyFolder( lastPlugin.WorkingPath, _workingPath );
        }

        public void Cleanup()
        {
            if ( _runtimeSettings.ContainsPropertyNamed( @"dontCleanUp" ) )
            {
                return ;
            }
         
            _disk.DeleteFileOrDirectory( _workingPath ) ;
        }

        public string WorkingPath
        {
            get { return _workingPath ; }
        }

        public string Moniker
        {
            get { return @"workingCopy"; }
        }

        #endregion

        static string getTempLocation( string path )
        {
            string filename = Path.GetFileNameWithoutExtension(path);
            
            string tempFilename = string.Format( 
                CultureInfo.InvariantCulture,
                @"{0}\{1}",
                Guid.NewGuid( ),
                filename ) ;
            
            string tempPath = Path.Combine( Path.GetTempPath( ), tempFilename ) ;
            
            return tempPath ;
        }
    }
}