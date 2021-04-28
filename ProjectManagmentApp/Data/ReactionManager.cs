using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Data
{
    class ReactionManager
    {
        private static readonly ReactionManager _instance = new ReactionManager();
        private ReactionManager()
        {
        }
        public static ReactionManager GetReactionManager()
        {
            return _instance;
        }
        TaskManager taskManager = TaskManager.GetTaskManager();
        CommentManager commentManager = CommentManager.GetCommentManager();

        public bool AddReactionToTask(long reactedById, long taskId)
        {
            ZTask task = taskManager.GetZTask(taskId);
            foreach (var reaction in task.Reaction)
            {
                if (reaction.ReactedById == reactedById)
                {
                    return false;
                }
            }
            Reaction newReaction = new Reaction
            {
                ReactedById = reactedById,
                ReactedToId = taskId
            };
            task.Reaction.Add(newReaction);
            return true;
        }

        public bool AddReactionToComment(long reactedById, long commentId, long taskId)
        {
            Comment myComment = new Comment();
            myComment = commentManager.GetComment(commentId, taskId);
            foreach (var reaction in myComment.Reaction)
            {
                if (reaction.ReactedById == reactedById)
                {
                    return false;
                }
            }
            Reaction newReaction = new Reaction
            {
                ReactedById = reactedById,
                ReactedToId = taskId
            };
            myComment.Reaction.Add(newReaction);
            return true;
        }
    }
}
