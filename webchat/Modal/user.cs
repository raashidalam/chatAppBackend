using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webchat.Modal
{
    public class user
    {
     
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string onlineStatus { get; set; }
        public string socketId { get; set; }
    }
    public class userRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class userResponse
    {
      public string message { get; set; }
      public string username { get; set; }
    }
}
