using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GitHubUsers.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel(string login,BitmapImage bitmapImage,int numberOfRepositories)
        {
            Login = login;
            Avatar = bitmapImage;
            NumberOfRepository = numberOfRepositories;
        }
        public string Login { get; private set; }
        public BitmapImage Avatar { get; private set; }
        public int NumberOfRepository { get; private set; }
    }
}
