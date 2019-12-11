using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using laba5_lib;
using System.Text;
using System;

namespace homework
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public class ParallelSearchResult
        {
            public string Word { get; set; }

            public int Dist { get; set; }

            public int ThreadNum { get; set; }
        }

        public class MinMax
        {
            public int Min { get; set; }
            public int Max { get; set; }

            public MinMax(int pmin, int pmax)
            {
                Min = pmin;
                Max = pmax;
            }
        }

        public static class SubArrays
        {
            public static List<MinMax> DivideSubArrays(int beginIndex, int endIndex, int subArraysCount)
            {
                List<MinMax> result = new List<MinMax>();
                if ((endIndex - beginIndex) <= subArraysCount)
                {
                    result.Add(new MinMax(0, (endIndex - beginIndex)));
                }
                else
                {
                    int delta = (endIndex - beginIndex) / subArraysCount;
                    int currentBegin = beginIndex;
                    while ((endIndex - currentBegin) >= 2 * delta)
                    {
                        result.Add(new MinMax(currentBegin, currentBegin + delta));
                        currentBegin += delta;
                    }
                    result.Add(new MinMax(currentBegin, endIndex));
                }
                return result;
            }
        }

        class ParallelSearchThreadParam
        {
            public List<string> TempList { get; set; }

            public string WordPattern { get; set; }

            public int MaxDist { get; set; }

            public int ThreadNum { get; set; }
        }

        public static List<ParallelSearchResult> ArrayThreadTask(object paramObj)
        {
            ParallelSearchThreadParam param = (ParallelSearchThreadParam)paramObj;
            string wordUpper = param.WordPattern.Trim().ToUpper();
            List<ParallelSearchResult> Result = new List<ParallelSearchResult>();
            foreach (string str in param.TempList)
            {
                int dist = LevenshteinDistance.Distance(str.ToUpper(), wordUpper);
                if (dist <= param.MaxDist)
                {
                    ParallelSearchResult temp = new ParallelSearchResult()
                    {
                        Word = str,
                        Dist = dist,
                        ThreadNum = param.ThreadNum
                    };

                    Result.Add(temp);
                }
            }
            return Result;
        }

        readonly List<string> words = new List<string>();

        private void ButtonLoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog
            {
                Filter = "Текстовые Файлы|*.txt"
            };
            bool? result = fd.ShowDialog();
            if (result == true)
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                string text = File.ReadAllText(fd.FileName);
                char[] separators = new char[] { ' ', '.', ',', '?', '!', '\"', '<', '>', '/', '\t', '\n' };
                string[] textArray = text.Split(separators);
                foreach (string strTemp in textArray)
                {
                    string str = strTemp.Trim();
                    if (str != "")
                    {
                        if (!words.Contains(str)) words.Add(str);
                    }
                }
                timer.Stop();
                LabelTimer.Content = timer.Elapsed.ToString();
                LabelCountWords.Content = words.Count.ToString();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать файл");
            }
        }

        private void ButtonSearchWord_Click(object sender, RoutedEventArgs e)
        {
            string wordSearch = TextBoxCurrentWord.Text.Trim();
            if (!string.IsNullOrWhiteSpace(wordSearch) && words.Count > 0)
            {
                string wordUpper = wordSearch.ToUpper();
                List<string> tempList = new List<string>();
                Stopwatch timer = new Stopwatch();
                timer.Start();
                foreach (string str in words)
                {
                    if (str.ToUpper().Contains(wordUpper))
                    {
                        tempList.Add(str);
                    }
                }
                timer.Stop();
                LabelTimeSearch.Content = timer.Elapsed.ToString();
                ListBoxResult.ItemsSource = tempList;
            }
            else
            {
                MessageBox.Show("Выберите файл и введите слово для поиска");
            }
        }

        private void ButtonCalcDistance_Click(object sender, RoutedEventArgs e)
        {
            string currentWord = TextBoxCurrentWord.Text.Trim();
            if (!string.IsNullOrWhiteSpace(currentWord) && words.Count > 0)
            {
                if (!int.TryParse(TextBoxMaxDistance.Text.Trim(), out int maxDistance))
                {
                    MessageBox.Show("Введите максимальное расстояние");
                    return;
                }
                if (maxDistance < 1 || maxDistance > 5)
                {
                    MessageBox.Show("Максимальное расстояние должно быть в диапазоне от 1 до 5");
                    return;
                }
                if (!int.TryParse(TextBoxThreadCount.Text.Trim(), out int ThreadCount))
                {
                    MessageBox.Show("Необходимо указать количество потоков");
                    return;
                }
                Stopwatch timer = new Stopwatch();
                timer.Start();
                List<ParallelSearchResult> Result = new List<ParallelSearchResult>();
                List<MinMax> arrayDivList = SubArrays.DivideSubArrays(0, words.Count, ThreadCount);
                int count = arrayDivList.Count;
                Task<List<ParallelSearchResult>>[] tasks = new Task<List<ParallelSearchResult>>[count];
                for (int i = 0; i < count; i++)
                {
                    List<string> tempTaskList = words.GetRange(arrayDivList[i].Min, arrayDivList[i].Max - arrayDivList[i].Min);
                    tasks[i] = new Task<List<ParallelSearchResult>>(ArrayThreadTask,
                        new ParallelSearchThreadParam()
                        {
                            TempList = tempTaskList,
                            MaxDist = maxDistance,
                            ThreadNum = i,
                            WordPattern = currentWord
                        });
                    tasks[i].Start();
                }
                Task.WaitAll(tasks);
                timer.Stop();
                for (int i = 0; i < count; i++)
                {
                    Result.AddRange(tasks[i].Result);
                }
                timer.Stop();
                LabelTimeCalc.Content = timer.Elapsed.ToString();
                LabelThreadCount.Content = count.ToString();
                List<string> tempList = new List<string>();
                foreach (var x in Result)
                {
                    string temp = x.Word + " (расстояние: " + x.Dist.ToString() + "; поток: " + x.ThreadNum.ToString() + ")";
                    tempList.Add(temp);
                }
                LabelTimeCalc.Content = timer.Elapsed.ToString();
                ListBoxResult.ItemsSource = tempList;
            }
            else
            {
                MessageBox.Show("Выберите файл и введите слово для поиска");
            }
        }

        private void ButtonSaveReport_Click(object sender, RoutedEventArgs e)
        {
            string TempReportFileName = "Report_" + DateTime.Now.ToString("dd_MM_yyyy_hhmmss");
            SaveFileDialog fd = new SaveFileDialog
            {
                FileName = TempReportFileName,
                DefaultExt = ".html",
                Filter = "HTML Reports|*.html"
            };
            bool? result = fd.ShowDialog();
            if (result == true)
            {
                string ReportFileName = fd.FileName;
                StringBuilder b = new StringBuilder();
                b.AppendLine("<html>");
                b.AppendLine("<head>");
                b.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>");
                b.AppendLine("<title>" + "Отчет: " + ReportFileName + "</title>");
                b.AppendLine("</head>");
                b.AppendLine("<body>");
                b.AppendLine("<h1>" + "Отчет: " + ReportFileName + "</h1>");
                b.AppendLine("<table border='1'>");
                b.AppendLine("<tr>");
                b.AppendLine("<td>Время чтения из файла</td>");
                b.AppendLine("<td>" + LabelTimer.Content + "</td>");
                b.AppendLine("</tr>");
                b.AppendLine("<tr>");
                b.AppendLine("<td>Количество уникальных слов в файле</td>");
                b.AppendLine("<td>" + LabelCountWords.Content + "</td>");
                b.AppendLine("</tr>");
                b.AppendLine("<tr>");
                b.AppendLine("<td>Слово для поиска</td>");
                b.AppendLine("<td>" + TextBoxCurrentWord.Text + "</td>");
                b.AppendLine("</tr>");
                b.AppendLine("<tr>");
                b.AppendLine("<td>Максимальное расстояние для нечеткого поиска</td>");
                b.AppendLine("<td>" + TextBoxMaxDistance.Text + "</td>");
                b.AppendLine("</tr>");
                b.AppendLine("<tr>");
                b.AppendLine("<td>Время четкого поиска</td>");
                b.AppendLine("<td>" + LabelTimeSearch.Content + "</td>");
                b.AppendLine("</tr>");
                b.AppendLine("<tr>");
                b.AppendLine("<td>Время нечеткого поиска</td>");
                b.AppendLine("<td>" + LabelTimeCalc.Content + "</td>");
                b.AppendLine("</tr>");
                b.AppendLine("<tr valign='top'>");
                b.AppendLine("<td>Результаты поиска</td>");
                b.AppendLine("<td>");
                b.AppendLine("<ul>");
                foreach (var x in ListBoxResult.Items)
                {
                    b.AppendLine("<li>" + x.ToString() + "</li>");
                }
                b.AppendLine("</ul>");
                b.AppendLine("</td>");
                b.AppendLine("</tr>");
                b.AppendLine("</table>");
                b.AppendLine("</body>");
                b.AppendLine("</html>");
                File.AppendAllText(ReportFileName, b.ToString());
                MessageBox.Show("Отчет сформирован. Файл: " + ReportFileName);
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}