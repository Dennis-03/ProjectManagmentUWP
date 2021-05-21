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
        UserManager userManager = UserManager.GetUserManager();

        public ZTask ZTask
        {
            get { return (ZTask)GetValue(ZTaskProperty); }
            set { SetValue(ZTaskProperty, value); }
        }
        public static readonly DependencyProperty ZTaskProperty = DependencyProperty.Register("ZTask", typeof(ZTask), typeof(TaskList), null);

        public TaskList()
        {
            this.InitializeComponent();
        }

        public static string TimeDifference(DateTime assignedDate)
        {
            var dateTimeDifference = DateTime.UtcNow - assignedDate;

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
            else
                return $"{(int)dateTimeDifference.TotalSeconds}s ago";
        }
    }
}
