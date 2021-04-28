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
        public int NoOfLikes = 0;
        public int NoOfComments = 0;
        private ZTask ztask;
        private ObservableCollection<Comment> comments;
        private long userId;

        TaskManager taskManager = TaskManager.GetTaskManager();
        CommentManager commentManager = CommentManager.GetCommentManager();
        ReactionManager reactionManager = ReactionManager.GetReactionManager();
        UserManager userManager = UserManager.GetUserManager();

        public List<Comment> Comments
        {
            get { return (List<Comment>)GetValue(CommentsProperty); }
            set { SetValue(CommentsProperty, value); }
        }
        public static readonly DependencyProperty CommentsProperty = DependencyProperty.Register("Comments", typeof(Comment), typeof(TaskData), null);

        public ZTask zTask
        {
            get { return (ZTask)GetValue(zTaskProperty); }
            set { SetValue(zTaskProperty, value); }
        }
        public static readonly DependencyProperty zTaskProperty = DependencyProperty.Register("zTask", typeof(ZTask), typeof(TaskData), null);

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
            userId = userManager.GetUserId();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ztask = zTask;
            comments = new ObservableCollection<Comment>(Comments);
            CommentsList.ItemsSource = comments;
            NoOfLikes = ztask.Reaction.Count;
            NoOfLikesTB.Text = NoOfLikes.ToString();
            NoOfComments = comments.Count;
            NoOfCommentsTB.Text = NoOfComments.ToString();
            var buttonIcon = HttpUtility.HtmlDecode("&#xE006;");
            ztask.Reaction.ForEach(like => {
                if (like.ReactedById == userId)
                {
                    buttonIcon = HttpUtility.HtmlDecode("&#xE00B;");
                }
            });
            LikeReaction.Content = buttonIcon;
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
                TaskID = ztask.Id,
                UserId = userId,
                commentedDateTime=DateTime.Now
            };
            commentManager.AddComment(comment);
            comments.Add(comment);
            CommentBox.Visibility = Visibility.Collapsed;
            AddComment.Text = "";
            NoOfComments += 1;
            NoOfCommentsTB.Text = NoOfComments.ToString();
        }

        private void LikeReaction_Click(object sender, RoutedEventArgs e)
        {
            LikeReaction.Content = HttpUtility.HtmlDecode("&#xE00B;");
            bool status = reactionManager.AddReactionToTask(userId, ztask.Id);
            if (status) {
                NoOfLikes += 1;
                NoOfLikesTB.Text = NoOfLikes.ToString();
            }
        }

        private void CloseTask_Click(object sender, RoutedEventArgs e)
        {
            TaskDataContent.Visibility = Visibility.Collapsed;
        }
    }
}
