using System.Xml.Linq ;
using System.Xml.XPath ;

namespace Dunn.TreeTrim.SourceControlBindingRemovalPlugin
{
    public static class Settings
    {
        static Settings( )
        {
            XDocument document = SettingsHelper.LoadAndValidate( 
                @"SourceControlBindingRemovalPlugin.settings" ) ;

            Pattern = document.XPathSelectElement(@"Settings/Pattern" ).Value ;
        }

        public static string Pattern { get; private set ; }
    }
}