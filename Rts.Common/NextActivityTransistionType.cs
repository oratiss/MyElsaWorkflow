using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rts.Common
{
    public enum NextActivityTransistionType
    {
        None = 0,
        Normal = 5,
        SelectByLogic = 10,
        SelectByUser = 15
    }
}
