using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rts.Common
{
    public class TypeCheckPair<T>
    {
        public T Value { get; set; } = default!;
        public bool IsChecked { get; set; }

    }
}
