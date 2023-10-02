using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab1_server
{
    class Program
    {
        static ServObj server;
        static Thread listenerThread;

        static void Main(string[] args)
        {
            try
            {
                server = new ServObj();
                listenerThread = new Thread(new ThreadStart(server.Listen));
                listenerThread.Start(); // старт потока
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                server.Disconnect();
            }
        }
    }
}
