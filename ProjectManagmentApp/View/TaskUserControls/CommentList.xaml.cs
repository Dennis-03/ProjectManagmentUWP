using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ProjectManagmentApp.View.TaskUserControls
{
    public sealed partial class CommentList : UserControl
    {
        UserManager userManager = UserManager.GetUserManager();
        ReactionManager reactionManager = ReactionManager.GetReactionManager();
        CommentManager commentManager = CommentManager.GetCommentManager();
        private long userId;
        private int NoOfLikes;
        public ObservableCollection<Comment> Replies;
        private List<string> _reactersName;

        public Comment Comment
        {
            get { return (Comment)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, Comment); }
        }

        public static readonly DependencyProperty CommentProperty = DependencyProperty.Register("Comment", typeof(Comment), typeof(CommentList), null);

        public CommentList()
        {
            this.InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            userId = userManager.GetUserId();
            var user = userManager.GetUser(Comment.UserId);
            UserNameTB.Text = user.UserName;

            _reactersName = new List<string>();
            Comment.Reaction.ForEach(like =>
            {
                if (like.ReactedById == userId)
                    _reactersName.Insert(0, "You");
                else
                    _reactersName.Add(userManager.GetUser(like.ReactedById).UserName);
            });
            ReactersName.Text = string.Join(", ", _reactersName);

            NoOfLikes = Comment.Reaction.Count();
            if (NoOfLikes != 0)
                LikeCountTB.Content = NoOfLikes.ToString();
            UserCommentTB.Text = Comment.CommentString;

            if (user.AvatarPath.Contains("Assets"))
            {
                Uri uri = new Uri(user.AvatarPath, UriKind.Absolute);
                CommenterAvatar.Source = new BitmapImage(uri);
            }
            else
            {
                try
                {
                    BitmapImage image = new BitmapImage();
                    var storageFile = await StorageFile.GetFileFromPathAsync(user.AvatarPath);
                    using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        await image.SetSourceAsync(stream);
                    }
                    CommenterAvatar.Source = image;
                }
                catch(Exception exp)
                {
                    Uri uri = new Uri("ms-appx:/Assets/Avatar/no_image.png", UriKind.Absolute);
                    CommenterAvatar.Source = new BitmapImage(uri);
                }
            }

            var buttonIcon = HttpUtility.HtmlDecode("&#xE006;");
            Comment.Reaction.ForEach(like =>
            {
                if (like.ReactedById == userId)
                    buttonIcon = HttpUtility.HtmlDecode("&#xE00B;");
            });
            LikeCommentBtn.Content = buttonIcon;
            Replies = new ObservableCollection<Comment>(Comment.Reply);
            ReplyList.ItemsSource = Replies;
        }

        public static string TimeDifference(DateTime commentedTime)
        {
            var dateTimeDifference = DateTime.UtcNow - commentedTime;

            if (dateTimeDifference.Days >= 30)
                return $"{(int)dateTimeDifference.TotalDays / 30}mon ago";
            else if (dateTimeDifference.Days >= 7)
                return $"{(int)dateTimeDifference.TotalDays / 7}w ago";
            else if (dateTimeDifference.Days >= 1)
                return $"{(int)dateTimeDifference.TotalDays}d ago";
            else if (dateTimeDifference.Hours >= 1)
                return $"{(int)dateTimeDifference.TotalHours}h ago";
            else if (dateTimeDifference.Minutes >= 1)
                return $"{(int)dateTimeDifference.TotalMinutes}m ago";
            else if (dateTimeDifference.Seconds >= 0)
                return $"{(int)dateTimeDifference.TotalSeconds}s ago";
            else
                return $"1s ago";
        }

        private void LikeComment_Click(object sender, RoutedEventArgs e)
        {
            bool status = reactionManager.AddReaction(userId, Comment.Id);
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
            if (!string.IsNullOrEmpty(CommentTB.Text))
            {
                Comment addComment = new Comment
                {
                    TaskID = Comment.TaskID,
                    CommentString = CommentTB.Text,
                    ParentId = Comment.Id,
                    commentedDateTime = DateTime.Now,
                    UserId = userId
                };
                Replies.Insert(0, addComment);
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
