using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using ProjectManagmentApp.View.TaskUserControls;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProjectManagmentApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditTask : Page
    {
        private ObservableCollection<ZTask> _taskList;
        private long _userId;
        TaskManager taskManager = TaskManager.GetTaskManager();
        UserManager userManager = UserManager.GetUserManager();

        public EditTask()
        {
            this.InitializeComponent();
            _userId = userManager.GetUserId();
            _taskList = new ObservableCollection<ZTask>(taskManager.GetUserCreatedTasks(_userId));
        }

        private void TaskList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ZTask clickedItem = (ZTask)e.ClickedItem;
            ZTask zTask = taskManager.GetZTask(clickedItem.Id);
            TaskDetailsFrameEdit.Navigate(typeof(TaskEditor), zTask);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SelectNextAvailable();
            TaskEditor.DeleteTaskEvent += TaskEditor_DeleteTaskEvent;
            TaskEditor.DeselectSelectedItem += TaskEditor_DeselectSelectedItem;
            TaskEditor.UpdateTaskEvent += TaskEditor_UpdateTaskEvent;
            if (Window.Current.Bounds.Width < 900)
                TaskData.MobileSupport += TaskData_MobileSupport;
        }

        private void TaskEditor_DeselectSelectedItem()
        {
            MyTaskList.SelectedItem = null;
        }

        private void TaskData_MobileSupport()
        {
            TaskDetailsSV.Visibility = Visibility.Collapsed;
            TaskListContainer.Visibility = Visibility.Visible;
        }


        private void TaskEditor_UpdateTaskEvent(long taskId)
        {
            var oldTask = _taskList.First(task => task.Id == taskId);
            var index = _taskList.IndexOf(oldTask);
            _taskList.Remove(oldTask);
            ZTask zTask = taskManager.GetZTask(taskId);
            _taskList.Insert(index, zTask);
            SelectNextAvailable();
        }

        private void TaskEditor_DeleteTaskEvent(long taskId)
        {
            _taskList.Remove(_taskList.FirstOrDefault(task => task.Id == taskId));
            SelectNextAvailable();
        }

        private void SelectNextAvailable()
        {
            if (_taskList.Count != 0)
            {
                MyTaskList.SelectedIndex = 0;
                ZTask selecteditem = (ZTask)MyTaskList.SelectedItem;
                ZTask completeZTask = taskManager.GetZTask(selecteditem.Id);
                TaskDetailsFrameEdit.Navigate(typeof(TaskEditor), completeZTask);
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            TaskEditor.DeleteTaskEvent -= TaskEditor_DeleteTaskEvent;
            TaskEditor.UpdateTaskEvent -= TaskEditor_UpdateTaskEvent;
        }

    }
}