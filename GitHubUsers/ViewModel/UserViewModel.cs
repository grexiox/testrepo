using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GitHubUsers.ViewModel
{
    public class UserViewModel
    {
        public string Login { get; private set; }
        public Image Avatar { get; private set; }
        public int NumberOfRepository { get; private set; }
    }
}
