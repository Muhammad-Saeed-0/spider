using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace spider_v2
{
    public partial class Form1 : Form
    {
        Rectangle bordRectangle;

        Point currentPoint = new Point();
        Point oldPoint = new Point();


        //array of binary data representing the values of pixels in an image or display(including the colour of each of its pixels)
        Bitmap surface;

        //for painting on surfaces
        Graphics g;


        ColorDialog cd = new ColorDialog();
        bool paint = false;
        int index = 0, x, y, cX, cY;

        Pen pen0 = new Pen(Color.White, 1);
        Pen pen = new Pen(Color.White, 5);
        Pen pen2 = new Pen(Color.Black, 5);

        SolidBrush solidBrush = new SolidBrush(Color.White);

        string path0 = "";

        public Form1()
        {
            InitializeComponent();

            surface = new Bitmap(canvasPanel.Width, canvasPanel.Height);
            g = Graphics.FromImage(surface);
            canvasPanel.Image = surface;

            ChangeVisible();
        }

        private void ChangeVisible()
        {
            textBox1.Visible = false;
            print.Visible = false;
            saveText.Visible = false;
            path.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            fontBox.Visible = false;
            sizeBox.Visible = false;
            styleBox.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        
        private void canvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            oldPoint = e.Location;

            cX = e.X;
            cY = e.Y;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                currentPoint = e.Location;

                if (index == 0) // penPaint
                {
                    g.DrawLine(pen0, oldPoint, currentPoint);
                    oldPoint = currentPoint;
                }
                else if (index == 1) // brush
                {
                    g.DrawLine(pen, oldPoint, currentPoint);     
                    oldPoint = currentPoint;
                }
                else if (index == 2) // eraser
                {         
                    g.DrawLine(pen2, oldPoint, currentPoint);
                    oldPoint = currentPoint;
                }
            }

            canvasPanel.Refresh();
            x = e.X;
            y = e.Y;
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            currentPoint = e.Location;      

            if (index == 3) // ellipse
            {
                pen.DashStyle = DashStyle.Solid;
                g.DrawEllipse(pen, GetRectangle());
            }

            if (index == 4) // rectangle
            {
                pen.DashStyle = DashStyle.Solid;
                g.DrawRectangle(pen, GetRectangle());
            }

            if (index == 5) //line
            {
                pen.DashStyle = DashStyle.Solid;
                g.DrawLine(pen, cX, cY, x, y);
            }

            if (index == 7)
            {
                pen.DashStyle = DashStyle.Solid;
                g.FillEllipse(solidBrush, GetRectangle());
            }

            if (index == 8)
            {
                pen.DashStyle = DashStyle.Solid;
                g.FillRectangle(solidBrush, GetRectangle());
            }

            if (index == 9)
            {
                pen.DashStyle = DashStyle.Custom;
                pen.DashPattern = new float[] { 10, 2 };
                g.DrawLine(pen, cX, cY, x, y);
            }

            if (index == 10)
            {
                pen.DashStyle = DashStyle.Solid;
                bordRectangle = GetRectangle();
                Point point1 = new Point() { X = bordRectangle.X, Y = bordRectangle.Y + bordRectangle.Height };
                Point point2 = new Point() { X = bordRectangle.X + (bordRectangle.Width / 2), Y = bordRectangle.Y };
                Point point3 = new Point() { X = bordRectangle.X + bordRectangle.Width, Y = bordRectangle.Y + bordRectangle.Height };
                g.DrawPolygon(pen, new Point[] { point1, point2, point3 });
            }

            if (index == 11)
            {
                pen.DashStyle = DashStyle.Solid;

                bordRectangle = GetRectangle();
                Point point1 = new Point() { X = bordRectangle.X, Y = bordRectangle.Y + bordRectangle.Height };
                Point point2 = new Point() { X = bordRectangle.X + (bordRectangle.Width / 2), Y = bordRectangle.Y };
                Point point3 = new Point() { X = bordRectangle.X + bordRectangle.Width, Y = bordRectangle.Y + bordRectangle.Height };

                g.FillPolygon(solidBrush, new Point[] { point1, point2, point3 });
            }

            if (index == 13)
            {
                pen.DashStyle = DashStyle.Solid;
                pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                g.DrawLine(pen, cX, cY, x, y);
            }

            if (index == 14)
            {
                pen.DashStyle = DashStyle.Solid;
                bordRectangle = GetRectangle();
                g.DrawPolygon(pen, StarPoints(bordRectangle));
            }

            if (index == 15)
            {
                pen.DashStyle = DashStyle.Solid;
                bordRectangle = GetRectangle();
                g.FillPolygon(solidBrush, StarPoints(bordRectangle));
            }
        }

        private Rectangle GetRectangle()
        {
            bordRectangle = new Rectangle();

            bordRectangle.X = Math.Min(oldPoint.X, currentPoint.X);
            bordRectangle.Y = Math.Min(oldPoint.Y, currentPoint.Y);

            bordRectangle.Width = Math.Abs(oldPoint.X - currentPoint.X);
            bordRectangle.Height = Math.Abs(oldPoint.Y - currentPoint.Y);

            return bordRectangle;
        }

        private PointF[] StarPoints(Rectangle bounds)
        {
            // Make room for the points.
            PointF[] pts = new PointF[5];

            double rx = bounds.Width / 2;
            double ry = bounds.Height / 2;
            double cx = bounds.X + rx;
            double cy = bounds.Y + ry;

            // Start at the top.
            double theta = -Math.PI / 2;
            double dtheta = 4 * Math.PI / 5;
            for (int i = 0; i < 5; i++)
            {
                pts[i] = new PointF(
                    (float)(cx + rx * Math.Cos(theta)),
                    (float)(cy + ry * Math.Sin(theta)));
                theta += dtheta;
            }

            return pts;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { Close(); }

        private void eraser_Click(object sender, EventArgs e)
        {
            index = 2;
            pen2.Color = Color.Black;
            pen2.Width = (float)brushSize2.Value;
        }

        private void eraase_size(object sender, EventArgs e)
        {
            pen2.Width = (float)brushSize2.Value;
        }

        private void brush_size(object sender, EventArgs e)
        {
            pen.Width = (float)brushSize.Value;
        }

      
        static Point setPoint(PictureBox pictureBox, Point point)
        {
            float px = 1f * pictureBox.Image.Width / pictureBox.Width;
            float py = 1f * pictureBox.Image.Height / pictureBox.Height;

            return new Point((int)(point.X * px), (int)(point.Y * py));
        }

        private void changeColor()
        {
            pen.Color = pic_color.BackColor;
            pen0.Color = pic_color.BackColor;
            solidBrush = new SolidBrush(pic_color.BackColor);
        }

        private void color_palette_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = setPoint(color_palette, e.Location);
            pic_color.BackColor = ((Bitmap)color_palette.Image).GetPixel(point.X, point.Y);
            changeColor();
        }

        private void brush_Click(object sender, EventArgs e)
        {
            index = 1;
            pen.Color = pic_color.BackColor;
            pen.Width = (float)brushSize.Value;
        }

        private void colorBox_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            pic_color.BackColor = cd.Color;
            changeColor();
        }

        private void penPaint_Click(object sender, EventArgs e)
        {
            index = 0;
            pen0.Color = pic_color.BackColor;
            pen0.Width = 1;
        }
        
        private void save1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "png Files (*png) | *.png";
            saveFileDialog.DefaultExt = "png";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                surface.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void clearToolStrip_Click(object sender, EventArgs e)
        {
            g.Clear(Color.Black);
            canvasPanel.Image = surface;
            index = 0;
            canvasPanel.Visible = true;

            ChangeVisible();
            textBox1.Text = String.Empty;
        }

        private void canvas_paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (paint)
            {
                if (index == 3) // ellipse
                {
                    pen.DashStyle = DashStyle.Solid;
                    g.DrawEllipse(pen, GetRectangle());
                }

                if (index == 4) //rectangle
                {
                    pen.DashStyle = DashStyle.Solid;
                    g.DrawRectangle(pen, GetRectangle());
                }

                if (index == 5) //line
                {
                    pen.DashStyle = DashStyle.Solid;
                    g.DrawLine(pen, cX, cY, x, y);
                }

                if (index == 7)
                {
                    pen.DashStyle = DashStyle.Solid;
                    g.FillEllipse(solidBrush, GetRectangle());
                }

                if (index == 8)
                {
                    pen.DashStyle = DashStyle.Solid;
                    g.FillRectangle(solidBrush, GetRectangle());
                }

                if (index == 9)
                {
                    pen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
                    pen.DashPattern = new float[] { 10, 2 };
                    g.DrawLine(pen, cX, cY, x, y);
                }

                if (index == 10) // triangle
                {
                    pen.DashStyle = DashStyle.Solid;

                    bordRectangle = GetRectangle();
                    Point point1 = new Point() { X = bordRectangle.X, Y = bordRectangle.Y + bordRectangle.Height };
                    Point point2 = new Point() { X = bordRectangle.X + (bordRectangle.Width / 2), Y = bordRectangle.Y };
                    Point point3 = new Point() { X = bordRectangle.X + bordRectangle.Width, Y = bordRectangle.Y + bordRectangle.Height };
                    g.DrawPolygon(pen, new Point[] { point1, point2, point3 });
                }

                if (index == 11) // FillTriangle
                {
                    pen.DashStyle = DashStyle.Solid;

                    bordRectangle = GetRectangle();
                    Point point1 = new Point() { X = bordRectangle.X, Y = bordRectangle.Y + bordRectangle.Height };
                    Point point2 = new Point() { X = bordRectangle.X + (bordRectangle.Width / 2), Y = bordRectangle.Y };
                    Point point3 = new Point() { X = bordRectangle.X + bordRectangle.Width, Y = bordRectangle.Y + bordRectangle.Height };

                    g.FillPolygon(solidBrush, new Point[] { point1, point2, point3 });
                }

                if (index == 13) //arrow
                {
                    pen.DashStyle = DashStyle.Solid;
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    g.DrawLine(pen, cX, cY, x, y);
                }

                if (index == 14) //Star
                {
                    pen.DashStyle = DashStyle.Solid;
                    bordRectangle = GetRectangle();
                    g.DrawPolygon(pen, StarPoints(bordRectangle));
                }

                if (index == 15)
                {
                    pen.DashStyle = DashStyle.Solid;
                    bordRectangle = GetRectangle();
                    g.FillPolygon(solidBrush, StarPoints(bordRectangle));
                }
            }
        }

        private void canvas_mouseClick(object sender, MouseEventArgs e)
        {
            if (index == 6)
            {
                Point point = e.Location;
                Fill(surface, point.X, point.Y, pic_color.BackColor);
            }

            if (index == 12)
            {
                Color clr = surface.GetPixel(e.X, e.Y);
                pic_color.BackColor = clr;
                pen0.Color = pic_color.BackColor;
                pen.Color = pic_color.BackColor;
                solidBrush = new SolidBrush(pic_color.BackColor);
            }
        }

        private void text_fill_Click(object sender, EventArgs e)
        {
            index = 6;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            index = 7;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            index = 8;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            index = 9;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen the_pen = new Pen(Color.Red, 20))
            {
                Point[] pts = {
                                new Point(520, 520),
                                new Point(620, 520),
                                new Point(570, 620)
                                };
                the_pen.CompoundArray = new float[] { 0.0f, 0.4f, 0.6f, 1.0f };
                g.DrawPolygon(the_pen, pts);

                the_pen.Color = Color.Yellow;
                the_pen.CompoundArray = new float[] { 0.0f, 0.1f, 0.3f, 0.7f, 0.9f, 1.0f };
                g.DrawEllipse(the_pen, 400, 520, 100, 100);
            }
        }

        private void ellipse_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void line_Click(object sender, EventArgs e)
        {
            index = 5;
        }

        private void triangle_Click(object sender, EventArgs e)
        {
            index = 10;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            index = 11;
        }

        private void switchButton_Click(object sender, EventArgs e)
        {
           pic_color.BackColor = Color.White;
            pen0.Color = pic_color.BackColor;
            pen.Color = pic_color.BackColor;
            solidBrush = new SolidBrush(pic_color.BackColor);
        }

        private void eyeDropButton_Click(object sender, EventArgs e)
        {
            index = 12;
        }


        private void arrow1_Click(object sender, EventArgs e)
        {
            index = 13;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            index = 14;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            index = 15;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangeVisible();

            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "png Files (*png) | *.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        clearToolStrip_Click(sender, e);

                        using (myStream)
                        {
                            Image img = Image.FromStream(myStream);
                            canvasPanel.Width = img.Width;
                            canvasPanel.Height = img.Height;

                            surface = new Bitmap(img);
                            g = Graphics.FromImage(surface);
                            canvasPanel.Image = surface;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

            ChangeVisible();

            this.Hide();

            System.Threading.Thread.Sleep(1000);
            SendKeys.Send("{PRTSC}");
            Image img = Clipboard.GetImage();

            canvasPanel.Width = img.Width;
            canvasPanel.Height = img.Height;

            surface = new Bitmap(img);
            g = Graphics.FromImage(surface);
            canvasPanel.Image = surface;
            canvasPanel.Image = surface;

            this.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            clearToolStrip_Click(sender, e);
            canvasPanel.Visible = false;
            textBox1.Visible = true;
            print.Visible = true;
            saveText.Visible = true;
            path.Visible = true;
            label1.Visible   = true;
            label2.Visible   = true;
            label3.Visible   = true;
            fontBox.Visible  = true;
            sizeBox.Visible  = true;
            styleBox.Visible = true;

           
        }

        string f = "Time New Romans";
        int style = (int)FontStyle.Bold;
        float size = 14f;
        
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(textBox1.Text, new Font(f, size, (FontStyle)style), solidBrush, new PointF(100, 100));
        }

       
        private void saveText_Click_1(object sender, EventArgs e)
        {
            if (path0 == "")
            {
                path0 = @"D:\temp.text";
            }
            else
            {
                string exe = System.IO.Path.GetExtension(path0) == String.Empty ? ".text" : System.IO.Path.GetExtension(path0);

                string fName = System.IO.Path.GetFileName(path0) == String.Empty ? "temp" : System.IO.Path.GetFileName(path0);

                string p = System.IO.Path.GetPathRoot(path0) == String.Empty ? @"D:\" : System.IO.Path.GetPathRoot(path0);

                path0 = p + fName + exe;
            }

          
            StreamWriter streamWriter = new StreamWriter(path0);

            streamWriter.Write(textBox1.Text);
            streamWriter.Close();
        }

        private void print_Click_1(object sender, EventArgs e)
        {
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void path_TextChanged(object sender, EventArgs e)
        {
            path0 = path.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void fontBox_TextChanged(object sender, EventArgs e)
        {
            f = fontBox.Text;
        }

        private void styleBox_TextChanged(object sender, EventArgs e)
        {

           int x = Convert.ToInt32(styleBox.Text);
           if (x==0 || x==1 || x == 2 || x == 4 || x==8)
               style = x;
        }

        private void sizeBox_TextChanged(object sender, EventArgs e)
        {
            size = Convert.ToSingle(sizeBox.Text);
        }

        private void canvasPanel_Click(object sender, EventArgs e)
        {

        }

     
        private void rectangle_Click(object sender, EventArgs e)
        {
            index = 4;
        }

        private void validate(Bitmap bitmap, Stack<Point> points, int x, int y, Color oldColor, Color newColor)
        {
            Color color = bitmap.GetPixel(x, y);
            if (oldColor == color)
            {
                points.Push(new Point(x, y));
                bitmap.SetPixel(x, y, newColor);
            }
        }

        public void Fill(Bitmap bitmap, int x, int y, Color color)
        {
            Color old = bitmap.GetPixel(x, y);
            Stack<Point> points = new Stack<Point>();
            points.Push(new Point(x, y));
            

            if (old == color)
            {return;}

            bitmap.SetPixel(x, y, color);

            while (points.Count > 0)
            {
                Point point = points.Pop();
                if (point.X > 0 && point.Y > 0 && point.X < bitmap.Width - 1 && point.Y < bitmap.Height - 1)
                {
                    validate(bitmap, points, point.X - 1, point.Y, old, color);

                    validate(bitmap, points, point.X, point.Y - 1, old, color);

                    validate(bitmap, points, point.X + 1, point.Y, old, color);

                    validate(bitmap, points, point.X, point.Y + 1, old, color);
                }
            }
        }
    }
}


