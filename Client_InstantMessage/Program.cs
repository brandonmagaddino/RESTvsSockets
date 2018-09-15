using System;
using Shared_DataModels.Model;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Client_InstantMessage.Managers;

namespace Client_InstantMessage
{
    class Program
    {
        public static IMessageManager MessageManager = new WebSocket_MessageManager();

        static void Main(string[] args)
        {
            Console.Write("UserName: ");
            var username = Console.ReadLine();
            MessageManager.Sender = username;

            while (true)
            {
                var message = Console.ReadLine();
                MessageManager.SendMessage(message);
                MessageManager.UpdateAllMessages();
            }
        }
    }
}
