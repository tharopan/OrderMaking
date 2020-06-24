using OrderMaking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMaking.Templates
{
    public partial class LabelItemTemplate : IDisposable
    {
        public IList<LabelItem> LabelItems { get; set; }

        public void Dispose() { }
    }
}
