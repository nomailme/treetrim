using System.Collections.Generic ;

namespace Dunn.TreeTrim
{
    /// <summary>
    /// Represents a collection of <see cref="ITask"/>s.
    /// </summary>
    public interface ITaskCollection : IEnumerable<ITask>
    {
    }
}