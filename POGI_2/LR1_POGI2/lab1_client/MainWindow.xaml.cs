using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace lab1_client
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

        TcpClient client;
        NetworkStream stream;
        Thread listenThread;

        int port = 8888;
        string address = "127.0.0.1";

        string usName;

        public void Connect_Click(object sender, RoutedEventArgs e)
        {
            usName = userName.Text;
            if (usName.Length >= 2)
            {
                client = new TcpClient();
                try
                {
                    client.Connect(address, port); // подключение клиента
                    stream = client.GetStream(); // получаем поток отправки и получения данных

                    if (client.Connected) // если подключение к серверу успешно
                    {
                        //блокировка и разблокировка кнопок и полей для ввода сообщения и имени пользователя
                        msg.IsEnabled = true;
                        send.IsEnabled = true;
                        disconnect.IsEnabled = true;

                        userName.IsEnabled = false;
                        connect.IsEnabled = false;

                        // Отправляем имя
                        byte[] data = Encoding.Unicode.GetBytes(usName);
                        stream.Write(data, 0, data.Length);

                        // Запускаем новый поток для получения данных
                        listenThread = new Thread(new ThreadStart(listen));
                        listenThread.Start(); // запускаем поток
                        clientLog.Items.Add("Привет, " + usName);
                    }
                    else
                    {
                        clientLog.Items.Add("Отсутствует подключение к серверу!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Disconnect();
                }
            }
            else clientLog.Items.Add("Введите имя пользователя не короче двух символов!");
        }

        void listen()
        {
            try
            {
                // цикл ожидания сообщений
                while (true)
                {
                    byte[] data = new byte[2048];
                    StringBuilder getString = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        getString.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = getString.ToString();
                    Dispatcher.BeginInvoke(new Action(() => clientLog.Items.Add(message)));
                }
            }
            catch
            {
                // вывести сообщение об ошибке
                clientLog.Items.Add("Подключение прервано!");
            }
            finally
            {
                // закрыть канал связи и завершить работу клиента
                Disconnect();
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            //отправка имён получателей
            string address = name.Text;
            if (address.Length > 2)
            {
                byte[] dat = Encoding.Unicode.GetBytes(address);
                stream.Write(dat, 0, dat.Length);
            }
            else
            {
                byte[] dat = Encoding.Unicode.GetBytes("Отправить всем.");
                stream.Write(dat, 0, dat.Length);
            }

            Thread.Sleep(5);   //чтобы сервер не считывал поток одной строкой, состоящей из адреса и сообщения

            //отправка сообщения
            string message = msg.Text;
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);

            //вывод сообщения у клиента
            if (address.Length > 2)
            {
                clientLog.Items.Add($"Личное сообщение пользователю {address}: {message}");
            }
            else
            {
                clientLog.Items.Add($"Вы: {message}");
            }
            msg.Clear();
            name.Clear();
        }


        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            //отправка сообщения, которое на сервере вызовет удаление клиента из списка клиентов
            //и отправку всем пользователям сообщения о том, что пользователь покинул чат
            byte[] data = Encoding.Unicode.GetBytes("Пользователь вышел.");
            stream.Write(data, 0, data.Length);

            listenThread.Abort();

            Disconnect();
        }

        private void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }
    }
}
