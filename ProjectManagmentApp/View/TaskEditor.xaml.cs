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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProjectManagmentApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskEditor : Page
    {
        ZTask ZTask;
        TaskManager taskManager = TaskManager.GetTaskManager();

        public static event Action<long> UpdateTaskEvent;
        public static event Action DeselectSelectedItem;
        public static event Action MobileSupport;

        public static void NotifyMobileSupport()
        {
            MobileSupport?.Invoke();
        }

        public static void NotifyDeselectSelectedItem()
        {
            DeselectSelectedItem?.Invoke();
        }

        public static void NotifyUpdateTaskEvent(long taskId)
        {
            UpdateTaskEvent?.Invoke(taskId);
        }

        public TaskEditor()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ZTask = (ZTask)e.Parameter;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            TaskEditorContainer.Visibility = Visibility.Collapsed;
            NotifyDeselectSelectedItem();
        }

        private void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ZTask.Description) && !string.IsNullOrEmpty(ZTask.TaskName))
            {
                taskManager.UpdateTask(ZTask);
                TaskEditorContainer.Visibility = Visibility.Collapsed;
                NotifyUpdateTaskEvent(ZTask.Id);
            }
            else
                DisplayError.Visibility = Visibility.Visible;

        }
    }
}
