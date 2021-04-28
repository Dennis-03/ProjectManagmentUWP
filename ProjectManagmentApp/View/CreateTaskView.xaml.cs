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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProjectManagmentApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateTaskView : Page
    {
        private ZTask ztask = new ZTask();

        public CreateTaskView()
        {
            this.InitializeComponent();
        }

        //private void CreateTask_Click(object sender, RoutedEventArgs e)
        //{
        //    ztask.TaskName = ITaskName.Text;
        //    ztask.Description = ITaskDescripion.Text;
        //    ztask.Priority = Enum.Parse<PriorityEnum>(PriorityComboBox.Text);
        //    DisplayResult.Text = $"{ztask.TaskName},{ztask.Description},{ztask.AssignedTo},{ztask.Priority},{ztask.DueDate},{ztask.Completed}";
        //}

        //private void IDueDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        //{
        //    DateTimeOffset date = (DateTimeOffset)IDueDate.SelectedDate;
        //    ztask.DueDate = date.DateTime;
        //}

        //private void IAssigedTo_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        //{

        //}

        //private void PriorityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}
    }
}
