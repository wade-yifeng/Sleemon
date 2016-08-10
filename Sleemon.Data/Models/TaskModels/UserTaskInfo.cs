using Sleemon.Common;
using System.Collections.Generic;

namespace Sleemon.Data
{
    /// <summary>
    /// 用户任务信息（包括单项任务和任务组）
    /// </summary>
    public class UserTaskInfo
    {
        /// <summary>
        /// 类型：单项任务/任务组
        /// </summary>
        public TaskBelongTo Type { get; set; }
        /// <summary>
        /// 任务组Id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 任务组的标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 接收任务组需要的等级
        /// </summary>
        public int RequiredGrade { get; set; }
        /// <summary>
        /// 子任务列表/单项任务
        /// </summary>
        public IEnumerable<UserTaskModel> SubTaskList { get; set; }
    }
}
