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
        private List<User> _users;

        public ZTask ZTask
        {
            get { return (ZTask)GetValue(ZTaskProperty); }
            set { SetValue(ZTaskProperty, value); }
        }
        public static readonly DependencyProperty ZTaskProperty = DependencyProperty.Register("ZTask", typeof(ZTask), typeof(TaskControls), null);

        public TaskControls()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _users = new List<User>(userManager.GetAllUsers());
            UserSelectCB.ItemsSource = _users;
            zTask.AssignedBy = userManager.GetUserId();
            zTask.AssignedDate = DateTime.Now;
            //PriorityComboBox.SelectedItem = ZTask.Priority.ToString();
            if (ZTask.Priority == PriorityEnum.High)
            {
                PriorityComboBox.SelectedIndex = 2;
            }
            else if (ZTask.Priority == PriorityEnum.Medium)
            {
                PriorityComboBox.SelectedIndex = 1;
            }
            else
            {
                PriorityComboBox.SelectedIndex = 0;
            }
        }

        private void IDueDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            DateTimeOffset date = (DateTimeOffset)IDueDate.SelectedDate;
            zTask.DueDate = date.DateTime;
            SetValue(ZTaskProperty, zTask);
        }

        private void PriorityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var item = (ComboBoxItem)combo.SelectedItem;
            zTask.Priority = Enum.Parse<PriorityEnum>(item.Content.ToString());
            SetValue(ZTaskProperty, zTask);
        }

        private void UserSelectCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var user = (User)combo.SelectedItem;
            zTask.AssignedTo = (long)user.Id;
            SetValue(ZTaskProperty, zTask);
        }

        private void ITaskDescripion_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            zTask.Description = textBox.Text;
            SetValue(ZTaskProperty, zTask);
        }

        private void ITaskName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            zTask.TaskName = textBox.Text;
            SetValue(ZTaskProperty, zTask);
        }

        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
