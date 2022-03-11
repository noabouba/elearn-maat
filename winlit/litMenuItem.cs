using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winlit
{
    public class litMenuItem
    {
        private Panel pnl;

        public Panel ViewPanel
        {
            get { return pnl; }
            set { pnl = value; }
        }

        private litButton btn;

        public litButton Button
        {
            get { return btn; }
            set { btn = value; }
        }

        public litMenuItem(string text)
        {
            btn = new litButton(text);
            btn.ForeColor = Pallete.LightGray;
           
        }
    }
}
