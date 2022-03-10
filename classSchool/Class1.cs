using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace classSchool
{
    public class Class1
    {
        Random rand = new Random();
        // Генерация оценок, посещаемости на 10 дней вперед, начиная с текущей даты со списком переданных студентов
        public List<Mark> GetMarks(DateTime now, List<Students> students)
        {
            List<string> estimation =new List<string>() { "2", "3", "4", "5", "п", "н", "б"," " };
            List<Mark> marks = new List<Mark>();
            int len = estimation.Count;
            
            foreach (Students student in students)
            {
                for (int i = 0; i < 10; i++)
                {
                    int r =rand.Next(len);
                    marks.Add(new Mark
                        (
                            now.AddDays(i),
                            estimation[r],
                            student)
                        );
                }
            }
            return marks;
        }
        // Вычисление среднего арифметического значения оценки в меньшую сторону 
        public double MinAVG(string[] marks)
        {
            double i = 0;
            double sum = 0;
            foreach (string mark in marks)
            {
                if (int.TryParse(mark, out int est))
                {
                    sum += est;
                    i++;
                }
            }
            string res = (sum / i).ToString();
            if (res.Contains(','))
            return double.Parse(res.Substring(0, res.IndexOf(',')+2));
            else return double.Parse(res);
        }

        // Вычисление количество прогулов за месяц за период
        public int[] GetCountTruancy(List<Mark> marks)
        {
            return GetCountPasses(marks, "н");
        }

        // Вычисление количество пропусков по болезни за месяц за период
        public int[] GetCountDisease(List<Mark> marks)
        {
            return GetCountPasses(marks, "б");
        }

        private static int[] GetCountPasses(List<Mark> marks, string passes)
        {
            Dictionary<int, int> res = new Dictionary<int, int>();
            var tt = (from m in marks
                      group m by m.date.Month into g
                      select new { date = g.Key }).ToList();
            foreach (var t in tt)
            {
                int prom = marks.FindAll(m => m.date.Month == t.date && m.Estimation == passes).Count;
                res.Add(t.date, prom);
            }
            return res.Select(x => x.Value).ToArray();
        }

        // Генерация номера студенческого билета в формате: yyyy.group.initial
        public string GetStudNumber(int year, int group, string fio)
        {
            string[] FIO = fio.Split();
            return $"{year}.{group}.{FIO[0][0]}{FIO[1][0]}{FIO[2][0]}";
        }
    }

    public class Students
    {
     public   int year { get; set; }
        public int group { get; set; }
        public string fio { get; set; }
        public Students(int y, int g, string FIO)
        {
            year = y;
            group = g;
            fio = FIO;
        }
    }

    public class Mark
    {
        public DateTime date { get; set; }
        public string Estimation { get; set; }
        public Students student { get; set; }
        public Mark(DateTime dt, string est, Students std)
        {
            date = dt;
            Estimation = est;
            student = std;
        }
    }
}
