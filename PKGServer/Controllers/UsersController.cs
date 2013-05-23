using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PKGServer.Models;
using System.Security.Cryptography;
using System.Text;
using PKGServer.Authorization;

namespace PKGServer.Controllers
{
    public class UsersController : ApiController
    {

        private db9002ae48e70f46e38530a1c6009ea6b3Entities db = new db9002ae48e70f46e38530a1c6009ea6b3Entities();

        // GET api/users
        public IEnumerable<User> Get()
        {
            return db.Users.ToList<User>();
        }

        // GET api/users/view/5
        [HttpGet]
        public User View(int id)
        {
            var user = (from m in db.Users
                        where m.Id == id
                        select m).First();
            return user;
        }

        // POST api/users
        [HttpPost]
        public void Create([FromBody]User value)
        {
            db.Users.Add(value);
            db.SaveChanges();
        }

        // PUT api/users/edit/5
        [HttpPut]
        public void Edit(int id, [FromBody]User value)
        {
        }

        // DELETE api/users/delete/5
        [HttpDelete]
        public void Delete(int id)
        {
            var user = (from m in db.Users
                        where m.Id == id
                        select m).First();
            db.Users.Remove(user);
            db.SaveChanges();
        }

        //POST api/users/login
        [HttpPost]
        public TokenString Login([FromBody] LoginInfo user)
        {
            var dbUser = (from m in db.Users
                        where m.Email == user.Email
                        select m).First();
            TokenString token = new TokenString();
            token.Token = "invalid";
            if (dbUser.Password == user.Password)
            {
                token.Token = BitConverter.ToString(AesConfig.EncryptStringToBytes_Aes(user.Email)).Replace("-", string.Empty);
                //token.Token= Convert.ToBase64String(AesConfig.EncryptStringToBytes_Aes(user.Email));
            }
            return token;
        }
    }
}
