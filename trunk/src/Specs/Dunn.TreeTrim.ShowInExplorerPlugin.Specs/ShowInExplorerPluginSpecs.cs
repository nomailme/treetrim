using Microsoft.Practices.Unity ;
using Moq ;
using Xunit ;

namespace Dunn.TreeTrim.ShowInExplorerPlugin.Specs
{
	public class ShowInExplorerPluginSpecs
	{
		public class When_run : ContextSpecification
		{
			Plugin _plugin ;
			IPluginRuntimeSettings _runtimeSettings ;
			Mock< IProcessFacade > _processFacade ;
			const string PATH_TO_FILE = @"c:\file.zip";

			public override void Because()
			{
				_plugin.Run(
					_runtimeSettings,
					mockLastPlugin( PATH_TO_FILE ) ) ;
			}

			public override void EstablishContext()
			{
				using( var container = new UnityContainer( ) )
				{
					_processFacade = mockedProcessFacade( ) ;
					
					container.RegisterInstance<IProcessFacade>( _processFacade.Object );

					_plugin  = container.Resolve< Plugin >( ) ;

					_runtimeSettings = stubRuntimeSettings( string.Empty ) ;
				}
			}


			[Fact]
			public void it_should_start_explorer_and_pass_the_path_as_an_argument()
			{
				_processFacade.VerifyAll(  );
			}
			
			[Fact]
			public void it_should_return_the_correct_moniker()
			{
				Assert.Equal( @"showInExplorer", _plugin.Moniker );
			}

			[Fact]
			public void it_should_return_the_correct_working_path()
			{
				Assert.Equal( PATH_TO_FILE, _plugin.WorkingPath );
			}

			static Mock< IProcessFacade > mockedProcessFacade( )
			{
				var processFacade = new Mock< IProcessFacade >();

				processFacade.Setup( 
					t => t.Start(	It.Is<string>( x => x==@"explorer.exe" ), 
									It.Is<string>( x => x == @"/select,"+PATH_TO_FILE ) ) )
					.AtMostOnce( )
					.Verifiable( ) ;
				
				return processFacade ;
			}

			static IPlugin mockLastPlugin(string path)
			{
				return new NullPlugin( path );
			}

			static IPluginRuntimeSettings stubRuntimeSettings(string value)
			{
				return new PluginRuntimeSettings( value );
			}
		}

	}
}