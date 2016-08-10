namespace Sleemon.Data
{
    using System;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class RoleListModel
    {
        public IList<RoleExtend> listRole { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
    public class RoleExtend
    {
        public Role role{get;set;}
        public string permissions { get; set; }
    }
}
