using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winlit
{
    public partial class EditForm : Form
    {
        private class _TextBox<K> : TextBox
        {
            private K target;

            public K TargetObject
            {
                get { return target; }
                set { this.target = value; }
            }


            private Func<bool> validate;

            public Func<bool> Validator
            {
                get { return validate; }
                set { validate = value; }
            }

            private object firstVal;
            public object FirstValue
            {
                set { firstVal = value; }
                get { return firstVal; }
            }


            public _TextBox()
            {

            }


            public _TextBox(object obj)
            {
                this.target = (K)obj;
            }

            public K ToObj()
            {
                return (K)Convert.ChangeType(this.Text, typeof(K));
            }



        }

        #region Properties
        public delegate void CommitEditHandler(object sender, object newValue, EventArgs e);
        public event CommitEditHandler OnCommitEdit;

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                tt.Text = value;
            }
        }

        private Panel borderPanel;

        public Panel BorderPanel
        {
            get { return borderPanel; }
            set { borderPanel = value; }
        }

        private Panel infoPanel;

        public Panel InfoPanel
        {
            get { return infoPanel; }
            set { infoPanel = value; }
        }


        private DataGridViewCell target;

        public DataGridViewCell TargetObject
        {
            get { return target; }
            set { target = value; }
        }

        #endregion

        public EditForm(DataGridViewCell cell)
        {
            InitializeComponent();

            this.target = cell;
          ///  EditForm_Load(this, new EventArgs());

        }

        Label tt;
        TextBox load_text;
        private void EditForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Size = new Size(350, 300);
            this.BackColor = Pallete.LightGray;
            this.borderPanel = new Panel();
            this.borderPanel.MouseMove += BorderPanel_MouseMove;
            this.borderPanel.MouseDown += BorderPanel_MouseDown;
            this.borderPanel.Location = Point.Empty;
            this.borderPanel.BackColor = Pallete.Blue;
            //    this.borderPanel.Location = new Point(0, 0);
            this.borderPanel.Size = new Size(this.Size.Width, this.Size.Height / 5);
            this.Controls.Add(this.borderPanel);

            litButton bt = new litButton("X");
            bt.BackColor = Pallete.Blue;
            bt.ForeColor = Pallete.LightGray;
            bt.Location = new Point(this.borderPanel.Width - 50, 10);
            bt.Width = 40;
            bt.Click += (s, c) =>
            {
                DialogResult res = MessageBox.Show("Are you sure you want to quit? \n Some data may be lost.", "Warning", MessageBoxButtons.OKCancel);
                if (res == System.Windows.Forms.DialogResult.OK)
                    this.Close();
            };
            bt.Font = Fonts.Big;

            tt = new Label();
            tt.Text = title;
            tt.Font = Fonts.Regular;
            tt.AutoSize = true;
            tt.TextAlign = ContentAlignment.MiddleCenter;
            tt.Location = new Point(this.Size.Width / 2 - tt.Width / 2, this.borderPanel.Height / 2);
            this.borderPanel.Controls.Add(tt);

            borderPanel.Controls.Add(bt);

            this.infoPanel = new Panel();
            this.infoPanel.Dock = DockStyle.Fill;
            // this.infoPanel.BackColor = Color.Red;
            this.Controls.Add(this.infoPanel);

            ////
            load_text = new TextBox();
            load_text.Text = this.target.Value.ToString();
            this.Controls.Add(load_text);
           

            load_text.Font = Fonts.Regular;

            Size size = TextRenderer.MeasureText(load_text.Text, load_text.Font);
            load_text.Width = size.Width;
            load_text.Height = size.Height;

            load_text.Location = new Point(this.Size.Width / 2 - load_text.Size.Width / 2, this.Size.Height / 2);

            //load_text.Location = new Point(100, 100);
            load_text.BringToFront();

            litButton commitbtn = new litButton("Commit Edit");
            commitbtn.BackColor = Pallete.DarkGray;
            commitbtn.Font = Fonts.Regular;
            commitbtn.Location = Point.Add(load_text.Location, new Size(0, 75));
            this.Controls.Add(commitbtn);
            commitbtn.Click += Commitbtn_Click;
            commitbtn.BringToFront();

        }

        private void Commitbtn_Click(object sender, EventArgs e)
        {
            if (load_text.Text != this.target.Value.ToString())
            {
                if (MessageBox.Show("Are you sure you want to edit?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {

                    object newVal;
                    Type T = this.target.ValueType;
                    try
                    {
                        newVal = Convert.ChangeType(load_text.Text, T);
                    }
                    catch
                    {
                        MessageBox.Show(String.Format("Could not convert given text to type {0}", T.ToString()));
                        return;
                    }

                    OnCommitEdit(this, newVal, new EventArgs());
                    this.Close();


                }
                else
                {
                    return;
                }
            }
            else
            {
                this.Close();
            }
        }

        private Point mouseLoc;
        private void BorderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLoc = new Point(-e.X, -e.Y);
        }

        private void BorderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseLoc.X, mouseLoc.Y);
                Location = mousePos;
            }
        }

        #region

        //        this.target = (T)obj;

        //            _TextBox<int> tInt;
        //        _TextBox<string> tString;
        //        _TextBox<DateTime> tDate;

        //        var arr = typeof(T).GetProperties();
        //        Point pin = new Point(10, 10);
        //        TextBox baseT;
        //            for (int i = 0; i<arr.Length; i++)
        //            {

        //                try
        //                {
        //                    if (arr[i].CanRead)
        //                    {
        //                        tInt = null;
        //                        tString = null;
        //                        tDate = null;

        //                        ///
        //                        var genericTextbox = typeof(_TextBox<>);
        //        Type specificType = genericTextbox.MakeGenericType(arr[i].PropertyType);
        //        var tb = Activator.CreateInstance(specificType); // _TextBox

        //                        if (specificType == typeof(_TextBox<int>)) // Check for type
        //                        {
        //                            tInt = tb as _TextBox<int>;
        //                            tInt.FirstValue = arr[i].GetValue(target);
        //        tInt.Text = arr[i].GetValue(target).ToString();
        //        tInt.Validator += () =>
        //                            {
        //                                int a = int.MinValue;
        //        int.TryParse(tInt.Text, out a);

        //                                return a != int.MinValue;
        //                            };

        //    baseT = tInt as TextBox;

        //                        }
        //                        else if (specificType == typeof(_TextBox<string>))
        //                        {
        //                            tString = tb as _TextBox<string>;
        //                            tString.FirstValue = arr[i].GetValue(target);
        //tString.Text = arr[i].GetValue(target).ToString();
        //tDate.Validator += () => { return true; };

        //                            baseT = tString as TextBox;
        //                        }
        //                        else if (specificType == typeof(_TextBox<DateTime>))
        //                        {
        //                            tDate = tb as _TextBox<DateTime>;
        //                            tDate.Text = (DateTime.Parse(arr[i].GetValue(target).ToString())).ToShortDateString();
        //tDate.FirstValue = arr[i].GetValue(target).ToString();
        //tDate.Validator += () =>
        //                            {
        //                                DateTime a;
        //DateTime.TryParse(tDate.Text, out a);

        //                                return a != null;
        //                            };

        //                            baseT = tDate as TextBox;

        //                        }
        //                        else
        //                            continue;

        //                        Label lbl = new Label();
        //lbl.Text = arr[i].Name;
        //                        lbl.Font = Fonts.Small;
        //                        lbl.Location = pin;
        //                        this.Controls.Add(lbl);

        //baseT.Location = Point.Add(pin, new Size(lbl.Width + 5, 0));
        //                        baseT.Font = Fonts.Small;
        //                        baseT.Enabled = arr[i].CanWrite;
        //                        this.Controls.Add(baseT);

        //pin = Point.Add(pin, new Size(0, lbl.Height + 5));
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e);
        //                    continue;
        //                }
        //                ///
        //            }
        #endregion


    }
}
