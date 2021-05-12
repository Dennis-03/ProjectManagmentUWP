using ProjectManagmentApp.Constants;
using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using ProjectManagmentApp.View.TaskUserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProjectManagmentApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class TaskView : Page
    {
        private ObservableCollection<ZTask> _taskList;
        TaskManager taskManager = TaskManager.GetTaskManager();
        UserManager userManager = UserManager.GetUserManager();
        private string _userName;

        public TaskView()
        {
            this.InitializeComponent();
            _taskList = new ObservableCollection<ZTask>(taskManager.ListAllTasks());
        }

        private void TaskList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ZTask clickedItem = (ZTask)e.ClickedItem;
            ZTask zTask= taskManager.GetZTask(clickedItem.Id);
            TaskDetailsFrame.Navigate(typeof(TaskDetails),zTask, new SuppressNavigationTransitionInfo());
            TaskDetailsSV.Visibility = Visibility.Visible;
            if (Window.Current.Bounds.Width < 900)
            {
                TaskListContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TaskData.TaskCompleted += HandleTaskCompleted;
            TaskData.SelectNextZtask += TaskData_SelectNextZtask;
            if (Window.Current.Bounds.Width < 900)
            {
                TaskData.MobileSupport += TaskData_MobileSupport;
            }
            _userName = userManager.GetUser(userManager.GetUserId()).UserName;
            SelectNextAvailable();
            WelcomeUserName.Text = String.Format(" {0} !!!", _userName);
        }

        private void TaskData_MobileSupport(bool status)
        {
            TaskDetailsSV.Visibility = Visibility.Collapsed;
            TaskListContainer.Visibility = Visibility.Visible;
        }

        private void TaskData_SelectNextZtask(bool obj)
        {
            SelectNextAvailable();
        }

        private void SelectNextAvailable()
        {
            TaskList.SelectedIndex = 0;
            var selectedItem = (ZTask)TaskList.SelectedItem;
            if (selectedItem != null)
            {
                ZTask zTask = taskManager.GetZTask(selectedItem.Id);
                TaskDetailsFrame.Navigate(typeof(TaskDetails), zTask, new SuppressNavigationTransitionInfo());
            }
        }

        private void HandleTaskCompleted(long taskId)
        {
            _taskList.Remove(_taskList.First(task=>task.Id==taskId));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            TaskData.TaskCompleted -= HandleTaskCompleted;
        }
    }
}
