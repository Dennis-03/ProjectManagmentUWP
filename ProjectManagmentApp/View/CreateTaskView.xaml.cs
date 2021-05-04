﻿using ProjectManagmentApp.Constants;
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
    public sealed partial class CreateTaskView : Page
    {
        private ZTask _zTask= new ZTask();
        TaskManager taskManager = TaskManager.GetTaskManager();

        public CreateTaskView()
        {
            this.InitializeComponent();
            _zTask.Priority = PriorityEnum.Low;
        }

        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_zTask.Description) && !string.IsNullOrEmpty(_zTask.TaskName) && _zTask.DueDate.Ticks != 0 && _zTask.AssignedTo != 0)
            {
                taskManager.AddTask(_zTask);
                Frame.Navigate(typeof(EditTask));
            }
            else
                DisplayError.Visibility = Visibility.Visible;
        }
    }
}
