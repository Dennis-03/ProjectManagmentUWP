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
        private List<ZTask> _allTasks;
        TaskManager taskManager = TaskManager.GetTaskManager();
        UserManager userManager = UserManager.GetUserManager();
        private long _userId;

        public TaskView()
        {
            this.InitializeComponent();
            _allTasks = new List<ZTask>(taskManager.ListAllTasks());
            _taskList = new ObservableCollection<ZTask>(_allTasks);
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
            TaskData.DeselectItem += TaskData_DeselectItem;
            TaskData.TaskEditor += TaskData_TaskEditor;
            TaskEditor.UpdateTaskEvent += TaskEditor_UpdateTaskEvent;
            TaskEditor.DeselectSelectedItem += TaskEditor_DeselectSelectedItem;
            if (Window.Current.Bounds.Width < 900)
            {
                TaskDetailsSV.Visibility = Visibility.Collapsed;
            }
            SelectNextAvailable();
            _userId = userManager.GetUserId();
        }

        private void TaskEditor_DeselectSelectedItem()
        {
            TaskList.SelectedItem = null;
        }

        private void TaskEditor_UpdateTaskEvent(long taskId)
        {
            var index = _taskList.IndexOf(_taskList.FirstOrDefault(task=>task.Id==taskId));
            _taskList.RemoveAt(index);
            _taskList.Insert(index, taskManager.GetZTask(taskId));
            SelectNextAvailable();
        }

        private void TaskData_TaskEditor(long taskId)
        {
            TaskDetailsFrame.Navigate(typeof(TaskEditor), taskManager.GetZTask(taskId));
        }

        private void TaskData_DeselectItem()
        {
            TaskList.SelectedItem = null;
            if (Window.Current.Bounds.Width < 900)
            {
                TaskListContainer.Visibility = Visibility.Visible;
                TaskDetailsSV.Visibility = Visibility.Collapsed;
            }
        }

        private void SelectNextAvailable()
        {
            if (Window.Current.Bounds.Width > 900)
            {
                TaskList.SelectedIndex = 0;
                var selectedItem = (ZTask)TaskList.SelectedItem;
                if (selectedItem != null)
                {
                    ZTask zTask = taskManager.GetZTask(selectedItem.Id);
                    TaskDetailsFrame.Navigate(typeof(TaskDetails), zTask, new SuppressNavigationTransitionInfo());
                    TaskDetailsSV.Visibility = Visibility.Visible;
                }
            }
        }

        private void HandleTaskCompleted(long taskId)
        {
            _allTasks.Remove(_allTasks.FirstOrDefault(task => task.Id == taskId));
            _taskList.Remove(_taskList.FirstOrDefault(task => task.Id == taskId));
            TaskDetailsSV.Visibility = Visibility.Collapsed;
            SelectNextAvailable();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            TaskData.TaskCompleted -= HandleTaskCompleted;
            TaskData.DeselectItem -= TaskData_DeselectItem;
            TaskData.TaskEditor -= TaskData_TaskEditor;
            TaskEditor.UpdateTaskEvent -= TaskEditor_UpdateTaskEvent;
            TaskEditor.DeselectSelectedItem -= TaskEditor_DeselectSelectedItem;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Window.Current.Bounds.Width < 900)
            {
                if (TaskList.SelectedItem == null)
                {
                    TaskDetailsSV.Visibility = Visibility.Collapsed;
                    TaskListContainer.Visibility = Visibility.Visible;
                }
                else
                {
                    TaskListContainer.Visibility = Visibility.Collapsed;
                    TaskDetailsSV.Visibility = Visibility.Visible;
                }
            }
            else
                TaskListContainer.Visibility = Visibility.Visible;
        }

        private void AllTasks_Click(object sender, RoutedEventArgs e)
        {
            TaskListType.Text = "All Tasks";
            if (_taskList != null)
                _taskList.Clear();
            _allTasks.ForEach(task =>
            {
                _taskList.Add(task);
            });
            SelectNextAvailable();
            TaskListTypeSelect.Flyout.Hide();
        }

        private void MyTasks_Click(object sender, RoutedEventArgs e)
        {
            TaskListType.Text = "My Tasks";
            _taskList.Clear();
            _allTasks.ForEach(task =>
            {
                if (task.AssignedTo == _userId)
                    _taskList.Add(task);
            });
            SelectNextAvailable();
            TaskListTypeSelect.Flyout.Hide();
        }

        private void TasksByMe_Click(object sender, RoutedEventArgs e)
        {
            TaskListType.Text = "Tasks By Me";
            _taskList.Clear();
            _allTasks.ForEach(task =>
            {
                if (task.AssignedBy == _userId)
                    _taskList.Add(task);
            });
            SelectNextAvailable();
            TaskListTypeSelect.Flyout.Hide();
        }
    }
}
