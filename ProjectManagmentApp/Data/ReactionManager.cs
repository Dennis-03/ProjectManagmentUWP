using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Data
{
    class ReactionManager
    {
        private static readonly ReactionManager _instance = new ReactionManager();
        SQLite.Net.SQLiteConnection conn;
        private ReactionManager()
        {
            string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<Reaction>();
        }
        public static ReactionManager GetReactionManager()
        {
            return _instance;
        }
        TaskManager taskManager = TaskManager.GetTaskManager();
        CommentManager commentManager = CommentManager.GetCommentManager();

        public bool AddReaction(long reactedById, long reactedToId)
        {            
            var status = conn.Table<Reaction>().Any(task => task.ReactedById == reactedById && task.ReactedToId == reactedToId);
            if (status == true)
            {
                return false;
            }
            conn.Insert(new Reaction
            {
                Id = DateTime.Now.Ticks,
                ReactedById = reactedById,
                ReactedToId = reactedToId
            });
            return true;
        }

        public List<Reaction> GetReaction(long reactedToId)
        {
            return new List<Reaction>(conn.Table<Reaction>().Where(reaction=>reaction.ReactedToId==reactedToId));
        }
    }
}
