using ProjectManagmentApp.Data;
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
    public sealed partial class SignUp : Page
    {

        UserManager userManager = UserManager.GetUserManager();

        public SignUp()
        {
            this.InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var userName = Username.Text;
            var password = Password.Password;
            var rePassword = RePassword.Password;

            if(rePassword == password)
            {
                var status=userManager.AddUser(userName, password);
                if (status == false)
                {
                    ResponseTextBlock.Text = "Username Already exists";
                }
                else
                {
                    Frame.Navigate(typeof(SignIn));
                }
            }
            else
            {
                ResponseTextBlock.Text = "Passwords do not match";
            }
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignIn));
        }
    }
}
