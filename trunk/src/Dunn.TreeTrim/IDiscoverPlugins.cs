using System.Collections.Generic ;

namespace Dunn.TreeTrim
{
    /// <summary>
    /// Represents types that discovers plugins.
    /// </summary>
    public interface IDiscoverPlugins 
    {
        /// <summary>
        /// Gets the dicovered plugins.
        /// </summary>
        /// <value>The dicovered plugins.</value>
        IEnumerable<IPlugin> DiscoveredPlugins { get ; }
    }
}