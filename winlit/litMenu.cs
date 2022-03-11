using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winlit
{
    public class litMenu
    {
        private Panel menuPanel;
        public static string current;
        public Panel ViewPanel
        {
            get { return menuPanel; }
        }

        private Dictionary<string,litMenuItem> pairs;

        public litMenu(Panel pnl)
        {
            this.menuPanel = pnl;

            this.pairs = new Dictionary<string, litMenuItem>();
        }

        public void AddItem(litMenuItem item)
        {
            this.pairs[item.Button.Text] = item;
            this.menuPanel.Controls.Add(item.Button);
        }

        public litMenuItem GetItem(string title)
        {
            return pairs[title];
        }
        
    }
}
