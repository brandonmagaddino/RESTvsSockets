using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_DataModels.Model
{
    [Serializable]
    public class Message
    {
        public DateTime TimeSent { get; set; }
        public string Msg { get; set; }
        public string Sender { get; set; }
    }
}
