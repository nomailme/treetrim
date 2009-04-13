namespace Dunn.TreeTrim
{
    /// <summary>
    /// Represents types that provide runtime settings for <see cref="IPlugin"/> objects.
    /// Normally, these are supplied on the command line.
    /// </summary>
    public interface IPluginRuntimeSettings
    {
        /// <summary>
        /// Gets the settings value.  This type
        /// exposes data in key/value pairs.  This property is the whole string
        /// for those pairs.  
        /// <example>-zip:writeTo:"c:\temp\my.zip"+dontCleanUp</example>
        /// </summary>
        /// <value>The settings value.</value>        string Value { get ; }
        string Value { get ; }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified property name.
        /// </summary>
        /// <param name="propertyName">The property's name.</param>
        /// <value>The value of the property.</value>
        string this[ string propertyName ]
        {
            get;
        }

        /// <summary>
        /// Determines whether the property exists.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// <c>true</c> if the property exists; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsPropertyNamed(string propertyName) ;
    }
}