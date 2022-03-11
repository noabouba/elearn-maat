using System;
using System.Drawing;
using System.Windows.Forms;
using eLearnBL;
using Transitions;
namespace Winlit
{
    public partial class EditCourse : Form
    {
        private Course course;
        private Panel creationPanel;
        private Panel selected = null;
        private Panel lessonPanel;
        public EditCourse(Course crs)
        {

            InitializeComponent();
            this.course = crs;
            //            this.AutoSize = true;
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private litButton saveBtn;
        private Label subject;
        private TextBox pathTxt;
        private TextBox titleTxt;
        private litButton delBtn;
        private int count = 0;
        private void EditCourse_Load(object sender, EventArgs e)
        {
            Size size = new Size(450, 150);
            this.Size = new Size(720, 300);

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            lessonPanel = new Panel();
            lessonPanel.Dock = DockStyle.Left;
            lessonPanel.AutoScroll = true;
            lessonPanel.MouseEnter += (s, h) => { lessonPanel.Focus(); };
            lessonPanel.Size = new Size(size.Width, this.Height);
            this.Controls.Add(lessonPanel);

            CreationPanel();

            RedrawLessons();

            subject = new Label();
            subject.Font = Fonts.Regular;
            subject.TextAlign = ContentAlignment.TopCenter;
            subject.Size = new Size(this.Width - lessonPanel.Width - 20, 75);
            subject.Location = new Point(lessonPanel.Width, 25);
            this.Controls.Add(subject);

            saveBtn = new litButton("Save Edit");
            saveBtn.Location = new Point(this.lessonPanel.Width, this.Height - 120);
            saveBtn.Font = Fonts.Regular;
            saveBtn.Size = new Size(this.Width - lessonPanel.Width, 40);
            saveBtn.BackColor = Pallete.DarkGray;
            saveBtn.Click += (s, h) =>
            {
                Lesson ls = (selected.Tag as Lesson);
                if (pathTxt.Text != "" && titleTxt.Text != "")
                {
                    if (pathTxt.Text != ls.VideoPath)
                        ls.VideoPath = pathTxt.Text;

                    if (titleTxt.Text != ls.Title)
                        ls.Title = titleTxt.Text;

                    selected = null;

                    RefreshButtons();
                    RedrawLessons();
                }
                else
                    MessageBox.Show("All fields must be full to save edit.");


            };
            this.Controls.Add(saveBtn);

            delBtn = new litButton("Delete Lesson");
            delBtn.Click += (s, h) =>
            {
                this.selected.BackColor = Color.Red;
                this.course.RemoveLesson((this.selected.Tag as Lesson));
            };

            delBtn.Size = new Size((this.Width - lessonPanel.Width) / 2 - 10, 30);
            delBtn.Location = new Point(this.lessonPanel.Width, this.Height - 75);
            delBtn.Font = Fonts.Small;
            delBtn.BackColor = Pallete.DarkGray;
            delBtn.AutoSize = false;
            this.Controls.Add(delBtn);

            litButton addBtn = new litButton("Create Lesson");
            addBtn.Click += (s, h) =>
            {
                ToggleCreationPanel();
            };

            addBtn.Size = new Size((this.Width - lessonPanel.Width) / 2 - 10, 30);
            addBtn.Location = new Point(this.lessonPanel.Width + delBtn.Width + 5, this.Height - 75);
            addBtn.Font = Fonts.Small;
            addBtn.AutoSize = false;
            addBtn.BackColor = Pallete.DarkGray;
            this.Controls.Add(addBtn);

            Label lbl1 = new Label();
            lbl1.Text = "Title:";
            lbl1.Font = Fonts.Regular;
            lbl1.AutoSize = true;
            lbl1.Location = new Point(subject.Location.X, subject.Top + subject.Height);
            Controls.Add(lbl1);

            titleTxt = new TextBox();
            titleTxt.Size = new Size(160, 40);
            titleTxt.Location = Point.Add(lbl1.Location, new Size(lbl1.Size.Width + 5, 0));
            Controls.Add(titleTxt);

            Label lbl2 = new Label();
            lbl2.Font = Fonts.Regular;
            lbl2.AutoSize = true;
            lbl2.Text = "Path:";
            lbl2.Location = new Point(subject.Location.X, subject.Top + subject.Height + lbl1.Height + 20);
            Controls.Add(lbl2);

            pathTxt = new TextBox();
            pathTxt.Size = new Size(160, 40); 
            pathTxt.Location = Point.Add(titleTxt.Location, new Size(0, lbl1.Height + 20));
            Controls.Add(pathTxt);

            litButton refreshBtn = new litButton("Refresh");
            refreshBtn.Font = Fonts.Small;
            refreshBtn.BackColor = Pallete.DarkGray;
            refreshBtn.AutoSize = true;
            refreshBtn.Click += (s, h) => { RedrawLessons(); };
            refreshBtn.Location = new Point(lessonPanel.Width, 0);
            this.Controls.Add(refreshBtn);


            RefreshButtons();

        }

        private void DrawLesson(Lesson ls, bool newLesson)
        {
            Size size = new Size(450, 150);


            Panel pnl = new Panel();
            pnl.BackColor = newLesson ? Color.Green : Pallete.CalmBlue;
            //pnl.Location = new Point(0, count * 155);
            pnl.Top = count * 155;
            pnl.Size = size;
            //            pnl.Size = new Size(this.Width,150);
            pnl.Tag = ls;
            pnl.Click += (s, a) => { if (this.selected != pnl) { this.selected = pnl; } else { this.selected = null; } RefreshButtons(); };

            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Font = Fonts.Big;
            lbl.Text = ls.Title;
            lbl.Location = new Point(15, 15);
            pnl.Controls.Add(lbl);

            Label lbl2 = new Label();
            lbl2.AutoSize = true;
            lbl2.Font = Fonts.Small;
            lbl2.Text = "Uploaded at: " + ls.UploadDate.ToShortDateString();
            lbl2.Tag = ls.UploadDate;
            lbl2.Location = new Point(15, 45);
            pnl.Controls.Add(lbl2);

            Label lbl3 = new Label();
            lbl3.AutoSize = true;
            lbl3.Font = Fonts.Small;
            lbl3.Text = "Course: " + this.course.Subject;
            lbl3.Location = new Point(15, 60);
            pnl.Controls.Add(lbl3);

            Label lbl4 = new Label();
            lbl4.AutoSize = true;
            lbl4.Font = Fonts.Small;
            lbl4.Text = "Teacher: " + this.course.Teacher.FirstName + " " + this.course.Teacher.LastName;
            lbl4.Location = new Point(15, 80);
            pnl.Controls.Add(lbl4);

            Label lbl5 = new Label();
            lbl5.AutoSize = true;
            lbl5.Font = Fonts.Small;
            lbl5.Text = "Path: " + ls.VideoPath;
            lbl5.Location = new Point(15, 100);
            pnl.Controls.Add(lbl5);
            count++;
            this.lessonPanel.Controls.Add(pnl);
        }

        private void RefreshButtons()
        {
            this.delBtn.Enabled = selected != null;
            this.saveBtn.Enabled = selected != null;
            pathTxt.Enabled = saveBtn.Enabled;
            titleTxt.Enabled = saveBtn.Enabled;

            if (this.saveBtn.Enabled)
                subject.Text = (this.selected.Tag as Lesson).Title;
            else
                subject.Text = "-";

            if (this.saveBtn.Enabled)
            {
                pathTxt.Text = (selected.Tag as Lesson).VideoPath;
                titleTxt.Text = (selected.Tag as Lesson).Title;
            }
            else
            {
                pathTxt.Text = "";
                titleTxt.Text = "";
                
            }
        }

        private void CreationPanel()
        {
            this.creationPanel = new Panel();

            creationPanel.Location = new Point(this.Width, 0);
            creationPanel.Size = new Size(this.Width - this.lessonPanel.Width, this.Height);
            creationPanel.BackColor = Pallete.SlateDarkGray;

            Label lbl1 = new Label();
            lbl1.Font = Fonts.Big;
            lbl1.Text = "Title";
            lbl1.AutoSize = true;
            lbl1.Location = new Point(10, 30);

            TextBox txt1 = new TextBox();
            txt1.Location = Point.Add(lbl1.Location, new Size(lbl1.Size.Width, 0));
            txt1.Size = new Size(creationPanel.Size.Width / 2, 50);
            txt1.Font = Fonts.Regular;


            Label lbl2 = new Label();
            lbl2.Font = Fonts.Big;
            lbl2.Text = "Path ";
            lbl2.Location = new Point(10, 70);
            TextBox txt2 = new TextBox();
            txt2.Location = Point.Add(lbl2.Location, new Size(lbl2.Size.Width, 0));
            txt2.Size = new Size(creationPanel.Size.Width / 2, 50);
            txt2.Font = Fonts.Regular;

            litButton cancleBtn = new litButton("Cancel");
            cancleBtn.Size = new Size(creationPanel.Width / 2 - 10, 50);
            cancleBtn.Font = Fonts.Small;
            cancleBtn.BackColor = Pallete.DarkGray;
            cancleBtn.Location = new Point(5, this.Height - 95);
            cancleBtn.Click += (s, h) =>
              {
                  txt1.Text = "";
                  txt2.Text = "";
                  ToggleCreationPanel();
              };

            litButton createBtn = new litButton("Create Lesson");
            createBtn.Size = new Size(creationPanel.Width / 2 - 10, 50);
            createBtn.Font = Fonts.Small;
            createBtn.BackColor = Pallete.DarkGray;
            createBtn.Location = new Point(cancleBtn.Width + 5, this.Height - 95);
            createBtn.Click += (s, h) =>
            {
                if (txt1.Text != "" && txt2.Text != "")
                {
                    Lesson ls = new Lesson(DateTime.Now.Date, txt2.Text, course.CourseID, txt1.Text);
                    DrawLesson(ls, true);
                    txt1.Text = "";
                    txt2.Text = "";
                    //count = 0;
                    this.course.AddLesson(ls);
                    Console.WriteLine(this.course.Lessons.Length);
                    ToggleCreationPanel();
                    RedrawLessons();
                }
                else
                    MessageBox.Show("All fields must be full to create a lesson.");
            };

            this.creationPanel.Controls.Add(createBtn);
            this.creationPanel.Controls.Add(cancleBtn);

            this.creationPanel.Controls.Add(lbl1);
            this.creationPanel.Controls.Add(txt1);

            this.creationPanel.Controls.Add(lbl2);
            this.creationPanel.Controls.Add(txt2);
            creationPanel.Enabled = false;
            this.Controls.Add(creationPanel);
        }

        private void ToggleCreationPanel()
        {
            Transition.run(this.creationPanel, "Left", this.creationPanel.Enabled ? this.Width : lessonPanel.Width, new TransitionType_Acceleration(200));
            this.creationPanel.Enabled = !this.creationPanel.Enabled;
        }

        private void RedrawLessons()
        {
            count = 0;
            lessonPanel.Controls.Clear();
            for (int i = 0; i < this.course.Lessons.Length; i++)
            {
                DrawLesson(course.Lessons[i], false);

            }
        }

    }
}
