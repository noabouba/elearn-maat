using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using eLearnBL;
using eLearnDAL;

namespace Winlit
{
    public partial class CourseCreate : Form
    {
        public CourseCreate()
        {
            InitializeComponent();
        }

        private void CourseCreate_Load(object sender, EventArgs e)
        {
            this.AutoSize = true;
            label1.Text = "Teacher:";
            label2.Text = "Subject:";


            radioButton1.Text = "Regular";
            radioButton2.Text = "Pre-Course";
            radioButton3.Text = "Post-Course";

            radioButton1.CheckedChanged += (s, v) => { comboBox3.Visible = false; };
            radioButton2.CheckedChanged += CheckChanged;
            radioButton3.CheckedChanged += CheckChanged;
            comboBox3.Visible = false;
            radioButton1.Select();

            var x = Enum.GetValues(typeof(eLearnDAL.CategoryDAL.Categories));
            foreach (var item in x)
            {
                comboBox1.Items.Add(item.ToString());
            }

            foreach (var item in new Users(Role.Teacher).Collection)
            {
                comboBox2.Items.Add(item.UserID + ": " + item.FirstName + " " + item.LastName);
            }

            foreach (var item in new Courses().Collection)
            {
                comboBox3.Items.Add(item.CourseID + ": " + item.Subject);
            }

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            litButton ok = new litButton("Create");
            ok.Click += (s, xx) => { Create(); };

            ok.Font = Fonts.Regular;
            ok.Location = new Point(this.Width / 2, this.Height - 60);

            this.Controls.Add(ok);
        }

        private bool precourse = false;
        private void CheckChanged(object sender, EventArgs e)
        {
            comboBox3.Visible = true;

            if ((sender as RadioButton).Text.StartsWith("Pre"))
            {
                comboBox3.Location = new Point(comboBox3.Location.X, radioButton2.Location.Y);
                precourse = true;
            }
            else
            {
                comboBox3.Location = new Point(comboBox3.Location.X, radioButton3.Location.Y);
                precourse = false;
            }
        }

        private void Create()
        {
            var x = comboBox2.SelectedItem.ToString().Substring(0, comboBox2.SelectedItem.ToString().IndexOf(":"));
            User teacher = new User(int.Parse(x));
            CategoryDAL.Categories cat = (CategoryDAL.Categories)(comboBox1.SelectedIndex + 1);

            Course cr = new Course(teacher, textBox2.Text, cat);
            if (comboBox3.Visible)
            {
                if (comboBox3.SelectedItem != null)
                {
                    int cr2 = int.Parse(comboBox3.SelectedItem.ToString().Substring(0, comboBox3.SelectedItem.ToString().IndexOf(":")));
                    Course cr2Obj = new Courses().Collection.Find(a => a.CourseID == cr2);
                    if (precourse)
                    {
                        cr2Obj.AddPreCourse(cr);
                    }
                    else
                        cr2Obj.AddPostCourse(cr);
                }
                else
                {
                    MessageBox.Show("You need to select a post/pre course.");
                    return;
                }
            }
            MessageBox.Show("Course Created");
        }
    }
}
