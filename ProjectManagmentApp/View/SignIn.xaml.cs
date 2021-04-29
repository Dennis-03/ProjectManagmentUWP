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
    public sealed partial class SignIn : Page
    { 
        UserManager userManager = UserManager.GetUserManager();

        public SignIn()
        {
            this.InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var name = Username.Text;
            var password = Password.Password;
            if(!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(name))
            {
                User user = userManager.VerifyUser(name, password);

                if (user!=null)
                {
                    userManager.SetUserId(user.Id);
                    Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    ResponseTextBlock.Text = "Invalid User";
                }
            }
            else
            {
                ResponseTextBlock.Text="Fill All the fields";
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUp));
        }
    }
}
