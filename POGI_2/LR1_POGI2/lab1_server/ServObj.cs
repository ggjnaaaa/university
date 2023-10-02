using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab1_server
{
    class ServObj
    {
        TcpListener listener;
        List<ClObj> clients = new List<ClObj>();

        //добавляем пользователя в список подключенных пользователей
        internal void AddConnection(ClObj clientObject)
        {
            clients.Add(clientObject);
        }

        //отправляем сообщение всем пользователям, кроме отправителя
        internal void Message(/*string name, */string message, string id)
        {
            //отправка сообщения
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id)
                {
                    clients[i].stream.Write(data, 0, data.Length);
                }
            }
        }

        internal void MessageToName(string name, string message, string id, string userName)
        {
            byte[] data = Encoding.Unicode.GetBytes($"Личное сообщение от {userName}: {message}");
            string[] names = name.Split(',');
            foreach (string n in names)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].Name == n)
                    {
                        clients[i].stream.Write(data, 0, data.Length);
                    }
                }
            }
        }

        //удаление пользователя из списка подключенных пользователей
        internal void RemoveConnection(string id)
        {
            ClObj client = clients.FirstOrDefault(i => i.Id == id);
            if (client != null)
                clients.Remove(client);
        }

        public void Listen()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, 8888);
                listener.Start(); // запуск прослушивания входящих запросов
                Console.WriteLine("Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();

                    ClObj clientObject = new ClObj(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start(); // старт потока
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Disconnect();
            }
        }

        internal void Disconnect()
        {
            listener.Stop(); // отсанавливаем сервер

            //отключение всех клентов от сервера
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
        }

        internal void UpdateName(string name, string id)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id == id)
                {
                    clients[i].Name = name;
                }
            }
        }
    }
}
