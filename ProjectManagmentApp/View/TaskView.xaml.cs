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
            TaskDetailsFrame.Navigate(typeof(TaskDetails),zTask);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TaskData.TaskCompleted += HandleTaskCompleted;
            _userName = userManager.GetUser(userManager.GetUserId()).UserName;
            WelcomeUserName.Text = String.Format(" {0} !!!", _userName);
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
