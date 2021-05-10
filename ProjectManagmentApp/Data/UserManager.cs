using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Data
{
    class UserManager
    {
        private static readonly UserManager _instance = new UserManager();
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        SQLite.Net.SQLiteConnection conn;

        private UserManager()
        {
            string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<User>();
        }
        public static UserManager GetUserManager()
        {
            return _instance;
        }

        public bool AddUser(string userName, string password,string avatar)
        {
            User existingUser = conn.Table<User>().FirstOrDefault(user => user.UserName == userName);
            if (existingUser != null)
            {
                return false;
            }
            conn.Insert(new User
            {
                Password = password,
                UserName = userName,
                AvatarPath = avatar,
                Id = DateTime.Now.Ticks
            });
            return true;
        }

        public User VerifyUser(string userName, string password)
        {
            User existingUser = conn.Table<User>().FirstOrDefault(user => user.UserName == userName);
            if (existingUser == null||existingUser.Password != password )
            {
                return null;
            }
            return existingUser;
        }

        public User GetUser(long Id)
        {
            return conn.Table<User>().FirstOrDefault(user=>user.Id==Id);
        }

        public List<User> GetAllUsers()
        {
            return new List<User>(conn.Table<User>());
        }

        public long GetUserId()
        {
            return (long)localSettings.Values["Id"];
        }
        public void SetUserId(long Id)
        {
            localSettings.Values["Id"] = Id;
        }
        public void Logout()
        {
            localSettings.Values["Id"] =Convert.ToInt64(0);
        }
    }
}
