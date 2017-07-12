using System.Text.RegularExpressions;
using WeChatMVC.Models;
using System.Web.Mvc;
using System.Drawing;
using System.Collections.Generic;

namespace WeChatMVC.Controllers
{
    class Letter
    {
        public static string[] dic = { "0", "2", "4", "6", "8", "b", "d", "f", "h", "j", "n", "p", "r", "t", "v", "x", "z", "L" };
        public static List<int[,]> map = new List<int[,]>();
        public static int Length { get { return dic.Length; } }
        public static void InitLetter()
        {
            int[,] l0 ={{0,0,1,1,1,1,1,1,1,1,0,0}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{1,1,1,0,0,0,0,0,0,1,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,1,0,0,0,0,0,0,1,1,1}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{0,0,1,1,1,1,1,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] l2 ={{0,0,1,1,0,0,0,0,0,0,1,1}
,{0,1,1,1,0,0,0,0,0,1,1,1}
,{1,1,1,0,0,0,0,0,1,1,1,1}
,{1,1,0,0,0,0,0,1,1,0,1,1}
,{1,1,0,0,0,0,1,1,1,0,1,1}
,{1,1,0,0,0,1,1,1,0,0,1,1}
,{0,1,1,1,1,1,1,0,0,0,1,1}
,{0,0,1,1,1,0,0,0,0,0,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] l4 ={{0,0,0,0,0,0,0,1,1,1,0,0}
,{0,0,0,0,0,0,1,1,1,1,0,0}
,{0,0,0,0,1,1,1,0,1,1,0,0}
,{0,0,0,1,1,1,0,0,1,1,0,0}
,{0,1,1,1,0,0,0,0,1,1,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,0,0,0,0,0,0,0,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] l6 ={{0,0,0,1,1,1,1,1,1,0,0,0}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{1,1,1,0,0,1,1,0,0,1,1,1}
,{1,1,0,0,1,1,0,0,0,0,1,1}
,{1,1,0,0,1,1,1,0,0,0,1,1}
,{1,1,0,0,1,1,1,0,0,0,1,1}
,{1,1,1,0,0,1,1,1,1,1,1,0}
,{0,1,1,0,0,0,1,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] l8 ={{0,0,1,1,1,1,0,1,1,1,0,0}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{0,0,1,1,1,1,0,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lb ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,1,1,1,1,1,1,0,0,1,1}
,{0,1,1,1,1,0,1,1,1,1,1,0}
,{0,0,0,0,0,0,0,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] ld ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{0,1,1,0,0,0,0,0,0,1,1,0}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{0,0,0,1,1,1,1,1,1,0,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lf ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lh ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lj ={{0,0,0,0,0,0,0,0,1,1,0,0}
,{0,0,0,0,0,0,0,0,1,1,1,0}
,{0,0,0,0,0,0,0,0,0,1,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,0}
,{1,1,1,1,1,1,1,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] ln ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,1,1,1,0,0,0,0,0,0,0,0}
,{0,0,1,1,1,0,0,0,0,0,0,0}
,{0,0,0,0,1,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,1,0,0,0,0}
,{0,0,0,0,0,0,0,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,1,1,1,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lp ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,1,0,1,1,1,0,0,0,0,0}
,{0,1,1,1,1,1,0,0,0,0,0,0}
,{0,0,1,1,1,0,0,0,0,0,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lr ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,1,0,0,0,0}
,{1,1,0,0,0,1,1,1,1,0,0,0}
,{1,1,1,1,1,1,0,1,1,1,1,0}
,{0,1,1,1,1,1,0,0,1,1,1,1}
,{0,0,1,1,1,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lt ={{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lv ={{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,0,0,0,0,0,0,0,0}
,{0,0,1,1,1,1,1,0,0,0,0,0}
,{0,0,0,0,1,1,1,1,1,0,0,0}
,{0,0,0,0,0,0,0,1,1,1,1,1}
,{0,0,0,0,0,0,0,0,0,1,1,1}
,{0,0,0,0,0,0,0,1,1,1,1,1}
,{0,0,0,0,1,1,1,1,1,0,0,0}
,{0,0,1,1,1,1,1,0,0,0,0,0}
,{1,1,1,1,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lx ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,1,0,0,0,0,0,0,1,1,1}
,{0,1,1,1,1,0,0,1,1,1,1,0}
,{0,0,0,1,1,1,1,1,1,0,0,0}
,{0,0,0,0,1,1,1,1,0,0,0,0}
,{0,0,0,1,1,1,1,1,1,0,0,0}
,{0,1,1,1,1,0,0,1,1,1,1,0}
,{1,1,1,0,0,0,0,0,0,1,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lz ={{0,0,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,1,1,1}
,{1,1,0,0,0,0,0,1,1,1,1,1}
,{1,1,0,0,0,0,1,1,1,0,1,1}
,{1,1,0,0,1,1,1,1,0,0,1,1}
,{1,1,0,1,1,1,0,0,0,0,1,1}
,{1,1,1,1,1,0,0,0,0,0,1,1}
,{1,1,1,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
};
            int[,] lL ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            map.Add(l0);
            map.Add(l2);
            map.Add(l4);
            map.Add(l6);
            map.Add(l8);
            map.Add(lb);
            map.Add(ld);
            map.Add(lf);
            map.Add(lh);
            map.Add(lj);
            map.Add(ln);
            map.Add(lp);
            map.Add(lr);
            map.Add(lt);
            map.Add(lv);
            map.Add(lx);
            map.Add(lz);
            map.Add(lL);
        }
        private Letter()
        {

        }
        public static int getwidth(int index)
        {
            return map[index].GetLength(0);
        }
        public static int getheight(int index)
        {
            return map[index].GetLength(1);
        }
        public static int getlength()
        {
            return dic.Length;
        }
    }
    class IdentificatImage
    {
        public Bitmap input;
        public string result;
        public int[,] map;
        public IdentificatImage(Bitmap thisinput)
        {
            if (Letter.map.Count == 0)
            {
                Letter.InitLetter();
            }
            Bitmap noindeximg = thisinput.Clone(
                new Rectangle(0, 0, thisinput.Width, thisinput.Height),
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            input = noindeximg;
            result = identimage();
        }
        private string identimage()
        {
            string result = "";
            map = new int[50, 17];
            binaryzation();
            howmanyqi(5, 5, map);
            int[] score = new int[Letter.Length];
            judger(5, ref score, map);
            int max = -99990; int index = 0;
            for (int k = 0; k < score.Length; k++)
            {
                if (score[k] > max)
                {
                    index = k; max = score[k];
                }
            }
            result = result + Letter.dic[index];
            int now = 5 + Letter.map[index].GetLength(0);
            judger(now, ref score, map);
            max = -99990; index = 0;
            for (int k = 0; k < score.Length; k++)
            {
                if (score[k] > max)
                {
                    index = k; max = score[k];
                }
            }
            result = result + Letter.dic[index];
            now = now + Letter.getwidth(index);
            judger(now, ref score, map);
            max = -99990; index = 0;
            for (int k = 0; k < score.Length; k++)
            {
                if (score[k] > max)
                {
                    index = k; max = score[k];
                }
            }
            result = result + Letter.dic[index];
            now = now + Letter.getwidth(index);
            judger(now, ref score, map);
            max = -99990; index = 0;
            for (int k = 0; k < score.Length; k++)
            {
                if (score[k] > max)
                {
                    index = k; max = score[k];
                }
            }
            result = result + Letter.dic[index];
            return result;
        }
        private void howmanyqi(int x, int y, int[,] map)
        {
            if (x >= 49)
            {
                howmanyqi(5, y + 1, map);
                return;
            }
            if (y >= 17)
            {
                return;
            }
            if (map[x, y] == 0 && getgraylevel(input.GetPixel(x, y)) == 0)
            {
                map[x, y] = 1;
                int now = 0;
                turntoblack(x, y, ref now, map);
                if (now <= 4)
                {
                    clear(x, y, map);
                }
            }
            howmanyqi(x + 1, y, map);
        }
        private void turntoblack(int x, int y, ref int now, int[,] map)
        {
            if (x >= 49 || x <= 4)
            {
                return;
            }
            if (y >= 17 || y <= 4)
            {
                return;
            }
            if (map[x, y] == 2)
            {
                return;
            }
            if (getgraylevel(input.GetPixel(x, y)) == 0)
            {
                now++;
                map[x, y] = 2;
                turntoblack(x + 1, y, ref now, map);
                turntoblack(x - 1, y, ref now, map);
                turntoblack(x, y + 1, ref now, map);
                turntoblack(x, y - 1, ref now, map);
            }
        }
        private void clear(int x, int y, int[,] map)
        {
            if (x >= 49 || x <= 4)
            {
                return;
            }
            if (y >= 17 || y <= 4)
            {
                return;
            }
            if (getgraylevel(input.GetPixel(x, y)) == 0)
            {
                map[x, y] = 0;
                Color newColor = Color.FromArgb(255, 255, 255);
                input.SetPixel(x, y, newColor);
                clear(x + 1, y, map);
                clear(x - 1, y, map);
                clear(x, y + 1, map);
                clear(x, y - 1, map);
            }
        }
        private void binaryzation()
        {
            for (int x = 5; x < map.GetLength(0); x++)
            {
                for (int y = 5; y < 17; y++)
                {
                    Color pixelColor = input.GetPixel(x, y);
                    if (getgraylevel(pixelColor) == 0)
                    {
                        Color newColor = Color.FromArgb(255, 255, 255);
                        input.SetPixel(x, y, newColor);
                    }
                    else if (getgraylevel(pixelColor) <= 764)
                    {
                        Color newColor = Color.FromArgb(0, 0, 0);
                        input.SetPixel(x, y, newColor);
                    }
                    else
                    {
                        Color newColor = Color.FromArgb(255, 255, 255);
                        input.SetPixel(x, y, newColor);
                    }
                }
            }
        }
        private static int getgraylevel(Color input)
        {
            return input.R + input.G + input.B;
        }
        private static void judger(int xbegin, ref int[] score, int[,] imagemap)
        {
            score = new int[Letter.Length];
            for (int i = 0; i < Letter.Length; i++)
            {
                for (int x = xbegin; x < xbegin + Letter.getwidth(i); x++)
                {
                    if (x >= 50)
                    {
                        break;
                    }
                    for (int y = 0; y < Letter.getheight(i); y++)
                    {
                        if (imagemap[x, y + 5] == 2 && Letter.map[i][x - xbegin, y] == 1)
                        {
                            score[i]++;
                        }
                        else if (imagemap[x, y + 5] == 0 && Letter.map[i][x - xbegin, y] == 0)
                        {

                        }
                        else if (imagemap[x, y + 5] == 2 && Letter.map[i][x - xbegin, y] == 0)
                        {
                            score[i] -= 10;
                        }
                        else
                        {
                            score[i] -= 10;
                        }
                    }
                }
            }
        }
    }
    public class APIController : BaseController
    {
        // GET: API
        public JsonResult ty(string studentnum, string pwd)
        {
            string post = string.Format("__EVENTTARGET = &__EVENTARGUMENT = &__LASTFOCUS = &__VIEWSTATE =% 2FwEPDwULLTIxMjk4Nzk4MzUPZBYCZg9kFghmDxBkZBYBZmQCBQ8PFgIeBFRleHRlZGQCBg8PFgIfAAUSMjAxNC85LzE2IDExOjI1OjUyZGQCBw8PFgIfAAUSMjAzNC85LzE2IDExOjI1OjUyZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFBWJ0bm9rg7wsmKWoIGv % 2B2o6SEkUuPN0q80w % 3D & dlljs = st & " +
                "txtuser={0}&txtpwd={1}&__VIEWSTATEGENERATOR=5B21F7B0&__EVENTVALIDATION=%2FwEWCAKAvaGBDAKBwaG%2FAQLMrvvQDQLd8tGoBALWwdnoCALB2tiCDgKd%2B7q4BwL9kpmqCm3geVz7Uj28fz9x4hImTVbLX%2BiL&btnok.x=0&btnok.y=0",
                studentnum, pwd);

            MyHttpHelper httphelper = new MyHttpHelper(@"http://59.78.92.38:8888/sportscore/");
            httphelper.HttpGet();
            httphelper = new MyHttpHelper(@"http://59.78.92.38:8888/sportscore/");
            httphelper.HttpPost(post);
            httphelper = new MyHttpHelper(@"http://59.78.92.38:8888/sportscore/stScore.aspx");
            httphelper.HttpGet();

            Regex re = new Regex("<span id=\"lblname\">\\s*(?<name>[^\\s]*)\\s*</span>[\\s\\S]*?<span id=\"lblmsg\">[\\s\\S]*?早操：[\\s\\S]*?>(?<morningexercises>\\d+)<[\\s\\S]*?课外活动1：[\\s\\S]*?>(?<outclassactivite1>\\d+)<[\\s\\S]*?</span>");
            Match match = re.Match(httphelper.ToString());
            GroupCollection gc = match.Groups;
            string name = gc["name"].Value;
            string morningexercise = gc["morningexercises"].Value;
            string outclassactivite1 = gc["outclassactivite1"].Value;
            string reply = name + "同学，您晨跑次数为：" + morningexercise + ",课外活动次数为：" + outclassactivite1;


            JsonResult json = new JsonResult();
            var data = new object[3];

            data[0] = name;
            data[1] = morningexercise;
            data[2] = outclassactivite1; 
            json.Data = data;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
        public JsonResult lib()
        {
            Regex re = new Regex("(?<=>)\\d+(?=<)");
            MyHttpHelper library = new MyHttpHelper(@"http://lib.ecust.edu.cn:8081/gateseat/lrp.aspx");
            library.HttpGet();
            MatchCollection mc = re.Matches(library.ToString());
            JsonResult json = new JsonResult();
            var data = new object[6];
            for (int i = 1; i <= 6; i++)
            {
                var used = mc[2 * i - 2].Value;
                var remain = mc[2 * i - 1].Value;
                data[i - 1] = new { used, remain };
            }
            json.Data = data;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
        public string jwc(string studentnum, string pwd)
        {
            MyHttpHelper a = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_StudentQueryLogin.aspx");
            a.HttpGet();
            MyHttpHelper b = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/Base/VerifyCode.aspx");
            Bitmap input = b.HttpGetImage();
            IdentificatImage id = new IdentificatImage(input);
            string vc = id.result;
            MyHttpHelper c = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_StudentQueryLogin.aspx");
            c.HttpPost("__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE2OTIxNDU0MTMPZBYCAgEPZBYCAgcPDxYCHgRUZXh0BVDlrabnlJ%2FliJ3lp4vlr4bnoIHkuLrouqvku73or4Hlj7flkI7lha3kvY3jgILlr4bnoIHplb%2FluqbkuI3otoXov4cxMOS4quWtl%2BespuOAgmRkZDanEMgmeoYOigCgOHJXPnTdIOtq&TxtStudentid=" + studentnum + "&TxtPassword=" + pwd + "&txt_verifyCode=" + vc + "&BtnLogin=%E7%99%BB%E5%BD%95&__EVENTVALIDATION=%2FwEWBQKMjOWyBAKf8ICgBwLVqbaRCwLW2qK1CALi44eGDA67X3bLsDOxfx3HDe98WpJ8%2Bncw");
            MyHttpHelper d = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_BigScoreTableDetail.aspx?key=0");
            d.HttpGet();
            string html = d.ToString();
            Regex regex = new Regex("<td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\"" +
                " valign=\"middle\"><font face=\"[^\"]*\">(?<kemu>[^<]*)</font></td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font>" +
                "</td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\" valign=\"middle\">" +
                "<font face=\"[^\"]*\">(?<fengshu>\\d+)</font></td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\" " +
                "valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td>"
                ); string result = "";
            MatchCollection mc = regex.Matches(html);
            foreach (Match item in mc)
            {
                GroupCollection gc = item.Groups;
                result = result + gc["kemu"].Value + ":" + gc["fengshu"].Value + "\n";
            }
            return result;
        }
    }
}