namespace Dunn.TreeTrim
{
	/// <summary>
	/// Represents a facade over the <see cref="ProcessFacade"/> type.
	/// </summary>
	public class ProcessFacade : IProcessFacade
	{
		/// <summary>
		/// Starts a process with arguments.
		/// </summary>
		/// <param name="filename">The path to the file to execute</param>
		/// <param name="arguments">The arguments to pass to the process</param>
		public void Start(string filename, string arguments)
		{
			System.Diagnostics.Process.Start( filename, arguments ) ;
		}
	}
}