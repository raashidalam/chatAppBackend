using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using webchat.Modal;
namespace webchat
{
 //U
    public class ChatHub :Hub
    {
        // 
        // Context.ConnectionId;
        // 
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly chatcontext _context;
        public ChatHub(chatcontext context)
        {
            _context = context;

        }
        public void SendToAll(string name, string message)
        {

            Clients.All.SendAsync("sendToAll", name, message);
        }
        public void SendDirectMessage(message messageData)
        {
            string targetUserName = messageData.toUserId;
            var item = _context.users.FirstOrDefault(x => x.username == targetUserName);
            _context.messages.AddAsync(messageData);
            _context.SaveChanges();
           
            System.Console.WriteLine(targetUserName + Context.User.Identity.Name);
            Clients.Client(item.socketId).SendAsync("SendDM", messageData, targetUserName);
        }
        public void chat_list(string userid)
        {
           
           var item= _context.users.Select(u=> new{ u.username,u.onlineStatus}).ToList();
            var data = item.SingleOrDefault(u => u.username == userid);
            item.Remove(data);
            System.Console.WriteLine(item[0]);
            Clients.Client(Context.ConnectionId).SendAsync("chat-list", item);
        }
        public void addMessage(message messages)
        {

            _context.messages.AddAsync(messages);
            _context.SaveChanges();
        }
        public void Join(string userid)
        {
            //  var access = context.Request.Query["access_token"];
            //  var name =  Request.Cookies["userid"].to;
            //using (chatcontext db = new chatcontext())
            //{
            //    var item = db.users.FirstOrDefault(x => x.username == userid);
            //    if(item!= null)
            //    {

            //    }
            //}
            //using (SqlConnection connection = new SqlConnection("data source=GGKU5SER2;initial catalog=chatApp;user id=sa;password=Welcome@1234;"))
            //{
            //    connection.Open();
            //    SqlCommand cmd = new SqlCommand("update chatApp set socketId ='" + Context.ConnectionId + "'  where username ='" + userid + "'", connection);

            //    int result = cmd.ExecuteNonQuery();


            //}
            System.Console.WriteLine("VV" + " " + userid + " " + Context.ConnectionId);
            //  var connection = _context.users.Find(userid);
            var item = _context.users.FirstOrDefault(x => x.username == userid);
            System.Console.WriteLine(item);
            item.socketId = Context.ConnectionId;
            item.onlineStatus = "Y";
           _context.SaveChanges();
            var data = _context.users.Where(user => user.username == userid).Select(user => new { user.username, user.onlineStatus });

            Clients.All.SendAsync(
                "Joined",
                data
                );

        }
      
   public void logout(string username)
    {
        
            var item = _context.users.FirstOrDefault(x => x.username == username);
            item.onlineStatus = "N";
            _context.SaveChanges();
            var data = _context.users.Where(user => user.username == username).Select(user => new { user.username, user.onlineStatus });
            Clients.All.SendAsync("logout", data);
    }
    }

}

  

