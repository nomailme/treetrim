using System ;
using System.Collections.Generic ;
using System.Text.RegularExpressions ;

namespace Dunn.TreeTrim
{
    /// <summary>
    /// Represents a type that holds the runtime settings for an <see cref="IPlugin"/>.
    /// </summary>
    public class PluginRuntimeSettings : IPluginRuntimeSettings
    {
        readonly Dictionary< string, string > _propertyBag ;
        readonly string _value ;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginRuntimeSettings"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public PluginRuntimeSettings( string value )
        {
            _propertyBag = new Dictionary< string, string >( 
                StringComparer.CurrentCultureIgnoreCase ) ;

            _value = value ;

            populatePropertyBag( ) ;
        }

        /// <summary>
        /// Gets the settings value.  This type
        /// exposes data in key/value pairs.  This property is the whole string
        /// for those pairs.
        /// <example>-zip:writeTo:"c:\temp\my.zip"+dontCleanUp</example>
        /// </summary>
        /// <value>The settings value.</value>
        /// string Value { get ; }
        public string Value
        {
            get
            {
                return _value ;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <value>The property's value.</value>
        public string this[ string propertyName ]
        {
            get
            {
                return _propertyBag[ propertyName ];
            }

            set
            {
                _propertyBag[ propertyName ] = value;
            }
        }

        /// <summary>
        /// Determines whether the property exists.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// <c>true</c> if the property exists; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsPropertyNamed(string propertyName)
        {
            return _propertyBag.ContainsKey( propertyName ) ;
        }

        void populatePropertyBag( )
        {
            var regex = new Regex(
@"(?<key>.*?) # the stuff before the colon
   (?: # optional colon and stuff after it
      \:    # the colon
      (?<value>.*?) # the stuff after the colon
   )?
(?:\+|\z) # up to another + or the end of line
",
                RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            
            MatchCollection collection = regex.Matches(_value);

            foreach (Match eachMatch in collection)
            {
                string key = eachMatch.Groups[@"key"].Value ;
                string value = eachMatch.Groups[@"value"].Value ;
                
                _propertyBag[ key ] = value;
            }
        }
    }
}