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
        private ObservableCollection<ZTask> taskList;
        TaskManager taskManager = TaskManager.GetTaskManager();
        UserManager userManager = UserManager.GetUserManager();
        private string UserName;
        private string displayString;

        public TaskView()
        {
            this.InitializeComponent();
            taskManager.AddTask("Complete UWP Training", "Complete all the pending UWP videos", PriorityEnum.High, 2, 1, DateTime.Now, DateTime.Now,false);
            taskManager.AddTask("UWP Training", "Complete all the pending UWP videos", PriorityEnum.Medium, 1, 2, DateTime.Now, DateTime.Now, false);
            taskManager.AddTask("Complete Pending tasks", "Complete all the pending UWP videos Complete all the pending UWP videos", PriorityEnum.Low, 1, 2, DateTime.Now, DateTime.Now, true);
            taskList = new ObservableCollection<ZTask>(taskManager.ListAllTasks());
            UserName = userManager.GetUser(userManager.GetUserId()).UserName;
            if (!string.IsNullOrEmpty(UserName))
            {
                displayString = String.Format("Welcome {0} !!!",UserName);
            }
        }

        private void TaskList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ZTask clickedItem = (ZTask)e.ClickedItem;
            ZTask zTask= taskManager.GetZTask(clickedItem.Id);
            TaskDetailsFrame.Navigate(typeof(TaskDetails),zTask);
        }
    }
}
