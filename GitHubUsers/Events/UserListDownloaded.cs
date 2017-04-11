using GitHubUsers.ViewModel;
using System;
using System.Collections.Generic;

namespace GitHubUsers.Events
{
    public class UserListDownloaded : EventArgs
    {
        public List<UserViewModel> UserViewModelList { get; private set; }
        public UserListDownloaded(List<UserViewModel> userViewModelList)
        {
            this.UserViewModelList = userViewModelList;
        }
    }
}
