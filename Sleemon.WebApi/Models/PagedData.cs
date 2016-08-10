namespace Sleemon.WebApi
{
    using Newtonsoft.Json;

    using System.Collections.Generic;

    public class PagedData<T>
    {
        public IList<T> Data { get; set; }

        public int TotalCount { get; set; }
    }
}
