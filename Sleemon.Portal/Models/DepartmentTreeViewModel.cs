using System.Collections.Generic;

namespace Sleemon.Portal.Models
{
    public class DepartmentTreeViewModel
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public short Level { get; set; }

        public string Name { get; set; }

        public IEnumerable<DepartmentTreeViewModel> ChildrenCollection { get; set; }
    }
}
