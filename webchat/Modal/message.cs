using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webchat.Modal
{
    public class message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string fromUserid { get; set; }
        public string Message { get; set; }
        public string toUserId { get; set; }
    }
    public class messageRequest
    {
        public string fromUserId { get; set; }
        public string toUserId { get; set; }
    }
}
