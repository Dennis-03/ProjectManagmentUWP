using Gremlin.Net.Process.Traversal;
using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class TaskList : UserControl
    {
        public ZTask ZTask { get { return this.DataContext as ZTask; } }

        UserManager userManager = UserManager.GetUserManager();
        public TaskList()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ZTask != null)
            {
                var commentedDateTimeOffset = DateTime.Now.Subtract(ZTask.AssignedDate);

                if (commentedDateTimeOffset.Days >= 7)
                    DateTimeDifference.Text = $"{(int)commentedDateTimeOffset.TotalDays / 7}w ago";
                else if (commentedDateTimeOffset.Days >= 1)
                    DateTimeDifference.Text = $"{(int)commentedDateTimeOffset.TotalDays}d ago";
                else if (commentedDateTimeOffset.Hours > 1)
                    DateTimeDifference.Text = $"{(int)commentedDateTimeOffset.TotalHours}h ago";
                else
                    DateTimeDifference.Text = $"{(int)commentedDateTimeOffset.TotalMinutes}m ago";
            }
        }
    }
}
