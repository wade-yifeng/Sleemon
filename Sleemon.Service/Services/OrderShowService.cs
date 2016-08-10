namespace Sleemon.Service
{
    using System;
    using System.Linq;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using Sleemon.Core;
    using Sleemon.Data;
    using Sleemon.Common;
    using System.Transactions;
    using System.Configuration;

    public class OrderShowService : IOrderShowService
    {
        protected readonly string STATIC_RESOURCES_DOMAIN = ConfigurationManager.AppSettings["STATIC_RESOURCES_DOMAIN"];
        private readonly ISleemonEntities _invoicingEntities;
        private readonly IUserService userServices;

        public OrderShowService([Dependency] IUserService userServices)
        {
            this._invoicingEntities = new SleemonEntities();
            this.userServices = userServices;
        }
        private TransactionScopeOption _TransactionScopeOption = TransactionScopeOption.Required;

        private TransactionOptions _TransactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.RepeatableRead,
            Timeout = TimeSpan.FromMinutes(1)
        };

        public ResultBase PostOrderShowRequest(string userUniqueId, string filePath, string description)
        {
            ResultBase result = new ResultBase();
            //校验userUniqueId是否合法
            User user = userServices.GetUserById(userUniqueId);
            if (user == null)
            {
                result.StatusCode = 100;
                result.Message = "参数传入不合法";
                return result;
            }
            if (string.IsNullOrEmpty(filePath) && string.IsNullOrEmpty(description))
            {
                result.StatusCode = 101;
                result.Message = "提交内容不能为空";
                return result;
            }
            using (TransactionScope tran = new TransactionScope(_TransactionScopeOption, _TransactionOptions))
            {

                //insert UserOrderShow
                int userOrderShowId = InsertUserOrderShow(userUniqueId);
                //insert OrderShowFile
                int orderShowFileId = InsertOrderShowFile(userOrderShowId, filePath, description);
                tran.Complete();
            }
          
            result.StatusCode = 0;
            result.Message = "提交成功";
            return result;
        }

        public OrderShowResponse GetOrderShowList(int pageIndex, int pageSize, bool IsLegal, string userUniqueId,string userName, DateTime? startShowTime, DateTime? endShowTime)
        {
            OrderShowResponse response = new OrderShowResponse();
            ResultBase result = new ResultBase();
            List<OrderShowModel> returnList = new List<OrderShowModel>();
          
            var orderShowList = from uo in this._invoicingEntities.UserOrderShow
                                join of in this._invoicingEntities.OrderShowFile on uo.Id equals of.OrderShowId
                                join u  in this._invoicingEntities.User on uo.UserUniqueId equals u.UserUniqueId
                                where (IsLegal == true && uo.LegalStatus != (byte)LegalStatus.Illegal || IsLegal == false && uo.LegalStatus == (byte)LegalStatus.Illegal) 
                                    && uo.IsActive == true 
                                    && (string.IsNullOrEmpty(userUniqueId)||u.UserUniqueId==userUniqueId)
                                    && (string.IsNullOrEmpty(userName) || u.Name.IndexOf(userName)>=0)
                                    && (startShowTime==null ||uo.ShowTime>=startShowTime)
                                    && (endShowTime == null || uo.ShowTime <= endShowTime)
                                select new
                                {
                                    UserOrderShowId = uo.Id,
                                    UserName=u.Name,
                                    UserAvatar=u.Avatar,
                                    StaticResDomin= STATIC_RESOURCES_DOMAIN,
                                    FilePath = of.FilePath,
                                    Description=of.Description,
                                    LastUpdateTime=uo.LastUpdateTime
                                };
             if (orderShowList == null || orderShowList.ToList() == null)
            {
                response.TotalCount = 0;
                response.Result = result;
                response.OrderShowList = returnList;
                return response;
            }
             response.TotalCount = orderShowList.ToList().Count;
            var orderShowPagedList = orderShowList.OrderByDescending(p => p.LastUpdateTime).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            if (orderShowPagedList == null || orderShowPagedList.ToList() == null)
            {
                response.Result = result;
                response.OrderShowList = returnList;
                return response;
            }
            for (int i = 0; i < orderShowPagedList.ToList().Count; i++)
            {
                OrderShowModel model = new OrderShowModel();
                model.UserOrderShowId = orderShowPagedList.ToList()[i].UserOrderShowId;
                model.UserName = orderShowPagedList.ToList()[i].UserName;
                model.UserAvatar = orderShowPagedList.ToList()[i].UserAvatar;
                model.FilePath = orderShowPagedList.ToList()[i].FilePath;
                model.Description = orderShowPagedList.ToList()[i].Description;
                model.UserUniqueId = userUniqueId;
                model.StaticResDomin = orderShowList.ToList()[i].StaticResDomin;
                returnList.Add(model);
            }
            response.Result = result;
            response.OrderShowList = returnList;
            return response;
        }

        public ResultBase DeleteOrderShowRequest(int userOrderShowId)
        {
            ResultBase result = new ResultBase();
            UserOrderShow userOrderShow = this._invoicingEntities.UserOrderShow.FirstOrDefault(p => p.Id == userOrderShowId);
            OrderShowFile orderShowFile=  this._invoicingEntities.OrderShowFile.FirstOrDefault(p => p.OrderShowId == userOrderShowId);
            if (userOrderShow == null || orderShowFile == null)
            {
                result.StatusCode = 100;
                result.Message = "要删除的记录不存在";
                return result;
            }
            orderShowFile.IsActive = false;
            userOrderShow.IsActive = false;
            int dbResult= this._invoicingEntities.SaveChanges();
            if (dbResult <= 0)
            {
                result.StatusCode = 102;
                result.Message = "删除失败";
                return result;
            }
            result.StatusCode = 0;
            result.Message = "删除成功";
            return result;
          
        }

        public ResultBase SetOrderShowIsLegal(int userOrderShowId, bool IsLegal)
        {
            ResultBase result = new ResultBase();
            UserOrderShow userOrderShow = this._invoicingEntities.UserOrderShow.FirstOrDefault(p => p.Id == userOrderShowId);
            if (userOrderShow == null )
            {
                result.StatusCode = 100;
                result.Message = "要设置的记录不存在";
                return result;
            }
            userOrderShow.LegalStatus = IsLegal ? (byte)LegalStatus.Legal : (byte)LegalStatus.Illegal;
            int dbResult = this._invoicingEntities.SaveChanges();
            if (dbResult <= 0)
            {
                result.StatusCode = 102;
                result.Message = "设置失败";
                return result;
            }
            result.StatusCode = 0;
            result.Message = "设置成功";
            return result;
        }

        public int InsertUserOrderShow(string userUniqueId)
        {
            UserOrderShow userOrderShow = this._invoicingEntities.UserOrderShow.Create();
            userOrderShow.IsActive = true;
            userOrderShow.LastUpdateTime = DateTime.UtcNow;
            userOrderShow.ShowTime = DateTime.UtcNow;
            userOrderShow.UserUniqueId = userUniqueId;
            userOrderShow.LegalStatus = (byte)LegalStatus.UnCheck;
            UserOrderShow userOrderShowNew= this._invoicingEntities.UserOrderShow.Add(userOrderShow);
            this._invoicingEntities.SaveChanges();
            return userOrderShow.Id;
        }
        public int InsertOrderShowFile(int orderShowId,string filePath, string description)
        {
            OrderShowFile orderShowFile = this._invoicingEntities.OrderShowFile.Create();
            orderShowFile.Description = description;
            orderShowFile.FilePath = filePath;
            orderShowFile.IsActive = true;
            orderShowFile.LastUpdateTime = DateTime.UtcNow;
            orderShowFile.OrderShowId = orderShowId;
            OrderShowFile orderShowFileNew=this._invoicingEntities.OrderShowFile.Add(orderShowFile);
            this._invoicingEntities.SaveChanges();
            return orderShowFile.Id;
        }
    }
}
