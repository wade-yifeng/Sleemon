using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleemon.Data.Models
{
    public class NoticeListModel
    {
        public IList<EnterpriseNotice> NoticeList { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
