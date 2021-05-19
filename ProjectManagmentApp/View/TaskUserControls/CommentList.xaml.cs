using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ProjectManagmentApp.View.TaskUserControls
{
    public sealed partial class CommentList : UserControl
    {
        public Comment CommentData { get; set; }
        UserManager userManager = UserManager.GetUserManager();
        ReactionManager reactionManager = ReactionManager.GetReactionManager();
        CommentManager commentManager = CommentManager.GetCommentManager();
        private long userId;
        private int NoOfLikes;
        private Comment _comment;
        public ObservableCollection<Comment> Replies;
        private List<string> _reactersName = new List<string>();


        public CommentList()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => { CommentData = DataContext as Comment; };
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {   
            _comment = CommentData;
            userId = userManager.GetUserId();
            var user = userManager.GetUser(_comment.UserId);
            UserNameTB.Text = user.UserName;

            _comment.Reaction.ForEach(like =>
            {
                if (like.ReactedById == userId)
                    _reactersName.Insert(0, "You");
                else
                    _reactersName.Add(userManager.GetUser(like.ReactedById).UserName);
            });
            ReactersName.Text = string.Join(", ", _reactersName);


            var commentedDateTimeOffset = DateTime.Now.Subtract(_comment.commentedDateTime);

            if (commentedDateTimeOffset.Days >= 1)
                CommentedDateTimeTB.Text = $"{(int)commentedDateTimeOffset.TotalDays}d ago";
            else if(commentedDateTimeOffset.Hours >=1 )
                CommentedDateTimeTB.Text = $"{(int)commentedDateTimeOffset.TotalHours}h ago";
            else
                CommentedDateTimeTB.Text = $"{(int)commentedDateTimeOffset.TotalMinutes}m ago";

            NoOfLikes = _comment.Reaction.Count();
            if (NoOfLikes != 0)
                LikeCountTB.Content = NoOfLikes.ToString();
            UserCommentTB.Text = _comment.CommentString;

            Replies = new ObservableCollection<Comment>(_comment.Reply);
            ReplyList.ItemsSource = Replies;

            Uri uri = new Uri(user.AvatarPath, UriKind.Absolute);
            CommenterAvatar.Source = new BitmapImage(uri);

            var buttonIcon = HttpUtility.HtmlDecode("&#xE006;");
            _comment.Reaction.ForEach(like=> {
                if (like.ReactedById == userId)
                    buttonIcon = HttpUtility.HtmlDecode("&#xE00B;");
            });
            LikeCommentBtn.Content = buttonIcon;
        }

        private void LikeComment_Click(object sender, RoutedEventArgs e)
        {
            bool status=reactionManager.AddReaction(userId, _comment.Id);
            if (status)
            {
                LikeCommentBtn.Content = HttpUtility.HtmlDecode("&#xE00B;");
                NoOfLikes += 1;
                LikeCountTB.Content = NoOfLikes.ToString();
                _reactersName.Insert(0, "You");
                ReactersName.Text = string.Join(", ", _reactersName);

            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(CommentTB.Text))
            {
                Comment addComment = new Comment
                {
                    TaskID = _comment.TaskID,
                    CommentString = CommentTB.Text,
                    ParentId = _comment.Id,
                    commentedDateTime = DateTime.Now,
                    UserId = userId
                };
                Replies.Insert(0,addComment);
                commentManager.AddComment(addComment);
                AddReplyContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void AddReplyHolder_Click(object sender, RoutedEventArgs e)
        {
            AddReplyContainer.Visibility = Visibility.Visible;
        }
    }
}
