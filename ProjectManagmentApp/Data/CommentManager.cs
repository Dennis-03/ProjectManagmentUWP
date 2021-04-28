using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Data
{
    class CommentManager
    {
        private static readonly CommentManager _instance = new CommentManager();
        private CommentManager()
        {
        }
        public static CommentManager GetCommentManager()
        {
            return _instance;
        }

        TaskManager taskManager = TaskManager.GetTaskManager();

        public void AddComment(long taskId, string commentString,long userId)
        {
            Comment addComment = new Comment
            {
                CommentString = commentString,
                TaskID = taskId,
                UserId=userId,
                commentedDateTime = DateTime.Now
            };
            ZTask zTask = taskManager.GetZTask(taskId);
            zTask.Comment.Add(addComment);
            taskManager.UpdateTask(zTask);
        }

        public void AddComment(Comment comment)
        {
            ZTask zTask = taskManager.GetZTask(comment.TaskID);
            zTask.Comment.Add(comment);
            taskManager.UpdateTask(zTask);
        }

        public Comment GetComment(long commentId, long taskId)
        {
            ZTask task = taskManager.GetZTask(taskId);
            return task.Comment.Find(comment => comment.Id == commentId);
        }
    }
}
