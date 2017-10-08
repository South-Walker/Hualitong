using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using WeChatMVC.Models;
using System.Drawing.Imaging;
using System.IO;


namespace WeChatMVC.Controllers
{
    public class DrawerController : Controller
    {
        // GET: Drawer
        public ActionResult Index()
        {
            return View();
        }
    }
    public class ClassTableDrawer
    {
        public DateTime now;
        static string[] dates = new string[] { "7", "8", "9", "10", "11", "12", "13" };
        static string[] nums = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
        static string[] weekday = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        static string[] day = new string[] { "上午", "下午", "晚上" };
        static Color background = Color.White;
        static int classtablewidth = 1280 / 5 * 3;
        static int classtableheight = 1056;
        static int weekdaywidth = classtablewidth / 8;
        static int weekdayheight = 60;
        static int dinnertimeheight = 30;
        static int dinnertimewidth = classtablewidth;
        static int classwidth = weekdaywidth;
        static int classheight = 78;
        static int daywidth = weekdaywidth / 2;
        static int dayheight = classheight * 4;
        static int width = 1280 / 5 * 3;
        static int clogheight = 30;
        static int height = classtableheight + clogheight;
        delegate float FunctionY(float x);
        static string clogo = "©肖南行South-Walker";
        public Bitmap ClassTableImage = null;
        private Graphics g = null;
        private Pen mypen = new Pen(Color.FromArgb(208, 208, 208), 2);
        private Font biggerfont = new Font("方正卡通简体", 12, FontStyle.Regular, GraphicsUnit.Point);
        private Font smallerfont = new Font("方正卡通简体", 20, FontStyle.Bold, GraphicsUnit.Point);
        public ClassTableDrawer()
        {
            now = DateTime.Now;
            if (now.DayOfWeek == 0)
                now = now.AddDays(1);
        }
        public ClassTableDrawer(DateTime nowdate)
        {
            now = nowdate;
            if (now.DayOfWeek == 0)
                now = now.AddDays(1);
        }
        public void DrawClassTableAndSave(string openid, string path)
        {
            ClassTableImage = new Bitmap(width, height);
            g = Graphics.FromImage(ClassTableImage);
            g.Clear(background);
            initdate();
            drawingbase();
            drawclasses(openid);

            ClassTableImage.Save(path, ImageFormat.Png);
        }
        public Stream DrawClassTableInStream(string openid)
        {
            ClassTableImage = new Bitmap(width, height);
            g = Graphics.FromImage(ClassTableImage);
            g.Clear(background);
            initdate();
            drawingbase();
            drawclasses(openid);
            MemoryStream stream = new MemoryStream();
            ClassTableImage.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
        public void DrawAll(string openid, string root)
        {
            DateTime begin = new DateTime(2017, 9, 11);
            now = begin;
            for (int i = 1; i < 20; i++)
            {
                DrawClassTableAndSave(openid, root + i.ToString() + ".png");
                AddDays(7);
            }
        }
        public void AddDays(double i)
        {
            now = now.AddDays(i);
        }
        private void fillsmoothclassrectangle(Graphics g, RectangleF r, Color color)
        {
            float beginx1 = r.X;
            float beginx2 = r.X + r.Width - r.Width / 3;
            float endx1 = r.X + r.Width / 3;
            float endx2 = r.X + r.Width;

            fillbetweenfunctions(x => { return r.Y + r.Width / 3f - (float)Math.Sqrt(r.Width * r.Width / 9f - (x - r.X - r.Width / 3f) * (x - r.X - r.Width / 3f)); },
                x => { return r.Y + r.Height - r.Width / 3f + (float)Math.Sqrt(r.Width * r.Width / 9f - (x - r.X - r.Width / 3f) * (x - r.X - r.Width / 3f)); },
                g, beginx1, endx1, color);
            fillbetweenfunctions(x => { return r.Y; },
                x => { return r.Y + r.Height; },
                g, endx1, beginx2, color);
            fillbetweenfunctions(x => { return r.Y + r.Width / 3f - (float)Math.Sqrt(r.Width * r.Width / 9f - (x - r.X - r.Width * 2 / 3f) * (x - r.X - r.Width * 2 / 3f)); },
                x => { return r.Y + r.Height - r.Width / 3f + (float)Math.Sqrt(r.Width * r.Width / 9f - (x - r.X - r.Width * 2 / 3f) * (x - r.X - r.Width * 2 / 3f)); },
                g, beginx2, endx2, color);
        }
        private void fillbetweenfunctions(FunctionY funa, FunctionY funb, Graphics g, float beginx, float endx, Color color)
        {
            float y1, y2, y3, y4;
            for (float i = beginx; i < endx; i += 0.5f)
            {
                y1 = funa(i);
                y2 = funa(i + 1);
                y3 = funb(i);
                y4 = funb(i + 1);
                g.DrawLine(new Pen(color), i, y1, i, y3);
            }
        }
        private void drawfunction(FunctionY function, Graphics g, float beginx, float endx)
        {
            float y1, y2;
            for (float i = beginx; i < endx; i += 0.5f)
            {
                y1 = function(i);
                y2 = function(i + 1);
                g.DrawLine(Pens.Black, i, y1, i + 1, y2);
            }
        }
        private void drawingbase()
        {
            //横线
            g.DrawLine(mypen, 0, 0, classtablewidth, 0);
            g.DrawLine(mypen, 0, classtableheight, classtablewidth, classtableheight);
            g.DrawLine(mypen, 0, weekdayheight, classtablewidth, weekdayheight);
            g.DrawLine(mypen, 0, weekdayheight + classheight * 4, classtablewidth, weekdayheight + classheight * 4);
            g.DrawLine(mypen, 0, weekdayheight + classheight * 4 + dinnertimeheight, classtablewidth, weekdayheight + classheight * 4 + dinnertimeheight);
            g.DrawLine(mypen, 0, weekdayheight + classheight * 8 + dinnertimeheight, classtablewidth, weekdayheight + classheight * 8 + dinnertimeheight);
            g.DrawLine(mypen, 0, weekdayheight + classheight * 8 + dinnertimeheight * 2, classtablewidth, weekdayheight + classheight * 8 + dinnertimeheight * 2);
            for (int i = 1; i < 5; i++)
            {
                g.DrawLine(mypen, weekdaywidth / 2, weekdayheight + i * classheight,
                    classtablewidth, weekdayheight + i * classheight);

                g.DrawLine(mypen, weekdaywidth / 2, weekdayheight + dinnertimeheight + (i + 4) * classheight,
                    classtablewidth, weekdayheight + dinnertimeheight + (i + 4) * classheight);

                g.DrawLine(mypen, weekdaywidth / 2, weekdayheight + dinnertimeheight * 2 + (i + 8) * classheight,
                    classtablewidth, weekdayheight + dinnertimeheight * 2 + (i + 8) * classheight);
            }
            //竖线
            for (int i = 0; i <= 8; i++)
            {
                if (i == 0 || i == 8)
                    g.DrawLine(mypen, i * weekdaywidth, 0, i * weekdaywidth, classtableheight);
                else
                {
                    g.DrawLine(mypen, i * weekdaywidth, 0, i * weekdaywidth, weekdayheight + classheight * 4);
                    g.DrawLine(mypen, i * weekdaywidth, weekdayheight + classheight * 4 + dinnertimeheight,
                        i * weekdaywidth, weekdayheight + classheight * 8 + dinnertimeheight);
                    g.DrawLine(mypen, i * weekdaywidth, weekdayheight + classheight * 8 + dinnertimeheight * 2,
                        i * weekdaywidth, classtableheight);
                }
            }
            g.DrawLine(mypen, daywidth, weekdayheight, daywidth, weekdayheight + 4 * classheight);
            g.DrawLine(mypen, daywidth, weekdayheight + 4 * classheight + dinnertimeheight, daywidth, weekdayheight + 8 * classheight + dinnertimeheight);
            g.DrawLine(mypen, daywidth, weekdayheight + 8 * classheight + dinnertimeheight * 2, daywidth, classtableheight);


            StringFormat centerStringFormat = new StringFormat();
            centerStringFormat.Alignment = StringAlignment.Center;
            centerStringFormat.LineAlignment = StringAlignment.Center;

            StringFormat leftStringFormat = new StringFormat();
            leftStringFormat.Alignment = StringAlignment.Near;
            leftStringFormat.LineAlignment = StringAlignment.Near;

            StringFormat rightStringFormat = new StringFormat();
            rightStringFormat.Alignment = StringAlignment.Far;
            rightStringFormat.LineAlignment = StringAlignment.Far;
            //月份
            g.DrawString(DateTime.Now.Month + "月", smallerfont, Brushes.Black, new Rectangle(0, weekdayheight / 3, weekdaywidth, weekdayheight * 2 / 3), centerStringFormat);
            //星期几
            for (int i = 0; i < weekday.Length; i++)
            {
                Rectangle now = new Rectangle(weekdaywidth * (i + 1), 0, weekdaywidth, weekdayheight / 3);
                g.DrawString(weekday[i], biggerfont, Brushes.Black, now, leftStringFormat);
                now = new Rectangle(weekdaywidth * (i + 1), weekdayheight / 3, weekdaywidth, weekdayheight * 2 / 3);
                g.DrawString(dates[i], smallerfont, Brushes.Black, now, centerStringFormat);
            }

            //时段
            for (int i = 0; i < day.Length; i++)
            {
                Rectangle now = new Rectangle(0, weekdayheight + dayheight * i + dinnertimeheight * i, daywidth, dayheight / 2);
                g.DrawString(day[i].Substring(0, 1), smallerfont, Brushes.Black, now, centerStringFormat);
                now = new Rectangle(0, weekdayheight + dayheight * i + dinnertimeheight * i + dayheight / 2, daywidth, dayheight / 2);
                g.DrawString(day[i].Substring(1, 1), smallerfont, Brushes.Black, now, centerStringFormat);
            }

            //节数
            for (int i = 1; i < nums.Length; i++)
            {
                int ynow = 0;
                ynow = (i - 1) / 4 * dinnertimeheight + (i - 1) * classheight + weekdayheight;
                Rectangle now = new Rectangle(weekdaywidth / 2, ynow, weekdaywidth / 2, classheight);
                g.DrawString(nums[i], biggerfont, Brushes.Black, now, centerStringFormat);
            }


        }
        private void drawclasses(string openid)
        {
            StringFormat centerStringFormat = new StringFormat();
            centerStringFormat.Alignment = StringAlignment.Center;
            centerStringFormat.LineAlignment = StringAlignment.Center;

            StringFormat leftStringFormat = new StringFormat();
            leftStringFormat.Alignment = StringAlignment.Near;
            leftStringFormat.LineAlignment = StringAlignment.Near;

            StringFormat rightStringFormat = new StringFormat();
            rightStringFormat.Alignment = StringAlignment.Far;
            rightStringFormat.LineAlignment = StringAlignment.Far;

            List<List<Classob>> thisweek = getclasstable(openid).GetThisWeekClassTable(now);

            for (int i = 0; i < thisweek.Count; i++)
            {
                List<Classob> now = thisweek[i];
                for (int k = 0; k < now.Count; k++)
                {
                    drawaclass(g, now[k]);
                }
            }
            //clogo
            g.DrawString(clogo, biggerfont, Brushes.Black, new Rectangle(0, classtableheight, classtablewidth, clogheight), rightStringFormat);
        }
        private ClassTableob getclasstable(string openid)
        {
            FileStream fs = new FileStream(@"C:\Users\Administrator\Desktop\hualitongbuffer\classtablehtml\" + openid, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string html = sr.ReadToEnd();
            ClassTableob table = new ClassTableob(html, new DateTime(2017, 9, 11));
            return table;
        }
        private void drawaclass(Graphics g, Classob aclass)
        {
            Pen mypen = new Pen(Color.Black, 2);
            Rectangle rect = getclassrect(aclass);
        //    g.FillRectangle(Brushes.White, rect);//直角时涂白使用

            StringFormat centerStringFormat = new StringFormat();
            centerStringFormat.Alignment = StringAlignment.Center;
            centerStringFormat.LineAlignment = StringAlignment.Center;

            //  g.FillRectangle(new SolidBrush(aclass.colorintable), rect);//这个不是圆角
            fillsmoothclassrectangle(g, rect, aclass.colorintable);
            Font font = new Font("方正卡通简体", 12, FontStyle.Bold, GraphicsUnit.Point);
            g.DrawString(aclass.classname + "\r\n" + aclass.room, font, Brushes.Black, rect, centerStringFormat);
         //   g.DrawRectangle(mypen, rect);//直角时描边使用
        }
        private Rectangle getclassrect(Classob aclass)
        {
            int begin = aclass.timebegin;
            int week = aclass.weekcode + 1;
            int x = week * weekdaywidth;
            int y;
            int end = aclass.timeend;
            int length = (end - begin + 1) * classheight;
            if (begin <= 4)
                y = weekdayheight + (begin - 1) * classheight;
            else if (begin <= 8)
                y = weekdayheight + dinnertimeheight + (begin - 1) * classheight;
            else
                y = weekdayheight + dinnertimeheight * 2 + (begin - 1) * classheight;
            return new Rectangle(x, y, classwidth, length);
        }
        private void initdate()
        {
            int weekdaynow = (int)now.DayOfWeek;
            now = now.AddDays(weekdaynow * -1);
            for (int i = 0; i < dates.Length; i++)
            {
                dates[i] = now.Day.ToString();
                now = now.AddDays(1);
            }
        }
    }
    public class RandomColor
    {
        List<Color> Colors = new List<Color>();
        private int nowindex = -1;
        public RandomColor(int seed)
        {
            initlist();
            randomcolors(seed);
        }
        public RandomColor(string studentnum)
        {
            int total = 0;
            foreach (var item in studentnum) 
            {
                total += item;
            }
            initlist();
            randomcolors(total);
        }
        private void randomcolors(int seed)
        {
            Random r = new Random(seed);
            for (int i = Colors.Count - 1; i >= 0; i--)
            {
                int next = r.Next(0, i + 1);
                Color temp = Colors[next];
                Colors[next] = Colors[i];
                Colors[i] = temp;
            }
        }
        private void initlist()
        {
            Colors.Add(Color.FromArgb(255, 192, 203));
            Colors.Add(Color.FromArgb(199, 21, 133));
            Colors.Add(Color.FromArgb(139, 0, 139));
            Colors.Add(Color.FromArgb(255, 0, 255));
            Colors.Add(Color.FromArgb(153, 50, 204));
            Colors.Add(Color.FromArgb(123, 104, 238));
            Colors.Add(Color.FromArgb(65, 105, 225));
            Colors.Add(Color.FromArgb(100, 149, 237));
            Colors.Add(Color.FromArgb(30, 144, 255));
            Colors.Add(Color.FromArgb(0, 191, 255));
            Colors.Add(Color.FromArgb(95, 158, 160));
            Colors.Add(Color.FromArgb(127, 255, 170));
            Colors.Add(Color.FromArgb(60, 179, 113));
            Colors.Add(Color.FromArgb(46, 139, 87));
            Colors.Add(Color.FromArgb(0, 128, 0));
            Colors.Add(Color.FromArgb(255, 215, 0));
            Colors.Add(Color.FromArgb(218, 165, 32));
            Colors.Add(Color.FromArgb(255, 165, 0));
            Colors.Add(Color.FromArgb(160, 82, 45));
            Colors.Add(Color.FromArgb(255, 69, 0));
            Colors.Add(Color.FromArgb(240, 128, 128));
            Colors.Add(Color.FromArgb(192, 92, 92));
            Colors.Add(Color.FromArgb(178, 34, 34));
        }
        public Color Next()
        {
            nowindex++;
            if (nowindex >= Colors.Count)
                nowindex = 0;
            return Colors[nowindex];
        }
    }
}