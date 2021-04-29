using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using System.Collections.ObjectModel;
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

        public ObservableCollection<ZTask> MyZTasks
        {
            get { return (ObservableCollection<ZTask>)GetValue(MyZTasksProperty); }
            set { SetValue(MyZTasksProperty, value); }
        }
        public DependencyProperty MyZTasksProperty = DependencyProperty.Register("MyZTasksProperty", typeof(ObservableCollection<ZTask>), typeof(EditTask), null);

        public EditTask()
        {
            this.InitializeComponent();
        }
        private void TaskList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ZTask clickedItem = (ZTask)e.ClickedItem;
            ZTask zTask = taskManager.GetZTask(clickedItem.Id);
            TaskDetailsFrameEdit.Navigate(typeof(TaskDetails), zTask);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _taskList = new ObservableCollection<ZTask>(taskManager.GetUserCreatedTasks(_userId));
            SetValue(MyZTasksProperty, _taskList);
            COUNTER.Text = _taskList.Count.ToString();
            _userId = userManager.GetUserId();
            MyTaskList.ItemsSource = MyZTasks;
        }
    }
}