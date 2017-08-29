using System.Text.RegularExpressions;
using WeChatMVC.Models;
using System.Web.Mvc;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace WeChatMVC.Models
{
    public class ClassTableob
    {
        public static List<List<Classob>> ClassTable = new List<List<Classob>>();
        public static DateTime TermBegin = new DateTime(2017, 9, 11);
        public ClassTableob(string html)
        {
            if (ClassTable.Count == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    List<Classob> list = new List<Classob>();
                    ClassTable.Add(list);
                }
            }
            Regex regex = new Regex("<td[^>]*>(?<class>[^<]*)</td><td[^>]*>\\d+</td><td[^>]*>(?<teacher>[^<]*)</td><td[^>]*><font size=1>(?<date>[^<]*)</td><td [^>]*><font size=1>(?<room>[^<]*)</td><td[^>]*>[^<]*</td><td[^>]*>[^<]*</td><td[^>]*>[^<]*</td>");
            MatchCollection mc = regex.Matches(html);
            foreach (Match m in mc)
            {
                GroupCollection gc = m.Groups;
                string teacher = gc["teacher"].Value;
                string classname = gc["class"].Value;
                string date = gc["date"].Value;
                string room = gc["room"].Value;
                Classob now = new Classob(teacher, classname, date, room);
                ClassTable[now.weekcode].Add(now);
            }
            sort();
        }
        private void sort()
        {
            foreach (List<Classob> list in ClassTable)
            {
                if (list.Count <= 1)
                    continue;
                sortlist(list, 0, list.Count - 1);
            }
        }
        private void sortlist(List<Classob> list, int begin, int end)
        {
            Classob temp;
            if (end <= begin)
                return;
            int a = begin; int b = end;
            while (b != a)
            {
                while (Classob.isAearlier(list[a], list[b]) && b != a)
                {
                    a++;
                }
                temp = list[a];
                list[a] = list[b];
                list[b] = temp;
                while (Classob.isAearlier(list[a], list[b]) && b != a)
                {
                    b--;
                }
                temp = list[a];
                list[a] = list[b];
                list[b] = temp;
            }
            sortlist(list, begin, a - 1);
            sortlist(list, b + 1, end);
        }
        private static int getweeknum(DateTime today)
        {
            int days = (today - TermBegin).Days;
            return days / 7 + 1;
        }
        private static int getweekcode(DateTime today)
        {
            return (int)today.DayOfWeek;
        }
        public List<Classob> GetClassToday(DateTime today)
        {
            List<Classob> result = new List<Classob>();
            int weekcode = getweekcode(today);
            int weeknum = getweeknum(today);
            foreach (Classob clas in ClassTable[weekcode])
            {
                if (clas.isToday(weeknum))
                    result.Add(clas);
            }
            return result;
        }
        public string GetStringToday(DateTime today)
        {
            string result = "";
            List<Classob> todayclasses = GetClassToday(today);
            for (int i = 0; i < todayclasses.Count; i++)
            {
                //最终呈现给用户的字符处理好看一点
                result += todayclasses[i].classname;
            }
            if (result == "")
                return "今天并没有安排课程！";
            return result;
        }
    }
    public class Classob
    {
        public string teacher = "";
        public string classname = "";
        public int timebegin = 0;
        public int timeend = 11;
        public int datebegin = 0;
        public int dateend = 20;
        public string weekday = "";
        public int weekcode = 0;
        public string room = "";
        public string quanorsuang = "";
        public bool isshuang = true;
        public bool isdan = true;
        public Classob(string thisteacher, string thisclassname, string thisdate, string thisroom)
        {
            teacher = thisteacher;
            classname = thisclassname;
            room = thisroom;
            Regex regex = new Regex("(?<weekday>[^\\s]*)\\s+(?<timebegin>\\d+)-(?<timeend>\\d+)节\\s+(?<datebegin>\\d+)-(?<dateend>\\d+)(?<quanorshuang>.*)$");
            GroupCollection gc = regex.Match(thisdate).Groups;
            timebegin = Convert.ToInt32(gc["timebegin"].Value);
            timeend = Convert.ToInt32(gc["timeend"].Value);
            datebegin = Convert.ToInt32(gc["datebegin"].Value);
            dateend = Convert.ToInt32(gc["dateend"].Value);
            weekday = gc["weekday"].Value;
            quanorsuang = gc["quanorshuang"].Value;
            switch (quanorsuang)
            {
                case "双周":
                    isdan = false;
                    break;
                case "单周":
                    isshuang = false;
                    break;
                default:
                    break;
            }
            switch (weekday)
            {
                case "周一":
                    weekcode = 1;
                    break;
                case "周二":
                    weekcode = 2;
                    break;
                case "周三":
                    weekcode = 3;
                    break;
                case "周四":
                    weekcode = 4;
                    break;
                case "周五":
                    weekcode = 5;
                    break;
                case "周六":
                    weekcode = 6;
                    break;
                default:
                    weekcode = 0;
                    break;
            }
        }
        public static bool isAearlier(Classob a, Classob b)
        {
            return (a.timebegin <= b.timebegin) ? true : false;
        }
        public bool isToday(int weeknum)
        {
            if (weeknum % 2 == 1 && isdan)
            {
                return true;
            }
            else if (weeknum % 2 == 0 && isshuang)
            {
                return true;
            }
            return false;
        }
    }
}