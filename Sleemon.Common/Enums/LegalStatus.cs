using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleemon.Common
{
    public enum LegalStatus
    {
        [Description("未审核")]
        UnCheck = 0,

        [Description("合法")]
        Legal = 1,

        [Description("非法")]
        Illegal = 2
    }
}
