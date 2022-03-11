using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace Winlit
{
    class  litEditPanel<T> : Panel
    {

        private CheckBox editmode;
        public bool CanEdit
        {
            get { return editmode.Checked; }
            set
            {
                editmode.Checked = value;
            }
        }


        private DataGridView target;

        public DataGridView Target
        {
            get { return target; }
            set
            {
                target = value;
                Init();
            }
        }

        ComboBox cb;
        TextBox tb;
        public litEditPanel()
        {



        }

        public void Init()
        {

            // Search bar
            tb = new TextBox();
            tb.TextChanged += Tb_TextChanged;
            tb.Location = new Point(5, 10);
            this.Controls.Add(tb);

            cb = new ComboBox();
            foreach (DataGridViewColumn cl in target.Columns)
            {
                if (cl.Visible)
                    cb.Items.Add(cl.Name);

            }
            cb.Width = 100;
            cb.SelectedIndex = 0;
            cb.Location = Point.Add(tb.Location, new Size(tb.Width, 0));
            this.Controls.Add(cb);

            Label lbl = new Label();
            lbl.Text = "Edit Mode";
            lbl.Font = Fonts.Regular;
            lbl.Location = Point.Add(tb.Location, new Size(0, 25));
            lbl.AutoSize = true;
            this.Controls.Add(lbl);

            editmode = new CheckBox();
            editmode.Checked = false;
            editmode.Click += Editmode_Click; ;
            editmode.AutoCheck = false;
            editmode.Location = Point.Add(lbl.Location, new Size(lbl.Width, 0));
            this.Controls.Add(editmode);

            
        }

       

        private void Editmode_Click(object sender, EventArgs e)
        {
            CheckBox a = sender as CheckBox;
            if (a.Checked)
                a.Checked = false;
            else
            {
                if (MessageBox.Show("You are about to enter editing mode. \n Are you sure?", "Alert", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    a.Checked = false;
                }
                else
                {
                    a.Checked = true;
                   
                }
            }

            target.ReadOnly = !a.Checked;
        }



        private void Tb_TextChanged(object sender, EventArgs e)
        {
            if (target.DataSource is DataTable)
                (target.DataSource as DataTable).DefaultView.RowFilter = string.Format(cb.SelectedItem.ToString() + " LIKE '%{0}%'", tb.Text);
            else
            {
                if (tb.Text == "" || tb.Text == null)
                    foreach (DataGridViewRow r in target.Rows)
                        r.Visible = true;
                foreach (DataGridViewRow row in target.Rows)
                {
                    if (!row.Cells[cb.SelectedItem.ToString()].Value.ToString().ToLower().Contains(tb.Text.ToLower()))
                    {
                        CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[target.DataSource];
                        currencyManager1.SuspendBinding();
                        row.Visible = false;
                        currencyManager1.ResumeBinding();
                    }
                    else
                        row.Visible = true;
                }
            }

        }

    }
}
