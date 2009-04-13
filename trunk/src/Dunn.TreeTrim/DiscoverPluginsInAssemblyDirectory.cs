using System ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel.Composition ;
using System.ComponentModel.Composition.Hosting ;

namespace Dunn.TreeTrim
{
    /// <summary>
    /// Represents a type that discovers plugins in the current assembly's directory.
    /// </summary>
    public class DiscoverPluginsInAssemblyDirectory : IDiscoverPlugins
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoverPluginsInAssemblyDirectory"/> class.
        /// </summary>
        public DiscoverPluginsInAssemblyDirectory( )
        {
            var disk = new Disk( );

            string thisDirectory = disk.DirectoryOfExecutingAssembly ;

            var catalog = new DirectoryCatalog(thisDirectory);

            var container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();
            batch.AddPart(this);

            container.Compose(batch);
        }

        /// <summary>
        /// Gets or sets the plugins.
        /// </summary>
        /// <value>The plugins.</value>
        [Import( typeof( IPlugin ) )]
        public IList<IPlugin> Plugins
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the dicovered plugins.
        /// </summary>
        /// <value>The dicovered plugins.</value>
        public IEnumerable< IPlugin > DiscoveredPlugins
        {
            get { return new ReadOnlyCollection< IPlugin >( Plugins ) ; }
        }
    }
}