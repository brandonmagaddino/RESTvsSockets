using Shared_DataModels.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client_InstantMessage.Managers
{
    public abstract class IMessageManager
    {
        public abstract String Sender { get; set; }
        public abstract List<Message> Messages { get; }
        
        public abstract void SendMessage(string message);


        public virtual void UpdateAllMessages()
        {
            Console.Clear();
            foreach (var message in Messages)
            {
                Console.WriteLine("{2} - {0}: {1}", message.Sender, message.Msg, message.TimeSent);
            }
        }
    }
}
