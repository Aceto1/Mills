using Mills.Database;
using Mills.Database.Entities.User;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Mills.Server.Handler
{
    internal class UserHandler
    {
        private readonly DatabaseContext databaseContext;

        public UserHandler(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public bool Register(string username, string password)
        {
            var user = new User()
            {
                Username = username,
                Password = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)))
            };

            databaseContext.Users.Add(user);
            databaseContext.SaveChanges();

            return true;
        }

        public int Login(string username, string password)
        {
            var user = databaseContext.Users.FirstOrDefault(m => m.Username == username);

            if (user == null)
                return -1;

            var hashedPw = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));

            if (user.Password != hashedPw)
                return -1;

            return user.Id;
        }
    }
}
