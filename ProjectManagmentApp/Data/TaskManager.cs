using ProjectManagmentApp.Constants;
using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Data
{
    class TaskManager
    {
        private static readonly TaskManager _instance = new TaskManager();
        SQLite.Net.SQLiteConnection conn;

        private TaskManager()
        {
            string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<ZTask>();
            conn.CreateTable<Comment>();
            conn.CreateTable<Reaction>();
        }
        public static TaskManager GetTaskManager()
        {   
            return _instance;
        }

        CommentManager commentManager = CommentManager.GetCommentManager();
        ReactionManager reactionManager = ReactionManager.GetReactionManager();

        public void AddTask(string taskString,string description, PriorityEnum priority, long assignedTo, long assignedBy, DateTime assignedDate, DateTime dueDate,bool completed)
        {
            conn.Insert(new ZTask
            {
                Id=DateTime.Now.Ticks,
                TaskName = taskString,  
                Description = description,
                AssignedTo = assignedTo,
                AssignedBy = assignedBy,
                AssignedDate = assignedDate,
                DueDate = dueDate,
                Priority = priority,
                Completed = completed
            });
        }

        public void AddTask(ZTask zTask)
        {
            zTask.Id = DateTime.Now.Ticks;
            conn.Insert(zTask);
        }

        public List<ZTask> ListAllTasks()
        {
            return new List<ZTask>(conn.Table<ZTask>().Where(task=>task.Completed==false).OrderByDescending(task=>task.AssignedDate));
        }

        public ZTask GetZTask(long taskId)
        {
            ZTask zTask = conn.Table<ZTask>().FirstOrDefault(ztask => ztask.Id == taskId);
            zTask.Comment = commentManager.GetTaskComments(zTask.Id);
            zTask.Reaction = reactionManager.GetReaction(taskId);
            return zTask;
        }

        public void UpdateTask(ZTask updateTask)
        {
            conn.InsertOrReplace(updateTask);
        }

        public void DeleteTask(long taskId)
        {
            conn.Table<ZTask>().Delete(zTask => zTask.Id == taskId);
        }

        public List<ZTask> GetUserTasks(long userId)
        {
            return new List<ZTask>(conn.Table<ZTask>().Where(zTask=>zTask.AssignedTo==userId).OrderByDescending(task => task.AssignedDate));
        }

        public List<ZTask> GetUserCreatedTasks(long userId)
        {
            return new List<ZTask>(conn.Table<ZTask>().Where(zTask => zTask.AssignedBy == userId).OrderByDescending(task => task.AssignedDate));
        }
        public void MarkCompleted(long taskId)
        {
            ZTask zTask = GetZTask(taskId);
            zTask.Completed = true;
            UpdateTask(zTask);
        }
    }
}
