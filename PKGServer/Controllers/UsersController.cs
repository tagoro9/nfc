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
using System.Web;

namespace PKGServer.Controllers
{
    public class UsersController : ApiController
    {

        private db9002ae48e70f46e38530a1c6009ea6b3Entities1 db = new db9002ae48e70f46e38530a1c6009ea6b3Entities1();

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
        public string Create([FromBody]User value) 
        {
            value.Confirmed = 0;
            value.Password = PasswordHash.CreateHash(value.Password);
            db.Users.Add(value);
            string id = BitConverter.ToString(AesConfig.EncryptStringToBytes_Aes(value.Email)).Replace("-", string.Empty);
            string link = "https://pkg.apphb.com/#confirm=" + id;
            try
            {
                Mailer.Mailer.SendMail(value.Email, "PKG Registration", "You have been successfully registered to this PKG, in order to start using your key, you need to activate your account first. To do so click this [link](" + link + ")");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }                
            db.SaveChanges();
            return "User successfully created";
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
            if (PasswordHash.ValidatePassword(user.Password,dbUser.Password) && dbUser.Confirmed == 1)
            {
                token.Token = BitConverter.ToString(AesConfig.EncryptStringToBytes_Aes(user.Email)).Replace("-", string.Empty);
                //token.Token= Convert.ToBase64String(AesConfig.EncryptStringToBytes_Aes(user.Email));
            }
            return token;
        }

        //Get api/users/confirm?id=whatever
        [HttpGet]
        public string Confirm()
        {
            string query = Request.RequestUri.ParseQueryString().Get("id");
            string email = AesConfig.DecryptStringFromBytes_Aes(AccessTokenValidator.StringToByteArray(query));
            var dbUser = (from m in db.Users
                          where m.Email == email
                          select m).FirstOrDefault();
            dbUser.Confirmed = 1;
            db.SaveChanges();
            return "You have successfully activated yout account";
        }
    }
}
