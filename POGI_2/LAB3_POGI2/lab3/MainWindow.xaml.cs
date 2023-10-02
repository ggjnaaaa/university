using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer player = new MediaPlayer();
        DispatcherTimer Timer = new DispatcherTimer();

        Dictionary<string, string> list = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();

            Timer.Interval = new TimeSpan(0, 0, 0, 1);
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
            volume.Value = 10; player.Volume = 1;
        }

        #region Обработчики кнопок
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "mp3 |*.MP3";
            dlg.Multiselect = true;
            dlg.ShowDialog();

            foreach (string file in dlg.FileNames)
            {
                string[] s = file.ToString().Split('\\');   //массив из адреса файла эл-ты которого разделены знаком '\'
                int i = s.Length - 1;
                list.Add(s[i], file.ToString());   //добавление в словарь ключа в виде названия трека и значения в виде полного поти к этому треку
                playlist.Items.Add(s[i]);   //добавление названия трека в список песен
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            player.Play();
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            player.Pause();
            Timer.Tick -= new EventHandler(dispatcherTimer_Tick);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            time.Value = 0;

            thisTime.Content = "00:00:00";
            allTime.Content = "00:00:00";
        }
        #endregion

        #region Воспроизведение треков
        private void Playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (playlist.SelectedIndex != -1)
            {
                time.Value = 0;
                player.Open(new Uri(list[playlist.SelectedItem.ToString()], UriKind.Relative));
                player.Play();
                Timer.Start();
                player.MediaOpened += MediaOpened;
                player.MediaEnded += MediaEnded;
            }
            else
            {
                thisTime.Content = "00:00:00";
                allTime.Content = "00:00:00";
            }
        }

        private void MediaOpened(object sender, EventArgs e)
        {
            time.Maximum = player.NaturalDuration.TimeSpan.TotalMilliseconds;  //максимальное значение у ползунка
        }

        private void MediaEnded(object sender, EventArgs e)
        {
            if (rand.IsChecked == false & playlist.Items.Count >= 1) //обычное воспроизведение
            {
                if (playlist.SelectedIndex != (playlist.Items.Count - 1)) //если только что воспроизведённая песня не последняя
                {
                    player.Stop();
                    (playlist.SelectedIndex)++;
                    time.Value = 0;
                    player.Play();
                }
                else
                {
                    time.Value = 0;
                    player.Stop();
                    playlist.SelectedIndex = -1;
                    Timer.Stop();
                }
            }
            else if (rand.IsChecked == true & playlist.Items.Count >= 1) //рандомное воспроизведение
            {
                W:
                Random rand = new Random();
                int i = rand.Next(0, playlist.Items.Count);
                if (i != playlist.SelectedIndex)
                {
                    playlist.SelectedIndex = i;
                }
                else goto W;
            }
        }
        #endregion

        #region Всё, что связанно с ползунком времени
        private void Time_ValueChanged(object sender, DragCompletedEventArgs e)
        {
            Time();
        }

        private void Time_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Time();
        }

        void Time()
        {
            int SliderValue = (int)time.Value;

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            player.Position = ts;
        }
        #endregion

        private void Volume_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            player.Volume = (volume.Value / 10);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                time.Value = (int)player.Position.TotalMilliseconds;

                thisTime.Content = (string)player.Position.ToString("hh':'mm':'ss");
                allTime.Content = (string)player.NaturalDuration.TimeSpan.ToString("hh':'mm':'ss");
            }
            catch{}
        }
    }
}
