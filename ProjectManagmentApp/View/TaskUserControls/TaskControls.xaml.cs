using ProjectManagmentApp.Constants;
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
    public sealed partial class TaskControls : UserControl
    {
        UserManager userManager = UserManager.GetUserManager();
        TaskManager taskManager = TaskManager.GetTaskManager();

        private ZTask zTask = new ZTask();
        private List<User> users;

        private long userId;

        public TaskControls()
        {
            this.InitializeComponent();
        }
        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            zTask.TaskName = ITaskName.Text;
            zTask.Description = ITaskDescripion.Text;
            zTask.AssignedBy = userId;
            zTask.AssignedDate = DateTime.Now;
            DisplayResult.Text = $"{zTask.TaskName},{zTask.Description},{zTask.AssignedTo},{zTask.Priority},{zTask.DueDate},{zTask.Completed}";
            taskManager.AddTask(zTask);
        }

        private void IDueDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            DateTimeOffset date = (DateTimeOffset)IDueDate.SelectedDate;
            zTask.DueDate = date.DateTime;
        }

        private void PriorityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var item = (ComboBoxItem)combo.SelectedItem;
            zTask.Priority = Enum.Parse<PriorityEnum>(item.Content.ToString());

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            users = new List<User>(userManager.GetAllUsers());
            UserSelectCB.ItemsSource = users;
            userId = userManager.GetUserId();
        }

        private void UserSelectCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var user = (User)combo.SelectedItem;
            zTask.AssignedTo = (long)user.Id;
        }
    }
}
