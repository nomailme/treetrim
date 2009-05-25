using System;
using System.ComponentModel.Composition ;

namespace Dunn.TreeTrim.ShowInExplorerPlugin
{
	[Export(typeof(IPlugin))]
	public class Plugin	: IPlugin
	{
		string _workingPath;
		readonly IProcessFacade _processFacade ;

		/// <summary>
		/// Initializes a new instance of the <see cref="Plugin"/> class.
		/// </summary>
		public Plugin()
			: this(new ProcessFacade( ))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Plugin"/> class.
		/// </summary>
		/// <param name="processFacade">The process facade.  Injection point for DI containers.</param>
		public Plugin(IProcessFacade processFacade)
		{
			_processFacade = processFacade;
		}

		public string Moniker
		{
			get { return @"showInExplorer"; }
		}

		public string WorkingPath
		{
			get { return _workingPath; }
		}

		public void Cleanup()
		{
		}

		public void Run(IPluginRuntimeSettings settings, IPlugin lastPlugin)
		{
			if (lastPlugin == null)
			{
				throw new InvalidOperationException(
					@"The 'show in explorer' plugin cannot run because no previous plugin had run to show in Explorer.");
			}

			_workingPath = lastPlugin.WorkingPath;

			_processFacade.Start( 
				@"explorer.exe", 
				@"/select," + _workingPath ) ;
		}
	}
}
