using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shared_DataModels.Model;

namespace Client_InstantMessage.Managers
{
    class RESTMessageManager : IMessageManager
    {
        public RESTMessageManager()
        {
            new Task(async () => {
                while (true)
                {
                    UpdateMessages();

                    await Task.Delay(3000);
                }
            }).Start();
        }

        private List<Message> _messages = new List<Message>();
        public override List<Message> Messages
        {
            get
            {
                return _messages;
            }
        }
        public override string Sender { get; set; }

        public void UpdateMessages()
        {
            new Task(() => {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:56545/api/message");
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    MergeMessages(Newtonsoft.Json.JsonConvert.DeserializeObject<List<Message>>(result));
                }
            }).Start();
        }

        public override void SendMessage(string message)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:56545/api/message");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                Message sendingMessage = new Message
                {
                    Sender = Sender,
                    TimeSent = DateTime.Now,
                    Msg = message
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(sendingMessage);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                
                MergeMessages(Newtonsoft.Json.JsonConvert.DeserializeObject<List<Message>>(result));
            }
        }

        private void MergeMessages(List<Message> newMessages)
        {
            if(Messages.Count != newMessages.Count)
            {
                _messages = newMessages;
                UpdateAllMessages();
            }
        }
    }
}
