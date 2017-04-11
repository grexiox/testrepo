using GitHubUsers.Command;
using GitHubUsers.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GitHubUsers.ViewModel
{
    public class MainWindowViewModel
    {
        public ObservableCollection<UserViewModel> UserViewModelCollection;
        public MainWindowViewModel()
        {
            UserViewModelCollection = new ObservableCollection<UserViewModel>();
        }
        ICommand _loadedWindowCommand;
        public ICommand LoadedWindowCommand {
            get
            {
                return _loadedWindowCommand ?? (_loadedWindowCommand = new CommandHandler(() => OnLoaded(), true));
            }
        }


        private async void OnLoaded()
        {
            await Task.Run(() =>
            {
                DataManager.Instance.DownloadUsers();
            });
        }
    }
}
