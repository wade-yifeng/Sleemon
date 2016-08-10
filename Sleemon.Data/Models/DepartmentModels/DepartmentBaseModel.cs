namespace Sleemon.Data
{
    public class DepartmentBaseModel
    {
        /// <summary>
        /// Department unique id, also departmentid come from Wechat
        /// </summary>
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public int Order { get; set; }

        public string ParentHierarchyPath { get; set; }
    }
}
