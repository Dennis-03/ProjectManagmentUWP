using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using ProjectManagmentApp.View.TaskUserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<ZTask> _inCompleteTaskList;
        private ObservableCollection<ZTask> _completedTaskList;
        private ObservableCollection<ZTask> _taskList;
        public long TaskId;

        public MyTasksView()
        {
            long userId = userManager.GetUserId();
            this.InitializeComponent();
            _taskList = new ObservableCollection<ZTask>(taskManager.GetUserTasks(userId));
            _inCompleteTaskList = new ObservableCollection<ZTask>(_taskList.Where(task => task.Completed==false));
            _completedTaskList = new ObservableCollection<ZTask>(_taskList.Where(task => task.Completed==true));
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

        private void InCompleteTaskList_ItemClick(object sender, ItemClickEventArgs e)
        {
            CompletedTaskList.SelectedItem = null;
            ZTask clickedItem = (ZTask)e.ClickedItem;
            ZTask zTask = taskManager.GetZTask(clickedItem.Id);
            TaskId = clickedItem.Id;
            TaskDetailsFrame.Navigate(typeof(TaskDetails), zTask);
        }

        private void CompletedTaskList_ItemClick(object sender, ItemClickEventArgs e)
        {
            InCompleteTaskList.SelectedItem = null;
            ZTask clickedItem = (ZTask)e.ClickedItem;
            ZTask zTask = taskManager.GetZTask(clickedItem.Id);
            TaskId = clickedItem.Id;
            TaskDetailsFrame.Navigate(typeof(TaskDetails), zTask);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TaskData.TaskCompleted += TaskData_TaskCompleted;
        }

        private void TaskData_TaskCompleted(long taskId)
        {
            ZTask zTask = _taskList.First(task => task.Id == taskId);
            _inCompleteTaskList.Remove(zTask);
            _completedTaskList.Add(zTask);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            TaskData.TaskCompleted -= TaskData_TaskCompleted;
        }
    }
}