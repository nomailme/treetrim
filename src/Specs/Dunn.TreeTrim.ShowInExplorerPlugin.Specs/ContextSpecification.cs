namespace Dunn.TreeTrim.ShowInExplorerPlugin.Specs
{
	public abstract class ContextSpecification
	{
		protected ContextSpecification()
		{
// ReSharper disable DoNotCallOverridableMethodsInConstructor
			EstablishContext( ) ;
			Because( ) ;
// ReSharper restore DoNotCallOverridableMethodsInConstructor
		}

		public abstract void EstablishContext();
		public abstract void Because();
	}
}