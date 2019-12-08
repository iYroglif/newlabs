using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace laba4_wpf
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
                TextBoxCountWords.Text = words.Count.ToString();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать файл");
            }
        }

        private void ButtonSearchWord_Click(object sender, RoutedEventArgs e)
        {
            string wordSearch = TextBoxSearchWord.Text.Trim();
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
                ListBoxResult.ItemsSource=tempList;
            }
            else
            {
                MessageBox.Show("Выберите файл и введите слово для поиска");
            }
        }
    }
}
