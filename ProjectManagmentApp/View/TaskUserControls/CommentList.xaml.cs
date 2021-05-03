using Gremlin.Net.Process.Traversal;
using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class CommentList : UserControl
    {
        public Comment CommentData { get; set; }
        UserManager userManager = UserManager.GetUserManager();
        ReactionManager reactionManager = ReactionManager.GetReactionManager();
        CommentManager commentManager = CommentManager.GetCommentManager();
        private long userId;
        private int NoOfLikes;
        private Comment _comment;

        public CommentList()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => { CommentData = DataContext as Comment; };
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {   
            _comment = CommentData;
            userId = userManager.GetUserId();
            UserNameTB.Text = userManager.GetUser(_comment.UserId).UserName;
            CommentedDateTimeTB.Text = _comment.commentedDateTime.ToShortTimeString();
            NoOfLikes = _comment.Reaction.Count();
            LikeCountTB.Text = NoOfLikes.ToString();
            UserCommentTB.Text = _comment.CommentString;

            var buttonIcon = HttpUtility.HtmlDecode("&#xE006;");
            _comment.Reaction.ForEach(like=> {
                if (like.ReactedById == userId)
                    buttonIcon = HttpUtility.HtmlDecode("&#xE00B;");
            });
            LikeCommentBtn.Content = buttonIcon;
        }

        private void LikeComment_Click(object sender, RoutedEventArgs e)
        {
            bool status=reactionManager.AddReactionToComment(userId, _comment.Id, _comment.TaskID);
            if (status)
            {
                LikeCommentBtn.Content = HttpUtility.HtmlDecode("&#xE00B;");
                NoOfLikes += 1;
                LikeCountTB.Text = NoOfLikes.ToString();
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Comment addComment = new Comment
            {
                TaskID = _comment.TaskID,
                CommentString = CommentTB.Text,
                ParentId = _comment.Id,
                commentedDateTime=DateTime.Now,
                UserId=userId
            };
            commentManager.AddComment(addComment);
            ButtonFlyout.Hide();    
        }
    }
}
