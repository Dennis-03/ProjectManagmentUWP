using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Data
{
    class CommentManager
    {
        private static readonly CommentManager _instance = new CommentManager();
        SQLite.Net.SQLiteConnection conn;
        private CommentManager()
        {
            string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<Comment>();
        }
        public static CommentManager GetCommentManager()
        {
            return _instance;
        }

        TaskManager taskManager = TaskManager.GetTaskManager();
        ReactionManager reactionManager=ReactionManager.GetReactionManager();

        public void AddComment(long taskId, string commentString,long userId)
        {
            Comment addComment = new Comment
            {
                CommentString = commentString,
                TaskID = taskId,
                UserId = userId,
                ParentId = null,
                commentedDateTime = DateTime.Now
            };
            ZTask zTask = taskManager.GetZTask(taskId);
            zTask.Comment.Add(addComment);
            taskManager.UpdateTask(zTask);
        }

        public void AddReply(Comment Reply)
        {
            ZTask zTask = taskManager.GetZTask(Reply.TaskID);
            Comment comment = zTask.Comment.Find(mycomment => mycomment.Id == Reply.Id);
            comment.Reply.Add(Reply);
            taskManager.UpdateTask(zTask);
        }

        public void AddComment(Comment comment)
        {
            comment.Id = DateTime.Now.Ticks;
            conn.Insert(comment);
        }

        public Comment GetComment(long commentId, long taskId)
        {
            Comment returnComment = conn.Table<Comment>().FirstOrDefault(comment => comment.Id == commentId);
            returnComment.Reaction = reactionManager.GetReaction(commentId);
            return returnComment;
        }
        public List<Comment> GetTaskComments(long taskId)
        {
            List<Comment> comments = new List<Comment>(conn.Table<Comment>().Where(comment => comment.TaskID == taskId && comment.ParentId == null).OrderByDescending(comment => comment.commentedDateTime));
            comments.ForEach(comment =>
            {
                comment.Reaction = reactionManager.GetReaction(comment.Id);
                comment.Reply = GetReplies(comment.Id);
            });
            return comments;
        }   

        public List<Comment> GetReplies(long commentId)
        {
            List<Comment> replies = new List<Comment>(conn.Table<Comment>().Where(reply => reply.ParentId == commentId).OrderBy(comment => comment.commentedDateTime));
            replies.ForEach(reply =>
            {
                reply.Reaction = reactionManager.GetReaction(reply.Id);
                reply.Reply = GetReplies(reply.Id);
            });
            return replies;
        }
    }
}
