using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ProjectManagmentApp.View.TaskUserControls
{
    public sealed partial class TaskData : UserControl
    {
        private int _noOfLikes = 0;
        private int _noOfComments = 0;
        private ZTask _ztask;
        private long _userId;

        TaskManager taskManager = TaskManager.GetTaskManager();
        CommentManager commentManager = CommentManager.GetCommentManager();
        ReactionManager reactionManager = ReactionManager.GetReactionManager();
        UserManager userManager = UserManager.GetUserManager();

        public ObservableCollection<Comment> Comments
        {
            get { return (ObservableCollection<Comment>)GetValue(CommentsProperty); }
            set { SetValue(CommentsProperty, value); }
        }
        public static readonly DependencyProperty CommentsProperty = DependencyProperty.Register("Comments", typeof(ObservableCollection<Comment>), typeof(TaskData), null);

        public ZTask ZTask
        {
            get { return (ZTask)GetValue(ZTaskProperty); }
            set { SetValue(ZTaskProperty, value); }
        }
        public static readonly DependencyProperty ZTaskProperty = DependencyProperty.Register("ZTask", typeof(ZTask), typeof(TaskData), null);

        public string AssignedTo
        {
            get { return (string)GetValue(AssignedToProperty); }
            set { SetValue(AssignedToProperty, value); }
        }
        public static readonly DependencyProperty AssignedToProperty = DependencyProperty.Register("AssignedTo", typeof(string), typeof(TaskData), null);

        public string AssignedBy
        {
            get { return (string)GetValue(AssignedByProperty); }
            set { SetValue(AssignedByProperty, value); }
        }
        public static readonly DependencyProperty AssignedByProperty = DependencyProperty.Register("AssignedBy", typeof(string), typeof(TaskData), null);

        public SolidColorBrush PriorityColor
        {
            get { return (SolidColorBrush)GetValue(PriorityColorProperty); }
            set { SetValue(PriorityColorProperty, value); }
        }
        public static readonly DependencyProperty PriorityColorProperty = DependencyProperty.Register("PriorityColor", typeof(SolidColorBrush), typeof(TaskData), null);

        public TaskData()
        {
            this.InitializeComponent();
            _userId = userManager.GetUserId();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _ztask = ZTask;
            _noOfLikes = _ztask.Reaction.Count;
            NoOfLikesTB.Text = _noOfLikes.ToString();
            _noOfComments = Comments.Count;
            NoOfCommentsTB.Text = _noOfComments.ToString();
            var buttonIcon = HttpUtility.HtmlDecode("&#xE006;");
            _ztask.Reaction.ForEach(like => {
                if (like.ReactedById == _userId)
                {
                    buttonIcon = HttpUtility.HtmlDecode("&#xE00B;");
                }
            });
            LikeReaction.Content = buttonIcon;
            if (_ztask.Completed==false&&_ztask.AssignedTo==_userId)
            {
                MarkCompleted.Visibility = Visibility.Visible;
            }
            else
            {
                MarkCompleted.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowComment_Click(object sender, RoutedEventArgs e)
        {
            CommentBox.Visibility = Visibility.Visible;
        }

        private void SendClick_Click(object sender, RoutedEventArgs e)
        {
            Comment comment = new Comment
            {
                CommentString = AddComment.Text,
                TaskID = _ztask.Id,
                UserId = _userId,
                commentedDateTime=DateTime.Now
            };
            commentManager.AddComment(comment);
            Comments.Add(comment);
            CommentBox.Visibility = Visibility.Collapsed;
            AddComment.Text = "";
            _noOfComments += 1;
            NoOfCommentsTB.Text = _noOfComments.ToString();
        }

        private void LikeReaction_Click(object sender, RoutedEventArgs e)
        {
            LikeReaction.Content = HttpUtility.HtmlDecode("&#xE00B;");
            bool status = reactionManager.AddReactionToTask(_userId, _ztask.Id);
            if (status) {
                _noOfLikes += 1;
                NoOfLikesTB.Text = _noOfLikes.ToString();
            }
        }

        private void CloseTask_Click(object sender, RoutedEventArgs e)
        {
            TaskDataContent.Visibility = Visibility.Collapsed;
        }

        private void MarkCompleted_Click(object sender, RoutedEventArgs e)
        {
            _ztask.Completed = true;
            taskManager.MarkCompleted(_ztask.Id);
            SetValue(ZTaskProperty, _ztask);
            MarkCompleted.Visibility = Visibility.Collapsed;
        }
    }
}
