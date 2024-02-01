using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Media;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace project4
{
    using shape;
    public partial class Form1 : Form
    {
        private SoundPlayer soundPlayer; // Declare SoundPlayer instance

        public Form1()
        {
            InitializeComponent();

            this.Width = 950;
            this.Height = 700;
            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            pic.Image = bm;

        }
        shapeList pts = new shapeList();
        int curIndex = -1;
        
        Bitmap bm;
        Graphics g;
        bool paint = false;
        Point px, py;
        Pen p = new Pen(Color.Black, 1);
        Pen erase = new Pen(Color.White, 10);
        int index;
        int x, y, sX, sY, cX, cY;
        ColorDialog cd = new ColorDialog();
        Color new_color;
        int flag = -1;

        public const string BasePath = @"C:\Users\Aharon Family\source\repos\project4\project4\Resources\";


        private void btn_ellipse_Click(object sender, EventArgs e)
        {
            if (flag == -1)
            {
                soundPlayer = new SoundPlayer(BasePath+ "ellipse.WAV");
                soundPlayer.Play();
            }
            index = 3;
        }

        private void btn_rect_Click(object sender, EventArgs e)
        {
            if (flag == -1)
            {
                soundPlayer = new SoundPlayer(BasePath+"rectangle.WAV");
                soundPlayer.Play();
            }
            index = 4;
        }

        private void btn_line_Click(object sender, EventArgs e)
        {
            if (flag == -1)
            {
                soundPlayer = new SoundPlayer(BasePath+"line.WAV");
                soundPlayer.Play();
            }
            index = 5;
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            pts.DrawAll(g);

            if (paint) //let us see the shape while drawing it 
            {
                if (index == 3)
                {
                    g.DrawEllipse(p, cX, cY, sX, sY);
                }

                if (index == 4)
                {
                    g.DrawRectangle(p, cX, cY, sX, sY);
                }

                if (index == 5)
                {
                    g.DrawLine(p, cX, cY, x, y);
                }
            }
           
        }

            private void btn_clear_Click(object sender, EventArgs e)
            {
            if (flag == -1)
            {
                soundPlayer = new SoundPlayer(BasePath+"clear.WAV");
                soundPlayer.Play();
            }
            g.Clear(Color.White);
                pic.Image = bm;
                index = 0;
                pts.Clear();
                pic.Invalidate();
                curIndex = -1;
            }

        private void btn_fill_Click(object sender, EventArgs e)// color click
            {
            if (flag==-1)
            {
                soundPlayer = new SoundPlayer(BasePath + "color.WAV");
                soundPlayer.Play();
            }
            cd.ShowDialog();
                new_color = cd.Color;
                pic_color.BackColor = cd.Color;
                p.Color = cd.Color;
            }
            static Point set_point(PictureBox pb, Point pt)//  Method to set& return color palette image point
            {
                float pX = 1f * pb.Image.Width / pb.Width;
                float pY = 1f * pb.Image.Height / pb.Height;
                return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
            }

            private void color_picker_MouseClick(object sender, MouseEventArgs e)//if user select any color from color_picker img than
                                                                                 //set that color to new_color, pen_color and pic_color
            {
                Point point = set_point(color_picker, e.Location);
                pic_color.BackColor = ((Bitmap)color_picker.Image).GetPixel(point.X, point.Y);
                new_color = pic_color.BackColor;
                p.Color = pic_color.BackColor;
            }

            private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)//creating method to validate pixel old_color 
                                                                                                             //before filling the shape to the new color
            {
                Color cx = bm.GetPixel(x, y);
                if (cx == old_color)
                {
                    sp.Push(new Point(x, y));
                    bm.SetPixel(x, y, new_color);
                }
            }

            public void Fill(Bitmap bm, int x, int y, Color new_clr)
            {
                Color old_color = bm.GetPixel(x, y);
                Stack<Point> pixel = new Stack<Point>();
                pixel.Push(new Point(x, y));
                bm.SetPixel(x, y, new_clr);
                if (old_color == new_clr) return;

                //this method will get the old pixel color and fill new_color from the clicked point till the stack count>0, if the 
                //old_color is equal to new_color than return.. do nothing 
                while (pixel.Count > 0)
                {
                    Point pt = (Point)pixel.Pop();
                    if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                    {
                        validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                        validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                        validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
                        validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);

                    }
                }

            }

            private void pic_MouseClick(object sender, MouseEventArgs e)
            {
                if (index == 7)
                {
                    Point point = set_point(pic, e.Location);
                    Fill(bm, point.X, point.Y, new_color);
                }

            }

            private void btn_fill_color_Click(object sender, EventArgs e)
            {
            if (flag==-1)
            {
                soundPlayer = new SoundPlayer(BasePath+"fill.WAV");
                soundPlayer.Play();
            }
            index = 7;
            }

            private void radioBtn_circle_CheckedChanged(object sender, EventArgs e)
            {
            if (flag == -1)
            {
                soundPlayer = new SoundPlayer(BasePath+"circle.WAV");
                soundPlayer.Play();
            }
            index = 8;
            }

            private void radioBtn_rectangle_CheckedChanged(object sender, EventArgs e)
            {
            if (flag == -1)
            {
                soundPlayer = new SoundPlayer(BasePath+"rectangle.WAV");
                soundPlayer.Play();
            }
            index = 9;
            }

            private void radioBtn_triangle_CheckedChanged(object sender, EventArgs e)
            {
            if (flag == -1)
            {
                soundPlayer = new SoundPlayer(BasePath+"triangle.WAV");
                soundPlayer.Play();
            }
            index = 10;
            }

            private void btn_load_Click(object sender, EventArgs e)
            {
            if (flag == -1)
            {
                soundPlayer = new SoundPlayer(BasePath+"load.WAV");
                soundPlayer.Play();
            }
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open);
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    DrawingData data = (DrawingData)binaryFormatter.Deserialize(stream);

                    bm = data.Paint;
                    g = Graphics.FromImage(bm); // in order to redraw after loading file
                    pts = data.Objects; // Directly assign the shapeList object

                    pic.Image = bm;
                    pic.Invalidate();
                }
            }
        private void btnMute_Click(object sender, EventArgs e)
        {
            flag *= -1;
            if (flag == 1)
            {
                Bitmap b1 = new Bitmap(BasePath+"mute.PNG");
                btnMute.Image = b1;
            }
            else
            {
                Bitmap b2 = new Bitmap(BasePath+"unmute.PNG");
                btnMute.Image = b2;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                p.Width = comboBox1.SelectedIndex;
                erase.Width = comboBox1.SelectedIndex;  
        }

        private void color_picker_Click(object sender, EventArgs e)
        {

        }

        private void btn_save_Click(object sender, EventArgs e)
            {
            if (flag == -1) {
                soundPlayer = new SoundPlayer(BasePath+ "save.WAV");
                soundPlayer.Play();
            }
                DrawingData data = new DrawingData();
                data.Paint = bm;
                data.Objects = pts;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
                saveFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    IFormatter formatter = new BinaryFormatter();
                    using (Stream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        formatter.Serialize(stream, data);
                    }
                MessageBox.Show("Your project has been saved successfully.", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }

            private void pic_MouseDown(object sender, MouseEventArgs e)
            {
                paint = true;
                py = e.Location;
                cX = e.X;
                cY = e.Y;
                curIndex = -1;

                for (int i = 0; i < pts.NextIndex; i++)
                {
                    if (pts[i].isInside(e.X, e.Y))
                    {
                        curIndex = i;
                        string s = e.Button.ToString();
                        if (s == "Right") //if Right button pressed - Remove
                        {
                            pts.Remove(curIndex);
                            curIndex = -1;
                            pic.Invalidate();
                            return;
                        }
                        break;
                    }
                }
                if (curIndex < 0)
                {
                    if (index == 8)
                    {
                        pts[pts.NextIndex] = new myCircle(e.X, e.Y, 5);
                    }
                    if (index == 9)
                    {
                        pts[pts.NextIndex] = new myRectangle(e.X, e.Y, 20, 10);
                    }
                    if (index == 10)
                    {
                        pts[pts.NextIndex] = new myTriangle(e.X, e.Y, 20, 10);
                    }
                    curIndex = pts.NextIndex - 1;
                    pic.Invalidate();
                }

            }

            private void pic_MouseMove(object sender, MouseEventArgs e)
            {

                if (paint)
                {
                    if (index == 1)
                    {
                        px = e.Location;
                        g.DrawLine(p, px, py);
                        py = px;

                    }
                    if (index == 2)
                    {
                        px = e.Location;
                        g.DrawLine(erase, px, py);
                        py = px;

                    }
                }
                x = e.X;
                y = e.Y;
                sX = e.X - cX;
                sY = e.Y - cY;
                pic.Refresh();

                if (curIndex >= 0 && ((index == 8) || (index == 9) || (index == 10)))
                {

                    shape c = (shape)pts[curIndex];
                    c.X = e.X;
                    c.Y = e.Y;
                    pic.Invalidate();

                }
            }

            private void pic_MouseUp(object sender, MouseEventArgs e)
            {
                curIndex = -1;
                paint = false;

                sX = x - cX;
                sY = y - cY;

                if (index == 3)
                {
                    g.DrawEllipse(p, cX, cY, sX, sY);
                }

                if (index == 4)
                {
                    g.DrawRectangle(p, cX, cY, sX, sY);
                }

                if (index == 5)
                {
                    g.DrawLine(p, cX, cY, x, y);
                }

            }

            private void btn_ereaser_Click(object sender, EventArgs e)
            {
            if (flag == -1)
            {
                soundPlayer = new SoundPlayer(BasePath+"eraser.WAV");
                soundPlayer.Play();
            }
            index = 2;
            }

            private void btn_pencil_Click(object sender, EventArgs e)
            {
            if (flag == -1)
            {
                soundPlayer = new SoundPlayer(BasePath+"pencil.WAV");
                soundPlayer.Play();
            }
            index = 1;
            }
        }
    }

