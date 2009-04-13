using System.Collections.Generic ;
using System.Linq ;

namespace Dunn.TreeTrim
{
    /// <summary>
    /// Represents a type that trims a directory - normally a source tree.
    /// </summary>
    public static class Trimmer
    {
        /// <summary>
        /// Trims the tree by using the provided tasks.
        /// </summary>
        /// <param name="tasks">The tasks to run.  These are run in order.
        /// When finished, they are run in reverse order and given a chance to clean up.</param>
        /// <param name="sourceTreeRoot">The source tree root.</param>
        public static void TrimTree(ITaskCollection tasks, string sourceTreeRoot)
        {
            ITask lastTask = new Task { Plugin = new NullPlugin( sourceTreeRoot ) } ;

            foreach ( ITask eachTask in tasks )
            {
                eachTask.Run( lastTask );
                
                lastTask = eachTask ;
            }

            IEnumerable<ITask> reversedTasks = tasks.Reverse( ) ;

            foreach (ITask eachTask in reversedTasks)
            {
                eachTask.Cleanup();
            }
        }
    }
}