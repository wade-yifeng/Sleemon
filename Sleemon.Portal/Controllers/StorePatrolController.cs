using Sleemon.Common;
using Sleemon.Core;
using Sleemon.Data;

namespace Sleemon.Portal.Controllers
{
    using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

    public class StorePatrolController : BaseController
    {
        /// <summary>
        /// Exam list
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="taskTitle"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StorePatrolIndex()
        {
            ViewBag.pageIndex = 1;
            ViewBag.CurrentUserId = UserUniqueId;
            return PartialView("StorePatrolList");
        }

        [HttpGet]
        public ActionResult UserStorePatrolIndex()
        {
            ViewBag.pageIndex = 1;
            ViewBag.CurrentUserId = UserUniqueId;
            return PartialView("UserStorePatrolList");
        }

        [HttpGet]
        public ActionResult StorePatrolEdit(int? taskid)
        {
            TaskDetailsModel model = null;
            if(taskid!=null)
            {
                model = ServiceClient.Request<ITaskService, TaskDetailsModel>(
                    service => service.GetTaskDetailById(taskid.Value));
            }
            model.TaskId = Convert.ToInt32(taskid);
            return PartialView("StorePatrolEdit",model);
        }

        [HttpGet]
        public ActionResult PointStorePatrolIndex(int taskid,int userTaskId)
        {
            ViewBag.taskid = taskid;
            ViewBag.userTaskId = userTaskId;
            UserStorePatrolPointModel fullModel = new UserStorePatrolPointModel();
            UserStorePatrolDetailModel modelUserStorePatrol = ServiceClient
                .Request<IStorePatrolService, UserStorePatrolDetailModel>(
                    service => service.GetUserStorePatrolDetails(userTaskId));
            fullModel.UserStorePatrols = modelUserStorePatrol;
            TaskDetailsModel modelTask = ServiceClient.Request<ITaskService, TaskDetailsModel>(
                    service => service.GetTaskDetailById(taskid));
            fullModel.taskDetail = modelTask;
            return PartialView("PointStorePatrol", fullModel);
        }
  
        [HttpPost]
        public ActionResult PointStorePatrol(int taskid,int userTaskId,bool isPass, IEnumerable<UserStorePatrolModel> userStorePatrols)
        {

            ResultBase result = ServiceClient
                .Request<IStorePatrolService, ResultBase>(
                    service => service.PointStorePatrol(isPass, userStorePatrols));
            ViewBag.IsSuccess = result.IsSuccess;
            ViewBag.Message = result.Message;

            UserStorePatrolPointModel fullModel = new UserStorePatrolPointModel();
            UserStorePatrolDetailModel modelUserStorePatrol = ServiceClient
                .Request<IStorePatrolService, UserStorePatrolDetailModel>(
                    service => service.GetUserStorePatrolDetails(userTaskId));
            fullModel.UserStorePatrols = modelUserStorePatrol;
            TaskDetailsModel modelTask = ServiceClient.Request<ITaskService, TaskDetailsModel>(
                service => service.GetTaskDetailById(taskid));
            fullModel.taskDetail = modelTask;
            return PartialView("PointStorePatrol", fullModel);
        }
    
        [HttpGet]
        public ActionResult GetStorePatrolList(int pageIndex, int pageSize, string taskTitle,DateTime? startFrom,DateTime? endTo)
        {
            ViewBag.pageIndex = pageIndex;
            ViewBag.CurrentUserId = UserUniqueId;
            ViewBag.taskTitle = taskTitle;
            ViewBag.startFrom = startFrom;
            ViewBag.endTo = endTo;
            
            TaskSearchContext context = new TaskSearchContext();
            context.PageIndex = pageIndex;
            context.PageSize = pageSize;
            context.TaskName = taskTitle;
            context.StartFrom = startFrom;
            context.EndTo = endTo;
            context.TaskCategory = (byte)TaskCategory.StorePatrol;//巡店任务
            context.BelongTo = (int)TaskBelongTo.SingleTask;//单任务

            var modelList = ServiceClient.Request<ITaskService, IList<TaskListModel>>(
                service => service.GetTaskList(context));
            return PartialView("StorePatrolList", modelList);
        }

        [HttpPost]
        public ActionResult AddUpdateStorePatrol(TaskDetailsModel model)
        {
            model.TaskCategory = (byte)TaskCategory.StorePatrol;//巡店任务
            model.BelongTo = (int)TaskBelongTo.SingleTask;//单任务
            model.Status = (byte)ActionCategory.Save;//已保存
            model.LastUpdateUser = UserUniqueId;
            model.LastUpdateTime = DateTime.UtcNow;
            var result = ServiceClient.Request<ITaskService, ResultBase>(
                    service => service.SaveTaskDetail(model));

            ViewBag.IsSuccess = result.IsSuccess;
            ViewBag.Message = result.Message;
            return PartialView("StorePatrolEdit", model);
        }
  
        [HttpPost]
        public ActionResult PublishStorePatrol(int taskid, string scences, string users)
        {
            List<int> sceneIds = ConvertToList<int>(scences);
            List<string> userIds = ConvertToList<string>(users);
            TaskDetailsModel model = ServiceClient.Request<ITaskService, TaskDetailsModel>(
                    service => service.GetTaskDetailById(taskid));
            model.Status =(byte)ActionCategory.Publish;//已发布
            model.LastUpdateUser = UserUniqueId;
            model.LastUpdateTime = DateTime.UtcNow;
            model.SceneIds = sceneIds;
            model.UserIds = userIds;
            model.TaskId = taskid;
            model.DispatchSubject = model.Title;
            model.DispatchPriority = 3;//高
            model.DispatchType = 2;//立即发布
            model.Status = 2;// 发布

            var result = ServiceClient.Request<ITaskService, ResultBase>(
                    service => service.SaveTaskDetail(model));
            ViewBag.IsSuccess = result.IsSuccess;
            ViewBag.Message = result.Message;
            return Json(result);
        }
        
     
        [HttpPost]
        public ActionResult DeleteStorePatrol(int taskid)
        {
            var result = ServiceClient.Request<ITaskService, ResultBase>(
                service => service.DeleteTaskById(taskid));
            return Json(result);
        }
        
        [HttpGet]
        public ActionResult GetUserStorePatrolList(int pageIndex, int pageSize, string username, DateTime? startFrom, DateTime? endTo)
        {
            ViewBag.pageIndex = pageIndex;
            ViewBag.CurrentUserId = UserUniqueId;
            ViewBag.username = username;
            ViewBag.startFrom = startFrom;
            ViewBag.endTo = endTo;
            int totalCount = 0;
            UserStorePatrolListModel returnModel = new UserStorePatrolListModel();
            IList<UserStorePatrolPreviewModel> modelList = ServiceClient.Request<IStorePatrolService, IList<UserStorePatrolPreviewModel>>(
                    service => service.GetStorePatrolList(pageIndex, pageSize, username, startFrom, endTo, out totalCount));
            returnModel.ListUserTask = modelList;
            returnModel.PageIndex = pageIndex;
            returnModel.PageSize = pageSize;
            returnModel.TotalCount = totalCount;
            return PartialView("UserStorePatrolList", returnModel);
        }
        //[Authorization]
        //[HttpGet]
        //public ActionResult GetStorePatrolDetail(int userTaskId)
        //{
        //    UserStorePatrolDetailsModel model = storePatrolModelClient.GetUserStorePatrolDetails(userTaskId);
        //    return PartialView("PointStorePatrol", model);
        //}

        //
        private List<T> ConvertToList<T>(string sources)
        {
            List<T> list=new List<T>();
            string[] array = sources.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                if (!string.IsNullOrEmpty(array[i]))
                {
                    list.Add(GetValue<T>(array[i]));
                }
            }
            return list;
        }
        private static T GetValue<T>(String value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}