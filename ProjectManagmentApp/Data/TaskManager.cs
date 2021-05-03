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

        private List<ZTask> TaskList = new List<ZTask>();

        public void AddTask(string taskString,string description, PriorityEnum priority, long assignedTo, long assignedBy, DateTime assignedDate, DateTime dueDate,bool completed)
        {
            ZTask addTask = new ZTask
            {
                Id = DateTime.Now.Ticks,
                TaskName = taskString,
                Description=description,
                AssignedTo = assignedTo,
                AssignedBy = assignedBy,
                AssignedDate = assignedDate,
                DueDate = dueDate,
                Priority = priority,
                Completed=completed
            };
            //TaskList.Add(addTask);
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
            conn.Insert(zTask);
        }

        public List<ZTask> ListAllTasks()
        {
            //return TaskList;
            return new List<ZTask>(conn.Table<ZTask>());
        }

        public ZTask GetZTask(long taskId)
        {
            //return TaskList.Find(task => task.Id == taskId);
            return (ZTask)conn.Table<ZTask>().FirstOrDefault(zTask => zTask.Id == taskId);
        }

        public void UpdateTask(ZTask updateTask)
        {
            conn.Table<ZTask>().FirstOrDefault(zTask=>zTask.Id==updateTask.Id).Description=updateTask.Description;
            foreach (var task in TaskList)
            {
                if (task.Id == updateTask.Id)
                {
                    task.Priority = updateTask.Priority;
                    task.AssignedTo = updateTask.AssignedTo;
                    task.AssignedDate = updateTask.AssignedDate;
                    task.TaskName = updateTask.TaskName;
                    task.DueDate = updateTask.DueDate;
                    task.Comment = updateTask.Comment;
                    task.Completed = updateTask.Completed;
                    task.Reaction = updateTask.Reaction;
                }
            }
        }

        public void DeleteTask(long taskId)
        {
            TaskList.RemoveAll(task => task.Id == taskId);
            conn.Table<ZTask>().Delete(zTask => zTask.Id == taskId);
        }

        public List<ZTask> GetUserTasks(long userId)
        {
            //return TaskList.FindAll(task => task.AssignedTo == userId);
            return new List<ZTask>(conn.Table<ZTask>().Where(zTask=>zTask.AssignedTo==userId));
        }

        public List<ZTask> GetUserCreatedTasks(long userId)
        {
            //return TaskList.FindAll(task => task.AssignedBy == userId);
            return new List<ZTask>(conn.Table<ZTask>().Where(zTask => zTask.AssignedBy == userId));
        }
        public void MarkCompleted(long taskId)
        {
            ZTask zTask = GetZTask(taskId);
            zTask.Completed = true;
            UpdateTask(zTask);
        }
    }
}
