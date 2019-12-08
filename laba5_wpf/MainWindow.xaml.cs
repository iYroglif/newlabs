using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using laba5_lib;

namespace laba5_wpf
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

        readonly List<string> words = new List<string>();

        private void ButtonLoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog
            {
                Filter = "Текстовые Файлы|*.txt"
            };
            Nullable<bool> result = fd.ShowDialog();
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
                this.LabelTimer.Content = timer.Elapsed.ToString();
                this.TextBoxCountWords.Text = words.Count.ToString();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать файл");
            }
        }

        private void ButtonCalcDistance_Click(object sender, RoutedEventArgs e)
        {
            string currentWord = this.TextBoxCurrentWord.Text.Trim();
            if (!string.IsNullOrWhiteSpace(currentWord) && words.Count > 0)
            {
                if (!int.TryParse(this.TextBoxMaxDistance.Text.Trim(), out int maxDistance))
                {
                    MessageBox.Show("Введите максимальное расстояние");
                    return;
                }
                if (maxDistance < 1 || maxDistance > 5)
                {
                    MessageBox.Show("Максимальное расстояние должно быть в диапазоне от 1 до 5");
                    return;
                }
                List<string> tempList = new List<string>();
                Stopwatch timer = new Stopwatch();
                timer.Start();
                foreach (string str in words)
                {
                    int distance = LevenshteinDistance.Distance(str, currentWord);
                    if (distance <= maxDistance)
                    {
                        tempList.Add(str + " (расстояние: " + distance + ")");
                    }
                }
                timer.Stop();
                this.LabelTimeCalc.Content = timer.Elapsed.ToString();
                this.ListBoxResult.ItemsSource = tempList;
            }
            else
            {
                MessageBox.Show("Выберите файл и введите слово для вычисления расстояния");
            }
        }
    }
}