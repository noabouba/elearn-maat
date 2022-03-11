using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using eLearnBL;
using Transitions;
using System.Linq;

namespace Winlit
{

    public partial class MainForm : Form
    {
        private Size clientSize;
        private Panel borderPanel;
        private Panel sidePanel;
        // private Dictionary<string, litMenuItem> panels;
        private litMenu menu;
        private Region regionConst;
        private string[] itemTitles = { "Dashboard", "Teachers", "Students", "Courses", "Lessons", "Suggestions" };
        public MainForm()
        {
            InitializeComponent();
            clientSize = new Size(935, 545);
            //   Cursor.Current = Cursors.Hand;


            this.MaximumSize = this.clientSize;
            this.MinimumSize = this.clientSize;
            this.BackColor = Pallete.LightGray;
            this.FormBorderStyle = FormBorderStyle.None;

            regionConst = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            Region = regionConst;
            sidePanel = new Panel();
            sidePanel.Dock = DockStyle.Left;
            sidePanel.BackColor = Pallete.DarkerGray;
            sidePanel.ForeColor = Pallete.DarkGray;
            sidePanel.Font = new Font(Fonts.Regular, FontStyle.Bold);
            sidePanel.Size = new Size(clientSize.Width / 4, clientSize.Height);
            Controls.Add(sidePanel);

            this.borderPanel = new Panel();
            this.borderPanel.MouseMove += BorderPanel_MouseMove;
            this.borderPanel.MouseDown += BorderPanel_MouseDown;
            this.borderPanel.Location = Point.Empty;
            this.borderPanel.BackColor = Pallete.Blue;
            this.borderPanel.Location = new Point(sidePanel.Width, 0);
            this.borderPanel.Size = new Size(clientSize.Width - sidePanel.Width, clientSize.Height / 5);
            this.Controls.Add(this.borderPanel);

            litButton bt = new litButton("X");
            bt.BackColor = Pallete.Blue;
            bt.ForeColor = Pallete.LightGray;
            bt.Location = new Point(this.borderPanel.Width - 50, 10);
            bt.Width = 40;
            bt.Font = Fonts.Big;

            bt.Click += (s, e) =>
            {
                this.Close();
            };

            bt.BringToFront();
            this.borderPanel.Controls.Add(bt);

            menu = new litMenu(sidePanel);

            int btH = 0;
            Random rnd = new Random();
            for (int i = 0; i < itemTitles.Length; i++)
            {
                litMenuItem m = new litMenuItem(itemTitles[i]);
                m.Button.Width = sidePanel.Width;
                m.Button.Font = new Font(Fonts.Big, FontStyle.Regular);
                m.Button.Location = new Point(0, 5 + btH * i);

                m.ViewPanel = new Panel();
                m.ViewPanel.Location = new Point(menu.ViewPanel.Width, borderPanel.Height);
                m.ViewPanel.Size = new Size(clientSize.Width - sidePanel.Width, clientSize.Height - borderPanel.Height);
                //  m.ViewPanel.Hide();
                int aa = rnd.Next(120, 180);
                m.ViewPanel.BackColor = Color.FromArgb(aa, aa, aa);


                m.Button.Click += (s, e) =>
                {
                    if (litMenu.current != (s as litButton).Text)
                    {
                        m.ViewPanel.Top = borderPanel.Height + m.ViewPanel.Height;
                        m.ViewPanel.Show();
                        m.ViewPanel.BringToFront();
                        Transition.run(m.ViewPanel, "Top", borderPanel.Height, new TransitionType_EaseInEaseOut(250)); // Trasnit the panel upwards
                        //m.ViewPanel.Height = clientSize.Height - borderPanel.Height;
                        this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                        litMenu.current = (s as litButton).Text;

                    }
                };

                menu.AddItem(m);

                this.Controls.Add(m.ViewPanel);

                if (btH == 0)
                    btH = m.Button.Height;


            }
            menu.GetItem(itemTitles[0]).Button.PerformClick();
            LockScreen();
            bt.BringToFront();
            //bt.BringToFront();
        }

        private void LockScreen()
        {
            Panel pnl = new Panel();
            pnl.Size = this.Size;

            Label lb = new Label();
            lb.Text = "eLearn";
            lb.Font = new Font("Arial", 46);
            lb.ForeColor = Color.Black;

            this.Controls.Add(pnl);
            pnl.BackColor = Color.LightGray;
            pnl.BringToFront();
            lb.AutoSize = true;

            litButton lbt = new litButton("X");
            lbt.Location = new Point(this.Width - 70, 15);
            lbt.Click += (s, e) =>
            {
                this.Close();
            };

            lbt.Font = Fonts.Big;
            pnl.Controls.Add(lbt);

            lbt.BringToFront();
            TextBox tb1 = new TextBox();
            TextBox tb2 = new TextBox();
            tb1.Width = 350;
            tb2.Width = 350;
            tb1.Font = Fonts.Regular;
            tb2.Font = Fonts.Regular;
            tb1.Location = new Point(this.Width / 2 - 175, this.Height / 2);
            tb2.Location = Point.Add(tb1.Location, new Size(0, 50));
            tb2.PasswordChar = '*';

            lb.Location = Point.Add(tb1.Location, new Size(35, -160));

            pnl.Controls.Add(lb);

            pnl.Controls.Add(tb1);
            pnl.Controls.Add(tb2);
            pnl.Move += Pnl_Move;

            litButton logbtn = new litButton("Login");
            logbtn.Location = Point.Add(tb2.Location, new Size(0, 50));
            logbtn.Font = Fonts.Big;
            logbtn.Click += (s, e) =>
              {
                  User us = PasswordUtils.IsUser(tb1.Text, tb2.Text);
                  if (us != null && us.Role == Role.Admin)
                  {
                      Transition.run(pnl, "Top", -this.Height, new TransitionType_Acceleration(700));
                  }
                  else
                  {
                      MessageBox.Show("Cannot login with given username and password.");
                  }
              };

            pnl.Controls.Add(logbtn);
        }

        private void Pnl_Move(object sender, EventArgs e)
        {
            if ((sender as Panel).Top < 10 - this.Height)
            {
                (sender as Panel).Hide();
                this.Controls.Remove((sender as Panel));
                Invalidate();
            }
        }

        public enum Permissions
        {
            Full,
            None,
            Readonly
        }

        private void InitializePanels()
        {
            // TOTO: Do dashboard, lesosns show comments? maybe.
            litMenuItem item = menu.GetItem(itemTitles[0]);
            Panel pnl = item.ViewPanel;


            chart.Series.Add("Topics");
            chart.Palette = ChartColorPalette.Grayscale;
            chart.Titles.Add("Most Wanted Topics");
            Courses crs = new Courses();
            chart.Series["Topics"].SetDefault(true);
            chart.Series["Topics"].Enabled = true;

            string[] arr = Enum.GetNames(typeof(eLearnDAL.CategoryDAL.Categories));
            foreach (var cat in arr)
            {
                int num = crs.Collection.Count(a => a.Category.ToString() == cat);
                chart.Series["Topics"].Points.AddXY(cat, num);
            }

            pnl.Controls.Add(chart);

            litCircularProgressbar cbar = new litCircularProgressbar();
            cbar.Type = litCircularProgressbar.ProgressType.RealValue;
            cbar.Value = new Users().Collection.Count;
            cbar.BarColor = Pallete.Blue;
            cbar.Thickness = 5f;
            cbar.Font = Fonts.Big;
            cbar.Size = new Size(220, 220);
            cbar.Location = Point.Add(chart.Location, new Size(0, 85));
            pnl.Controls.Add(cbar);
            Label lbl = new Label();
            lbl.Font = Fonts.Big;
            lbl.Text = "Users";
            lbl.Location = Point.Add(cbar.Location, new Size(50, 220));
            pnl.Controls.Add(lbl);
            lbl.BringToFront();
            // Teachers
            item = menu.GetItem(itemTitles[1]);
            pnl = item.ViewPanel;

            Size gridSize = new Size(5 * pnl.Width / 8, 7 * pnl.Height / 8);
            Size editSize = new Size((pnl.Width - gridSize.Width) * 5 / 6, gridSize.Height / 3);

            litGridView dgv = new litGridView();

            #region Teacher Refresher
            dgv.RefreshMethod = () =>
            {
                List<User> lst = new Users(Role.Teacher).Collection;
                DataGridViewColumn dc = new DataGridViewColumn(); // For object
                dc.Visible = false;
                // dc.ValueType = typeof(object);
                dc.Name = "obj";

                var newlist = lst.Select(x => new
                {
                    obj = x,
                    UserID = x.UserID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    BirthDate = x.BirthDate,
                    Email = x.Email,
                    Password = x.Password
                }
                ).ToList();

                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                        col.Tag = Permissions.Full;
                    else
                        col.Tag = Permissions.None;

                }

                dgv.DataSource = newlist;
                dgv.Columns["obj"].Visible = false;
                dgv.Columns["UserID"].Tag = Permissions.Readonly;
                dgv.Columns["Password"].Tag = Permissions.Readonly;

                return 0;
            };
            #endregion

            dgv.Size = gridSize;
            dgv.AutoSize = false;
            dgv.Size = gridSize;
            dgv.Location = new Point(10, 10);
            dgv.DataSource = new Users(Role.Teacher).Collection;
            dgv.ReadOnly = true;
            dgv.CellBeginEdit += Dgv3_CellBeginEdit;

            pnl.Controls.Add(dgv);
            litEditPanel<User> ep1 = new litEditPanel<User>();
            ep1.Size = editSize;
            ep1.Target = dgv;
            ep1.Location = Point.Add(dgv.Location, new Size(dgv.Width + 10, 0));
            ep1.BackColor = Pallete.SlateDarkGray;
            pnl.Controls.Add(ep1);
            //

            // Students
            item = menu.GetItem(itemTitles[2]);
            pnl = item.ViewPanel;
            litGridView dgv2 = new litGridView();
            #region Students Refresher
            dgv2.RefreshMethod = () =>
                 {
                     List<User> lst = new Users(Role.Student).Collection;
                     DataGridViewColumn dc = new DataGridViewColumn(); // For object
                     dc.Visible = false;
                     // dc.ValueType = typeof(object);
                     dc.Name = "obj";

                     var newlist = lst.Select(x => new
                     {
                         obj = x,
                         UserID = x.UserID,
                         FirstName = x.FirstName,
                         LastName = x.LastName,
                         BirthDate = x.BirthDate,
                         Email = x.Email,
                         Password = x.Password
                     }
                     ).ToList();
                     dgv2.DataSource = newlist;


                     dgv2.Columns["FirstName"].Visible = true;
                     foreach (DataGridViewColumn col in dgv2.Columns)
                     {
                         if (col.Visible)
                             col.Tag = Permissions.Full;
                         else
                             col.Tag = Permissions.None;

                     }



                     dgv2.Columns["obj"].Visible = false;
                     dgv2.Columns["UserID"].Tag = Permissions.Readonly;
                     dgv2.Columns["Password"].Tag = Permissions.Readonly;

                     return 0;
                 };
            #endregion

            dgv2.AutoSize = false;
            dgv2.Size = gridSize;
            dgv2.Location = new Point(10, 10);
            dgv2.CellBeginEdit += Dgv3_CellBeginEdit;

            dgv2.ReadOnly = true;
            pnl.Controls.Add(dgv2);
            dgv2.RefreshMethod();

            litEditPanel<User> ep2 = new litEditPanel<User>();
            ep2.Size = editSize;
            ep2.Location = Point.Add(dgv2.Location, new Size(dgv2.Width + 10, 0));
            ep2.Target = dgv2;
            ep2.BackColor = Pallete.SlateDarkGray;
            pnl.Controls.Add(ep2);
            //

            // Courses

            item = menu.GetItem(itemTitles[3]);
            pnl = item.ViewPanel;

            litGridView dgv3 = new litGridView();

            dgv3.AutoSize = false;
            dgv3.Size = gridSize;
            dgv3.Location = new Point(10, 10);

            #region Course Refresher
            dgv3.RefreshMethod = () =>
                {
                    List<Course> lst = new Courses().Collection;
                    //dgv3.Columns[0].Visible = false;
                    /// Add teacher name!!!!


                    var c = lst.Select(a => new
                    {
                        obj = a,
                        //  TeacherID = a.Teacher.UserID,
                        TeacherFName = a.Teacher.FirstName,
                        TeacherLName = a.Teacher.LastName,

                        // CourseID = a.CourseID,
                        Subject = a.Subject,
                        LessonCount = a.Lessons.Length
                    }).ToList();



                    dgv3.DataSource = c;


                    dgv3.Columns[0].Visible = false;
                    //dgv3.Columns[1].Visible = false;
                    //dgv3.Columns[4].Visible = false;

                    foreach (DataGridViewColumn col in dgv3.Columns)
                    {
                        if (col.Visible)
                            col.Tag = Permissions.Full;
                        else
                            col.Tag = Permissions.None;

                    }


                    dgv3.Columns["LessonCount"].Tag = Permissions.Readonly;


                    return 0;
                };

            #endregion

            pnl.Controls.Add(dgv3);

            dgv3.RefreshMethod(); // Call refresher

            dgv3.ReadOnly = true;
            dgv3.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv3.CellBeginEdit += Dgv3_CellBeginEdit;


            litEditPanel<Course> ep = new litEditPanel<Course>();
            ep.Size = editSize;
            ep.Location = Point.Add(dgv3.Location, new Size(dgv3.Width + 10, 0));
            ep.Target = dgv3;
            ep.BackColor = Pallete.SlateDarkGray;

            pnl.Controls.Add(ep);

            litButton add = new litButton("New Course");
            add.Location = new Point(5, 85);
            add.Font = Fonts.Regular;
            add.Click += (s, e) =>
                {
                    new CourseCreate().Show();
                };

            ep.Controls.Add(add);

            //
            item = menu.GetItem(itemTitles[4]);
            pnl = item.ViewPanel;

            ComboBox cb = new ComboBox();
            cb.Location = new Point(10, 10);
            var xx = new Courses().Collection;
            foreach (Course c in xx)
                cb.Items.Add(new CoursePair(c));

            //cb.SelectedIndex = 0;
            cb.Font = Fonts.Regular;
            cb.Size = new Size(250, 80);
            pnl.Controls.Add(cb);

            litButton editBtn = new litButton("Edit Lessons");
            editBtn.AutoSize = true;
            editBtn.Font = Fonts.Regular;
            editBtn.BackColor = Pallete.AlternateBlue;
            editBtn.Click += (s, e) =>
                {
                    EditCourse ec = new EditCourse(((CoursePair)cb.SelectedItem).CourseObj);
                    ec.Show();
                };

            editBtn.Location = new Point(300, 10);
            pnl.Controls.Add(editBtn);


            item = menu.GetItem(itemTitles[5]);
            pnl = item.ViewPanel;
            pnl.AutoScroll = true;
            CreateSuggestionPanels(pnl,CourseSuggestion.GetAllSuggestions());
        }

        public void CreateSuggestionPanels(Panel pn, List<CourseSuggestion> ls)
        {
            int h = 250;
            int w = 3 * (this.Size.Width - sidePanel.Width) / 4;

            ls = ls.OrderBy(a => a.IsApproved ? 1 : 0).ToList();

            //List<Panel> ret = new List<Panel>();
            for (int i = 0; i < ls.Count; i++)
            {
                Panel pnl = new Panel();
                pnl.Left = 15;
                pnl.Top = i * (int)(1.1*h) + 10;
                pnl.Size = new Size(w, h);
                pnl.BackColor = ls[i].IsApproved ? Color.LightGreen : Pallete.LightBlue;
                pnl.Tag = ls[i];

                Label lbl = new Label();
                lbl.Font = Fonts.Big;
                lbl.Text = ls[i].Subject;
                lbl.Location = new Point(10, 15);
                lbl.AutoSize = true;
                pnl.Controls.Add(lbl);

                lbl = new Label();
                lbl.Font = Fonts.Regular;
                lbl.Text = "Teacher: " + new User(ls[i].TeacherId).FullName;
                lbl.Location = new Point(10, 40);
                lbl.AutoSize = true;
                pnl.Controls.Add(lbl);

                lbl = new Label();
                lbl.Font = Fonts.Regular;
                lbl.Text = "Date suggested: " + ls[i].SuggestionDate.ToShortDateString();
                lbl.Location = new Point(10, 75);
                lbl.AutoSize = true;
                pnl.Controls.Add(lbl);

                Panel p2 = new Panel();
                p2.Location = new Point(10, 100);
                p2.Size = new Size(2 * w / 3 - 20, h * 4 / 10);
                p2.BackColor = Color.White;
                pnl.Controls.Add(p2);
                int i2 = 0;
                p2.AutoScroll = true;
                foreach (var item in ls[i].Videos)
                {
                    Label l2 = new Label();
                    l2.AutoSize = true;
                    l2.Font = Fonts.Small;
                    l2.Location = new Point(5, i2 * 30);
                    l2.Text = item.Title + " : ";
                    LinkLabel ll2 = new LinkLabel();
                    ll2.AutoSize = true;
                    ll2.Text = item.Video;
                    LinkLabel.Link lnk = new LinkLabel.Link();
                    lnk.LinkData = item.Video;
                    ll2.Location = new Point(100, i2 * 30);
                    ll2.Font = Fonts.Small;
                    ll2.LinkClicked += (s, e) =>
                    {
                        System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
                    };
                    ll2.Links.Add(lnk);

                    p2.Controls.Add(l2);
                    p2.Controls.Add(ll2);
                    i2++;
                }
                pnl.Controls.Add(p2);

                if (!ls[i].IsApproved)
                {
                    litButton bt = new litButton("Approve");
                    bt.Click += (s, e) =>
                    {
                        if (MessageBox.Show("Are you sure you want to approve this course?", "!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            (bt.Parent.Tag as CourseSuggestion).Approve();
                            bt.Enabled = false;
                            pnl.BackColor = Color.LightGreen;
                        }
                    };
                    bt.Location = new Point(10, h - 35);
                    bt.Font = Fonts.Regular;
                    pnl.Controls.Add(bt);
                }

                RichTextBox tb = new RichTextBox();
                tb.Font = Fonts.Small;
                tb.Width = 1 * w / 3 - 5;
                tb.Height = 3 * h / 4;
                tb.Location = new Point(2 * w / 3, 10);
                tb.Text = ls[i].Description;
                tb.ReadOnly = true;
                pnl.Controls.Add(tb);

                pn.Controls.Add(pnl);
            }

        }

        private struct CoursePair
        {
            private Course crs;

            public Course CourseObj
            {
                get { return crs; }
                set { crs = value; }
            }

            public CoursePair(Course c)
            {
                this.crs = c;
            }

            public override string ToString()
            {
                return this.crs.Subject;
            }

        }

        private void Dgv3_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell cell = (sender as DataGridView).CurrentCell;

            if ((Permissions)cell.OwningColumn.Tag == Permissions.Full)
            {
                EditForm form = new EditForm(cell);
                form.OnCommitEdit += Form_OnCommitEdit;
                form.Show();
            }
            else
            {
                e.Cancel = true;
            }


        }

        private void Form_OnCommitEdit(object sender, object newValue, EventArgs e)
        {

            EditForm f = sender as EditForm;
            DataGridViewCell cell = f.TargetObject;

            Console.WriteLine(newValue);
            //cell.Value = newValue;
            try
            {
                litGridView ldg = cell.DataGridView as litGridView;

                object obj = cell.OwningRow.Cells[0].Value;
                obj.GetType().GetProperty(cell.OwningColumn.Name).SetValue(obj, newValue);
                ldg.RefreshMethod();

                // RefreshDataGrid(cell.DataGridView);
                //  cell.DataGridView.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void RefreshDataGrid(DataGridView dgv)
        {
            var src = dgv.DataSource;
            dgv.DataSource = null;

            dgv.DataSource = src;
        }




        // For dragging
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
                mousePos.Offset(mouseLoc.X - menu.ViewPanel.Width, mouseLoc.Y);
                Location = mousePos;
            }
        }


        #region Beautify Form
        // Drop shadow
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }



        // Rounded edges
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializePanels();

        }
    }
}
