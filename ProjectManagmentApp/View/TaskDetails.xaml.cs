using ProjectManagmentApp.Data;
using ProjectManagmentApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class TaskDetails : Page
    {
        private ZTask _zTask;
        private string _assignedTo;
        private string _assignedBy;
        private ObservableCollection<Comment> _comments;
        private SolidColorBrush _priorityColor;


        UserManager userManager = UserManager.GetUserManager();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _zTask = (ZTask)e.Parameter;
            _assignedTo = userManager.GetUser(_zTask.AssignedTo).UserName;
            _assignedBy = userManager.GetUser(_zTask.AssignedBy).UserName;
            _comments =new ObservableCollection<Comment>(_zTask.Comment);
            if (_zTask.Priority == Constants.PriorityEnum.Low)
            {
                _priorityColor = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            }
            else if (_zTask.Priority == Constants.PriorityEnum.Medium)
            {
                _priorityColor = new SolidColorBrush(Color.FromArgb(255, 255, 160, 0));
            }
            else
            {
                _priorityColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            }


        }
        public TaskDetails()
        {
            this.InitializeComponent();
        }
    }
}
