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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace ProjectManagmentApp.View
{

    public sealed partial class MyTasksView : Page
    {
        TaskManager taskManager = TaskManager.GetTaskManager();
        UserManager userManager = UserManager.GetUserManager();

        private ObservableCollection<ZTask> _inCompleteTaskList;
        private ObservableCollection<ZTask> _completedTaskList;
        private List<ZTask> _taskList;
        public long TaskId;

        public MyTasksView()
        {
            this.InitializeComponent();
            _taskList = new List<ZTask>(taskManager.GetUserTasks(userManager.GetUserId()));
            _inCompleteTaskList = new ObservableCollection<ZTask>(_taskList.Where(task => task.Completed == false));
            _completedTaskList = new ObservableCollection<ZTask>(_taskList.Where(task => task.Completed == true));
            InCompleteDropLogo.Text = HttpUtility.HtmlDecode("&#xE019;");
            CompletedDropLogo.Text = HttpUtility.HtmlDecode("&#xE019;");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SelectNextAvailableTask();
            TaskData.TaskCompleted += TaskData_TaskCompleted;
            TaskData.SelectNextZtask += TaskData_SelectNextZtask;
            TaskData.DeselectItem += TaskData_DeselectItem;
            if (Window.Current.Bounds.Width < 900)
            {
                TaskListContainer.Visibility = Visibility.Visible;
                TaskDetailsSV.Visibility = Visibility.Collapsed;
            }
        }

        private void TaskData_DeselectItem()
        {
            InCompleteTaskList.SelectedItem = null;
            CompletedTaskList.SelectedItem = null;
            if (Window.Current.Bounds.Width < 900)
            {
                TaskListContainer.Visibility = Visibility.Visible;
                TaskDetailsSV.Visibility = Visibility.Collapsed;
            }
        }

        private void TaskData_SelectNextZtask()
        {
            SelectNextAvailableTask();
        }

        public void SelectNextAvailableTask()
        {
            ZTask selectedItem;
            if (_inCompleteTaskList.Count != 0)
            {
                InCompleteTaskList.SelectedIndex = 0;
                selectedItem = (ZTask)InCompleteTaskList.SelectedItem;
            }
            else
            {
                CompletedTaskList.SelectedIndex = 0;
                selectedItem = (ZTask)CompletedTaskList.SelectedItem;
            }
            if (selectedItem != null)
            {
                ZTask zTask = taskManager.GetZTask(selectedItem.Id);
                TaskDetailsFrame.Navigate(typeof(TaskDetails), zTask);
            }
        }

        private void TaskData_TaskCompleted(long taskId)
        {
            ZTask zTask = _taskList.First(task => task.Id == taskId);
            _inCompleteTaskList.Remove(zTask);
            _completedTaskList.Add(zTask);
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
            DisplayTask(clickedItem);
        }

        private void CompletedTaskList_ItemClick(object sender, ItemClickEventArgs e)
        {
            InCompleteTaskList.SelectedItem = null;
            ZTask clickedItem = (ZTask)e.ClickedItem;
            DisplayTask(clickedItem);
        }

        private void DisplayTask(ZTask zTask)
        {
            ZTask myZTask = taskManager.GetZTask(zTask.Id);
            TaskId = zTask.Id;
            TaskDetailsFrame.Navigate(typeof(TaskDetails), zTask);
            if (Window.Current.Bounds.Width < 900)
            {
                TaskListContainer.Visibility = Visibility.Collapsed;
            }
            TaskDetailsSV.Visibility = Visibility.Visible;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            TaskData.TaskCompleted -= TaskData_TaskCompleted;
            TaskData.SelectNextZtask -= TaskData_SelectNextZtask;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Window.Current.Bounds.Width < 900)
            {
                if (CompletedTaskList.SelectedItem != null || InCompleteTaskList.SelectedItem == null)
                {
                    TaskDetailsSV.Visibility = Visibility.Visible;
                    TaskListContainer.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TaskDetailsSV.Visibility = Visibility.Collapsed;
                    TaskListContainer.Visibility = Visibility.Visible;
                }
            }
            else
            {
                TaskListContainer.Visibility = Visibility.Visible;
            }
        }
    }
}