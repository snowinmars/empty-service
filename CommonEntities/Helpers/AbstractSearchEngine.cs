using System.IO;

namespace EmptyService.CommonEntities.Helpers
{
    internal abstract class AbstractSearchEngine<T>
    {
        public abstract T[] FindAbove(T path, string pattern);

        public abstract T[] FindBelow(T path, string pattern, SearchOption searchOption);
    }
}