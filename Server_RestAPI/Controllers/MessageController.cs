using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared_DataModels.Model;

namespace Server_RestAPI.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private static List<Message> messages = new List<Message>();

        // GET api/values/5
        [HttpGet]
        public List<Message> Get()
        {
            return messages;
        }

        // POST api/values
        [HttpPost]
        public List<Message> Post([FromBody]Message message)
        {
            messages.Add(message);
            return messages;
        }
    }
}
