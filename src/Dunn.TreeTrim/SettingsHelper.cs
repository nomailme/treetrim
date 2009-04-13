using System ;
using System.Globalization ;
using System.IO ;
using System.Xml ;
using System.Xml.Linq ;
using System.Xml.Schema ;

namespace Dunn.TreeTrim
{
    /// <summary>
    /// A helper class.  Hmmm.
    /// </summary>
    public static class SettingsHelper
    {
        /// <summary>
        /// Loads and validates the XML specified.  If no schema file exists, then validation is
        /// assumed to have succeeded.
        /// </summary>
        /// <param name="fileNameWithoutExtension">The file name without extension.</param>
        /// <returns>An <see cref="XDocument"/> that is validated against a 
        /// schema if a schema file exists.</returns>
        public static XDocument LoadAndValidate(string fileNameWithoutExtension )
        {
            var disk = new Disk( );
            
            string xmlPath = Path.Combine( 
                disk.DirectoryOfExecutingAssembly, 
                fileNameWithoutExtension + @".xml" ) ;

            string xsdPath = Path.Combine( 
                disk.DirectoryOfExecutingAssembly, 
                fileNameWithoutExtension + @".xsd" ) ;
            
            XDocument document = load( xmlPath ) ;

            validate( xsdPath, document ) ;

            return document ;
        }

        static void validate( string pathToXsd, XDocument document )
        {
            if ( !File.Exists( pathToXsd ) )
            {
                return ;
            }
            
            var schemaSet = new XmlSchemaSet( ) ;
            schemaSet.Add(
                string.Empty,
                new XmlTextReader( File.OpenRead( pathToXsd ) ) ) ;

            document.Validate(
                schemaSet,
                ( sender, validationEventArgs ) =>
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                @"Cannot load the settings for the plugin as the XML configuration is wrong: {0}.",
                                validationEventArgs.Message ) ) ;
                    },
                true ) ;
        }

        static XDocument load( string pathToXml )
        {
            return XDocument.Load( pathToXml ) ;
        }
    }
}