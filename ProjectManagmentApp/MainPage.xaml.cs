﻿using ProjectManagmentApp.Data;
using ProjectManagmentApp.View;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ProjectManagmentApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        UserManager userManager = UserManager.GetUserManager();

        public MainPage()
        {
            this.InitializeComponent();

            MainSplitView.OpenPaneLength = 200;
            MainSplitView.CompactPaneLength = 50;
            TitleBlock.Visibility = Visibility.Visible;
            NavigationMenu.Visibility = Visibility.Visible;
            TasksMenu.IsSelected = true;
            if (userManager.GetUserId() == 0)
            {
                MainRenderFrame.Navigate(typeof(SignIn));
                HideMenu();
            }

        }

        private void Hamburger_button_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void NavigationMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksMenu.IsSelected)
            {
                MainRenderFrame.Navigate(typeof(TaskView));
            }
            if (MyTasksMenu.IsSelected)
            {
                MainRenderFrame.Navigate(typeof(MyTasksView));
            }
            if (CreateTaskMenu.IsSelected)
            {
                MainRenderFrame.Navigate(typeof(CreateTaskView));
            }
            if (EditTask.IsSelected)
            {
                MainRenderFrame.Navigate(typeof(EditTask));
            }
            if (Logout.IsSelected)
            {
                LogoutUser();
            }

        }

        public void RedirectSignIn()
        {
            MainRenderFrame.Navigate(typeof(SignIn));
            HideMenu();
        }


        public void LogoutUser()
        {
            userManager.Logout();
            MainRenderFrame.Navigate(typeof(SignIn));
            HideMenu();
        }


        public void HideMenu()
        {
            MainSplitView.CompactPaneLength = 0;
            MainSplitView.OpenPaneLength = 0;
            TitleBlock.Visibility = Visibility.Collapsed;
            NavigationMenu.Visibility = Visibility.Collapsed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoForward)
            {
                Frame.GoForward();
            }
        }
    }
}
