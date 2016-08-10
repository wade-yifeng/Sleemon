using System.Collections.Generic;
namespace Sleemon.Data
{
    public class OrderShowResponse
    {
        public ResultBase Result { get; set; }
        public List<OrderShowModel> OrderShowList {get;set; }
        public int TotalCount { get; set; }
    }
}
