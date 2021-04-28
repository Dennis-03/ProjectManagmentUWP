using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProjectManagmentApp.View
{

    public sealed partial class MyTasksView : Page
    {
        TaskManager taskManager = TaskManager.GetTaskManager();
        UserManager userManager = UserManager.GetUserManager();

        private List<ZTask> inCompleteTaskList;
        private List<ZTask> completedTaskList;
        public long TaskId;


        public MyTasksView()
        {
            long userId = userManager.GetUserId();
            this.InitializeComponent();
            inCompleteTaskList = new List<ZTask>(taskManager.GetUserTasks(userId).FindAll(task => task.Completed==false));
            completedTaskList = new List<ZTask>(taskManager.GetUserTasks(userId).FindAll(task => task.Completed==true));
            InCompleteDropLogo.Text = HttpUtility.HtmlDecode("&#xE019;");
            CompletedDropLogo.Text = HttpUtility.HtmlDecode("&#xE019;");
        }

        private void InCompleteTasksButton_Click(object sender, RoutedEventArgs e)
        {
            if (InCompleteTaskList.Visibility == Visibility.Visible)
            {
                InCompleteDropLogo.Text = HttpUtility.HtmlDecode("&#xE017;");
                InCompleteTaskList.Visibility = Visibility.Collapsed;
            }
            else
            {
                InCompleteDropLogo.Text = HttpUtility.HtmlDecode("&#xE019;");
                InCompleteTaskList.Visibility = Visibility.Visible;
            }
        }

        private void CompletedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            if (CompletedTaskList.Visibility == Visibility.Visible)
            {
                CompletedDropLogo.Text = HttpUtility.HtmlDecode("&#xE017;");
                CompletedTaskList.Visibility = Visibility.Collapsed;
            }
            else
            {
                CompletedDropLogo.Text = HttpUtility.HtmlDecode("&#xE019;");
                CompletedTaskList.Visibility = Visibility.Visible;
            }
        }

        private void TaskList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ZTask clickedItem = (ZTask)e.ClickedItem;
            ZTask zTask = taskManager.GetZTask(clickedItem.Id);
            TaskId = clickedItem.Id;
            TaskDetailsFrame.Navigate(typeof(TaskDetails), zTask);
            if (clickedItem.Completed == false)
            {
                MarkCompleted.Visibility = Visibility.Visible;
            }
            else
            {
                MarkCompleted.Visibility = Visibility.Collapsed;
            }
        }

        private void MarkCompleted_Click(object sender, RoutedEventArgs e)
        {
            taskManager.MarkCompleted(TaskId);
            Frame.Navigate(typeof(MyTasksView));
        }
    }
}
