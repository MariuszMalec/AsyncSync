using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncSync.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _sourceFiles = (@"Source");
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ClickAsync_Click(object sender, RoutedEventArgs e)
        {
            mylabel.Content = "Reading started ...";
            var stopwatch = Stopwatch.StartNew();
            await ProcesFilesAsync();
            stopwatch.Stop();
            mylabel.Content = $"Async Finished in {stopwatch.ElapsedMilliseconds} ms.";

        }
        private async Task ProcesFilesAsync()
        {
            string[] files = Directory.GetFiles(_sourceFiles);
            int count = 0;
            foreach (var file in files)
            {
                using (var reader = new StreamReader(file, Encoding.UTF8))
                {
                    mylabel.Content = $"Reading {file}";
                    var fileContent = await Task.Run(() => reader.ReadToEnd());
                    count++;
                }
            }
        }
        private void ClickSync_Click(object sender, RoutedEventArgs e)
        {
            mylabel.Content = "Reading started ...";
            var stopwatch = Stopwatch.StartNew();
            ProcesFilesSync();
            stopwatch.Stop();
            mylabel.Content = $"Sync Finished in {stopwatch.ElapsedMilliseconds} ms.";

        }
        private void ProcesFilesSync()
        {
            string[] files = Directory.GetFiles(_sourceFiles);
            int count = 0;
            foreach (var file in files)
            {
                using (var reader = new StreamReader(file, Encoding.UTF8))
                {
                    mylabel.Content = $"Reading {file}";
                    var fileContent = reader.ReadToEnd();
                    count++;
                }
            }
        }

        private async void ClickAsyncTask_Click(object sender, RoutedEventArgs e)
        {
            mylabel.Content = "Reading started ...";
            var stopwatch = Stopwatch.StartNew();
            var lenghtFiles = await ProcesFilesAsyncTask();
            stopwatch.Stop();
            mylabel.Content = $"Async Finished in {stopwatch.ElapsedMilliseconds} ms. Lenght file: {lenghtFiles}";
        }
        private async Task<int> ProcesFilesAsyncTask()
        {
            var totalLenght = 0;
            string[] files = Directory.GetFiles(_sourceFiles);
            int count = 0;
            List<Task> tasks = new List<Task>();
            foreach (var file in files)
            {
                var task = Task.Run(() =>
                {
                    using (var reader = new StreamReader(file, Encoding.UTF8))
                    {
                        var fileContent = reader.ReadToEnd();
                        totalLenght += fileContent.Length;
                        count++;
                     }
                });
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
            return totalLenght;
        }

        private async void ClickAsyncTaskDelay_Click(object sender, RoutedEventArgs e)
        {
            mylabel.Content = "Reading started ...";
            await Task.Delay(1500);
            mylabel.Content = "Reading stop";           
        }
    }
}
