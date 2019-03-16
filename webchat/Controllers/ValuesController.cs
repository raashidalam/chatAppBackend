using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webchat.Modal;

namespace webchat.Controllers
{
    [Route("api/ChatingServer")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly chatcontext _context;
        public ValuesController(chatcontext context)
        {
            _context = context;
        }
     
        [HttpGet]
        public IQueryable<user> GetAll()
        {
            return _context.users;
        }
        
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody]user item)
        {
            Console.WriteLine(item);
              item.socketId = null;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            _context.users.Add(item);
           
             _context.SaveChanges();

            return StatusCode((int)System.Net.HttpStatusCode.Created, item.username);
   
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] userRequest userData)
        {
            userResponse usersRespo = new userResponse();
            try
            {
                string userName = userData.username;
                string password = userData.password;
                //  var item = _context.users.FirstOrDefault(data => (data.username == userName && data.password == password));
                var data = _context.users.Where(user => user.username == userName && user.password == password).Select(user => user.username);
                if (_context.users.Where(user => user.username == userName && user.password == password).Select(user => user.username).Any())
                {
                    System.Console.WriteLine("data" + data);
                    usersRespo.username = userName;
                    usersRespo.message = "successfully logged In";
                    return StatusCode((int)HttpStatusCode.OK, usersRespo);
                }
               
                usersRespo.username = null;
                usersRespo.message = "Incorrect username and password";
                return StatusCode((int)HttpStatusCode.Unauthorized, usersRespo);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

      [HttpPost]
      [Route("getMessage")]
      public IActionResult Getmaeesage([FromBody] messageRequest messageData)
        {
            string fromUseId = messageData.fromUserId;
            string toUserId = messageData.toUserId;
         //   var item = (from data in _context.messages where (data.fromUserid == fromUseId && data.toUserId == toUserId) || (data.fromUserid == toUserId && data.toUserId == fromUseId) select data.Message);
            //    item.OrderByDescending(Message);
            var item = _context.messages.Where
                (data => (data.fromUserid == fromUseId && data.toUserId == toUserId) || (data.fromUserid == toUserId && data.toUserId == fromUseId)).
                OrderBy(data => data.id).
                Select(data =>new { data.fromUserid, data.Message, data.toUserId }).ToList();
     
            return StatusCode((int)System.Net.HttpStatusCode.OK, item);
        }
       
        
        private bool UsersTableExists(string username)
        {
            return _context.users.Count(e => e.username == username) > 0;
        }

      
    }
   
}
