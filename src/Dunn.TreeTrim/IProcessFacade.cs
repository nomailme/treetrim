namespace Dunn.TreeTrim
{
	/// <summary>
	/// Represents an interface to <see cref="ProcessFacade"/> type. This is here
	/// to enable unit testing of the plugins.
	/// </summary>
	public interface IProcessFacade
	{
		/// <summary>
		/// Starts a process with arguments.
		/// </summary>
		/// <param name="filename">The path to the file to execute</param>
		/// <param name="arguments">The arguments to pass to the process</param>
		void Start( string filename, string arguments ) ;
	}
}