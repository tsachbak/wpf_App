using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class UsersMgr
    {
        public static UsersMgr instance;
        private List<User> users;
        private DataBaseHandler dbHandler;

        private UsersMgr()
        {
            dbHandler = new DataBaseHandler();
            users = dbHandler.GetUsersFromDB() == null ? new List<User>() : dbHandler.GetUsersFromDB();
        }

        public static UsersMgr GetInstance()
        {
            if (instance == null)
            {
                instance = new UsersMgr();
            }
            return instance;
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public bool RemoveUser(User user)
        {
            return users.Remove(user);
        }

        public int GetUsersAmount()
        {
            return users.Count;
        }

        public List<User> GetUsers()
        {
            return users;
        }

        public User? GetUserByMail(string mail)
        {
            foreach (User user in users)
            {
                if (user.Mail.Equals(mail))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
