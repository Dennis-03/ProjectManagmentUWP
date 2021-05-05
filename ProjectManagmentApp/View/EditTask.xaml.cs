using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
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
            _userId = userManager.GetUserId();
            TaskEditor.DeleteTaskEvent += TaskEditor_DeleteTaskEvent;
            TaskEditor.UpdateTaskEvent += TaskEditor_UpdateTaskEvent;
        }

        private void TaskEditor_UpdateTaskEvent(long taskId)
        {
            _taskList.Remove(_taskList.First(task => task.Id == taskId));
            ZTask zTask = taskManager.GetZTask(taskId);
            _taskList.Add(zTask);
        }

        private void TaskEditor_DeleteTaskEvent(long taskId)
        {
            _taskList.Remove(_taskList.First(task => task.Id == taskId));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            TaskEditor.DeleteTaskEvent -= TaskEditor_DeleteTaskEvent;
            TaskEditor.UpdateTaskEvent -= TaskEditor_UpdateTaskEvent;
        }
    }
}