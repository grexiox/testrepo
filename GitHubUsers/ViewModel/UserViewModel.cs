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
        public UserViewModel(string login,string url,int numberOfRepositories)
        {
            Login = login;
            Avatar = url;
            NumberOfRepository = numberOfRepositories;
        }
        public string Login { get; private set; }
        public string Avatar { get; private set; }
        public int NumberOfRepository { get; private set; }
    }
}
