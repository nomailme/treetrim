namespace Dunn.TreeTrim.Specs
{
    public class StubPlugin : IPlugin
    {
        readonly string _moniker ;

        public StubPlugin( string moniker )
        {
            _moniker = moniker ;
        }

        #region IPlugin Members

        public void Run( IPluginRuntimeSettings settings, IPlugin lastPlugin )
        {
            
        }

        public void Cleanup( )
        {
            
        }

        public string WorkingPath
        {
            get { return string.Empty ; }
        }

        public string Moniker
        {
            get { return _moniker ; }
        }

        #endregion
    }
}