using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Winlit
{

    public class litCircularProgressbar : UserControl
    {
        public enum ProgressType
        {
            Percentage,
            RealValue
        }
      
        public delegate void ValueChanged(object sender);
        private event ValueChanged OnValueChanged;

        #region Properties
        private Label lbl;

        private ProgressType progType;

        public ProgressType Type
        {
            get { return progType; }
            set { progType = value; }
        }

        private Color progColor;

        public Color BarColor
        {
            get { return progColor; }
            set { progColor = value; }
        }


        private int value;

        public int Value
        {
            get { return value; }
            set
            {
                if (value <= maxValue)
                {
                    this.value = value;
                    this.OnValueChanged(this);
                }
                else
                {
                    throw new Exception("Given value is larger than max value.");
                }
            }
        }

        private float thickness;

        public float Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

        private int maxValue;

        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        #endregion

        public litCircularProgressbar()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);
            this.maxValue = 100;
            this.thickness = 3f;

            this.OnValueChanged += litCircularProgressbar_OnValueChanged;
            this.Paint += litCircularProgressbar_Paint;
        }


        void litCircularProgressbar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle rect1 = new Rectangle(this.Location, Size.Subtract(this.Size, new Size((int)(this.Size.Width * 0.3), (int)(this.Size.Height * 0.3))));
            Rectangle rect2 = Rectangle.Inflate(rect1, -(int)thickness, -(int)thickness);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPie(new SolidBrush(this.progColor), rect1, 270, 360 - 360 * (this.maxValue - this.value) / 100);
            e.Graphics.FillPie(new SolidBrush(Color.LightGray), rect2, 360, 360);

            if (this.progType == ProgressType.Percentage)
            {
                string st = (100 - 100 * (this.maxValue - this.value) / this.maxValue) + "%";
                SizeF stSize = e.Graphics.MeasureString(st, this.Font);
                e.Graphics.DrawString(st, this.Font, new SolidBrush(this.ForeColor), new PointF(rect2.Location.X + rect2.Size.Width / 2 - stSize.Width / 2, rect2.Location.Y + rect2.Size.Height / 2 - stSize.Height / 2));
            }
            else
            {
                SizeF stSize = e.Graphics.MeasureString(this.value.ToString(), this.Font);
                e.Graphics.DrawString(this.value.ToString(), this.Font, new SolidBrush(this.ForeColor), new PointF(rect2.Location.X + rect2.Size.Width / 2 - stSize.Width / 2, rect2.Location.Y + rect2.Size.Height / 2 - stSize.Height / 2));
            }
        }


        void litCircularProgressbar_OnValueChanged(object sender)
        {
            this.Invalidate();
        }




    }
}
