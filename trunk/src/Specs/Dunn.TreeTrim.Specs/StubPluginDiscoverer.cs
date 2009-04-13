using System ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.Linq ;

namespace Dunn.TreeTrim.Specs
{
    public class StubPluginDiscoverer : IDiscoverPlugins
    {
        readonly List<IPlugin> _plugins  ;

        public StubPluginDiscoverer( params string[] monikers )
        {
            
            _plugins =  (from string moniker in monikers select new StubPlugin( moniker )).Cast<IPlugin>(  ).ToList(  )   ;
        }


        public IEnumerable< IPlugin > DiscoveredPlugins
        {
            get { return new ReadOnlyCollection< IPlugin >( _plugins ) ; }
        }
    }
}