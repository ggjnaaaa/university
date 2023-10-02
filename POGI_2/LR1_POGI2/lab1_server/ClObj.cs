using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab1_server
{
    class ClObj
    {
        internal string Id { get; private set; }
        internal string Name { get; set; }
        internal NetworkStream stream { get; set; }

        TcpClient client;
        ServObj server;

        string userName;
        string n = ""; //пустая строка на случай если нужно отправить сообщение от сервера

        public ClObj(TcpClient tcpClient, ServObj serverObject)
        {
            Id = Guid.NewGuid().ToString();
            Name = userName;
            client = tcpClient;
            server = serverObject;
            server.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                stream = client.GetStream();

                // Получаем имя пользователя
                userName = GetMessage();
                string message = $"Новый пользователь, поздоровайтесь с {userName}";
                // Рассылаем сообщение о входе в чат всем подключенным пользователям
                server.Message(message, /*n, */Id);
                Console.WriteLine(message);
                server.UpdateName(userName, Id);

                string name;

                //получение данных от пользователя
                while (true)
                {
                    name = GetMessage();
                    Thread.Sleep(5);

                    if (name == "Пользователь вышел.")
                    {
                        message = $"Пользователь {userName} покинул чат.";
                        server.Message(message, Id);
                        Console.WriteLine(message);
                        server.RemoveConnection(Id);
                        Close();
                        break;
                    }
                    else if (name == "Отправить всем.")
                    {
                        message = GetMessage();
                        message = string.Format($"{userName}: {message}");
                        Console.WriteLine(message);
                        server.Message(message, Id);
                    }
                    else
                    {
                        message = GetMessage();
                        string msg = string.Format($"{userName}: {name}: {message}");
                        Console.WriteLine(msg);
                        server.MessageToName(name, message, Id, userName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                server.RemoveConnection(Id);
                Close();
            }
        }

        //метод получения сообщения из потока
        internal string GetMessage()
        {
            byte[] data = new byte[512];
            StringBuilder getString = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                getString.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

            return getString.ToString();
        }

        internal void Close()
        {
            if (stream != null)
                stream.Close(); // закрываем поток
            if (client != null)
                client.Close(); // закрываем соединение
        }
    }
}
