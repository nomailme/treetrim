using System ;
using System.Collections.Generic ;
using System.ComponentModel.Composition ;
using System.Globalization ;
using System.IO ;
using ICSharpCode.SharpZipLib.Zip ;

namespace Dunn.TreeTrim.ZipPlugin
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        string _workingPath;
        IPluginRuntimeSettings _runtimeSettings ;

        #region IPlugin Members

        public void Run(IPluginRuntimeSettings settings, IPlugin lastPlugin)
        {
            _runtimeSettings = settings ;

            _workingPath = settings.ContainsPropertyNamed( @"writeTo" ) 
                ? settings[ @"writeTo" ] 
                : getTempLocation(lastPlugin.WorkingPath) ;

            if( !Directory.Exists( _workingPath ) )
            {
                Directory.CreateDirectory( Path.GetDirectoryName( _workingPath ) ) ;
            }

            zipFolder(lastPlugin.WorkingPath, _workingPath );
        }

        public void Cleanup()
        {
            if(_runtimeSettings.ContainsPropertyNamed( @"dontCleanUp" ))
            {
                return ;
            }

            File.Delete( _workingPath );
        }

        public string WorkingPath
        {
            get { return _workingPath; }
        }

        public string Moniker
        {
            get { return @"zip" ; }
        }

        #endregion

        static List<string> buildList(string directory)
        {
            var files = new List<string>();

            var thisLevel = new DirectoryInfo(directory);

            DirectoryInfo[] subDirectories = thisLevel.GetDirectories();

            foreach (DirectoryInfo eachSubDirectoryInfo in subDirectories)
            {
                files.AddRange(buildList(eachSubDirectoryInfo.FullName));
            }

            FileInfo[] fileInfos = thisLevel.GetFiles();
            foreach (FileInfo eachFileInfo in fileInfos)
            {
                files.Add(eachFileInfo.FullName);
            }

            return files;
        }

        static string getTempLocation( string path )
        {
            string filename = Path.GetFileNameWithoutExtension( path ) ;

            string tempFilename = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}\{1}.zip",
                Guid.NewGuid( ),
                filename ) ;
            
            string tempPath = Path.Combine( Path.GetTempPath( ), tempFilename ) ;
            
            return tempPath;
        }

        static void zipFolder(string basePath, string zipFileName)
        {
            if(!basePath.EndsWith( @"\", StringComparison.OrdinalIgnoreCase ) )
            {
                basePath = basePath + @"\" ;
            }
            
            using ( var outputStream = new ZipOutputStream( File.OpenWrite( zipFileName ) ) )
            {
                List<string> files = buildList(basePath);

                foreach (string eachFile in files)
                {
                    string sourceFileName = eachFile;

                    string path = Path.GetFullPath(sourceFileName).Replace(basePath, string.Empty);

                    var ze = new ZipEntry(path)
                                 {
                                     CompressionMethod = CompressionMethod.Deflated
                                 };
                    outputStream.PutNextEntry(ze);

                    using ( FileStream fileStream = File.OpenRead( sourceFileName ) )
                    {
                        var buff = new byte[1024];
                        int bytesRead ;
                        while ((bytesRead = fileStream.Read(buff, 0, buff.Length)) > 0)
                        {
                            outputStream.Write(buff, 0, bytesRead);
                        }

                        fileStream.Close();
                    }
                }

                outputStream.CloseEntry();
                outputStream.Close();
            }
        }
    }
}