using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winlit
{
    public class litButton : Button
    {
        public litButton(string text)
        {
            base.FlatStyle = FlatStyle.Flat;
            base.Text = text;
            base.AutoSize = true;

            base.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
          //  base.ForeColor = System.Drawing.Color.Transparent;
            base.FlatAppearance.BorderSize = 0;
            this.Cursor = Cursors.Hand;
        }
        

    }
}
