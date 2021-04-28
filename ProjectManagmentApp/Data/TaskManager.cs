using ProjectManagmentApp.Constants;
using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Data
{
    class TaskManager
    {
        private static readonly TaskManager _instance = new TaskManager();
        private TaskManager()
        {
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
                TaskName = taskString,
                Description=description,
                AssignedTo = assignedTo,
                AssignedBy = assignedBy,
                AssignedDate = assignedDate,
                DueDate = dueDate,
                Priority = priority,
                Completed=completed
            };

            TaskList.Add(addTask);
        }

        public void AddTask(ZTask zTask)
        { 
            TaskList.Add(zTask);
        }

        public List<ZTask> ListAllTasks()
        {
            return TaskList;
        }

        public ZTask GetZTask(long taskId)
        {
            return TaskList.Find(task => task.Id == taskId);
        }

        public void UpdateTask(ZTask updateTask)
        {
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
                }
            }
        }

        public void DeleteTask(long taskId)
        {
            TaskList.RemoveAll(task => task.Id == taskId);
        }

        public List<ZTask> GetUserTasks(long userId)
        {
            return TaskList.FindAll(task => task.AssignedTo == userId);
        }

        public List<ZTask> GetUserCreatedTasks(long userId)
        {
            return TaskList.FindAll(task => task.AssignedBy == userId);
        }
        public void MarkCompleted(long taskId)
        {
            ZTask zTask = GetZTask(taskId);
            zTask.Completed = true;
            UpdateTask(zTask);
        }
    }
}
