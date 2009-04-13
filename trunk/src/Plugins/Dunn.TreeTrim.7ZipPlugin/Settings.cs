using System.Xml.Linq ;
using System.Xml.XPath ;

namespace Dunn.TreeTrim._7ZipPlugin
{
    public static class Settings
    {
        static Settings( )
        {
            XDocument document = SettingsHelper.LoadAndValidate( @"7ZipPlugin.settings" ) ;

            PathTo7ZipBinary = document.XPathSelectElement( @"Settings/PathTo7ZipBinary" ).Value ;
            WriteTo = valueOrEmpty( document, @"Settings/WriteTo" ) ;
            Password = valueOrEmpty( document, "Settings/Password" ) ;
        }

        public static string Password { get; private set ; }
        public static string PathTo7ZipBinary { get; private set ; }
        public static string WriteTo { get; private set; }

        static string valueOrEmpty( XNode node, string xpath )
        {
            XElement element = node.XPathSelectElement( xpath ) ;
            return element != null ? element.Value : string.Empty ;
        }
    }
}