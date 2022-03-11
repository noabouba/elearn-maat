using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winlit
{
    public partial class ObjectPanel<T> : UserControl
    {
        private object target;

        public object Target
        {
            get { return target; }
        }

        public ObjectPanel(object targ)
        {
            InitializeComponent();
            this.target = targ;
            this.Load += ObjectPanel_Load;

        }

        private void ObjectPanel_Load(object sender, EventArgs e)
        {
            T load = ConvertTarget();
            Type[] accepted = { typeof(string), typeof(int), typeof(DateTime) };
            var props = load.GetType().GetProperties().ToList();
            foreach (var property in props)
            {
                if(property.CanRead || property.CanWrite && accepted.Contains(property.PropertyType))
                {
                    
                }
            }


        }

        private T ConvertTarget()
        {
            if (this.target is T)
            {
                return (T)this.target;
            }
            else {
                try
                {
                    return (T)Convert.ChangeType(target, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
        }
    }
}
