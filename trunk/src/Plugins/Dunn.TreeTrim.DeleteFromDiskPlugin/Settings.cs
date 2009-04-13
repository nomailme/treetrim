using System ;
using System.Xml.Linq ;

namespace Dunn.TreeTrim.DeleteFromDiskPlugin
{
    public static class Settings
    {
        static Settings( )
        {
            const string filenameWithoutExtension = @"DeleteFromDiskPlugin.settings" ;
            XDocument document = SettingsHelper.LoadAndValidate( filenameWithoutExtension  ) ;

            XElement settingsElement = document.Element( @"Settings" ) ;
            FilePattern = settingsElement.Element(@"FilePattern").Value;
            FolderPattern = settingsElement.Element(@"FolderPattern").Value;
        }

        public static string FilePattern { get; private set ; }
        public static string FolderPattern { get; private set ; }
    }
}