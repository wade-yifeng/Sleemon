namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;
using System;

    public interface IOrderShowService
    {
        ResultBase PostOrderShowRequest(string userUniqueId,string filePath,string description);
        OrderShowResponse GetOrderShowList(int pageIndex, int pageSize, bool IsLegal, string userUniqueId,string userName, DateTime? startShowTime, DateTime? endShowTime);
        ResultBase DeleteOrderShowRequest( int userOrderShowId);
        ResultBase SetOrderShowIsLegal( int userOrderShowId,bool IsLegal);
    }
}