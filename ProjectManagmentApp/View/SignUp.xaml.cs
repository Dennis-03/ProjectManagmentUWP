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
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
        private string _avatar;
        private ObservableCollection<Avatar> _avatarList;
        public SignUp()
        {
            this.InitializeComponent();
            _avatarList = new ObservableCollection<Avatar>
            {
                new Avatar { Path = "ms-appx:/Assets/Avatar/male-01.png", AvatarName = "Male 1" },
                new Avatar { Path = "ms-appx:/Assets/Avatar/male-02.png", AvatarName = "Male 2" },
                new Avatar { Path = "ms-appx:/Assets/Avatar/male-03.png", AvatarName = "Male 3" },
                new Avatar { Path = "ms-appx:/Assets/Avatar/female-01.png", AvatarName = "Female 1" },
                new Avatar { Path = "ms-appx:/Assets/Avatar/female-02.png", AvatarName = "Female 2" },
                new Avatar { Path = "ms-appx:/Assets/Avatar/female-03.png", AvatarName = "Female 3" }
            };
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var userName = Username.Text;
            var password = Password.Password;
            var rePassword = RePassword.Password;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePassword) || string.IsNullOrEmpty(_avatar))
            {
                ResponseTextBlock.Text = "Fill all the Fields";
            }
            else if (rePassword == password)
            {
                var status = userManager.AddUser(userName, password,_avatar);
                if (status == false)
                    ResponseTextBlock.Text = "Username Already exists";
                else
                    Frame.Navigate(typeof(SignIn));
            }
            else
                ResponseTextBlock.Text = "Passwords do not match";
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignIn));
        }

        private void AvatarCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var item = (Avatar)combo.SelectedItem;
            if (item != null)
            {
                _avatar = item.Path.ToString();
                Uri uri = new Uri(_avatar, UriKind.Absolute);
                DP.Source = new BitmapImage(uri);
            }
        }
        private async void GetPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker =new FileOpenPicker();
            openPicker.SuggestedStartLocation =PickerLocationId.PicturesLibrary;
            openPicker.ViewMode =PickerViewMode.Thumbnail;

            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");

            StorageFile file =await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                using (Windows.Storage.Streams.IRandomAccessStream fileStream =await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    DP.Source = bitmapImage;
                    _avatar= file.Path;
                    AvatarCB.SelectedItem = null;
                }
            }
        }

        private void CustomAvatar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AvatarCB.SelectedIndex = 0;
            Uri uri = new Uri("ms-appx:/Assets/Avatar/male-01.png", UriKind.Absolute);
            DP.Source = new BitmapImage(uri);
        }
    }
}
