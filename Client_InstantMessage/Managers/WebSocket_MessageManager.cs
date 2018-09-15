using System;
using System.Collections.Generic;
using System.Text;
using Shared_DataModels.Model;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;

namespace Client_InstantMessage.Managers
{
    class WebSocket_MessageManager : IMessageManager
    {
        private Stream SocketStream;

        public WebSocket_MessageManager()
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();

                tcpclnt.Connect("localhost", 8001);
                // use the ipaddress as in the server program
                
                SocketStream = tcpclnt.GetStream();

                StartListening();
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void StartListening()
        {
            new Task(() => {
                while (true)
                {
                    byte[] bytes = new byte[1024];
                    int byteArrayLength = SocketStream.Read(bytes, 0, 1024);

                    var result = System.Text.Encoding.Default.GetString(bytes);

                    var newMessages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Message>>(result);

                    if (newMessages.Count != Messages.Count)
                    {
                        _messages = newMessages;
                        UpdateAllMessages();
                    }
                }
            }).Start();
        }

        public override string Sender { get; set; }

        private List<Message> _messages = new List<Message>();
        public override List<Message> Messages { get
            {
                return _messages;
            }
        }

        public override void SendMessage(string message)
        {
            Message sendingMessage = new Message
            {
                Sender = Sender,
                TimeSent = DateTime.Now,
                Msg = message
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(sendingMessage);

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(json);
            Console.WriteLine("Transmitting.....");

            SocketStream.Write(ba, 0, ba.Length);
        }
    }
}
