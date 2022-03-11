using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winlit
{
    class litGridView : DataGridView
    {
        private Func<int> Refresh;

        public Func<int> RefreshMethod
        {
            get { return Refresh; }
            set { Refresh = value; }
        }

        public litGridView()
        { }

    }
}
