using GitHubUsers.Command;
using GitHubUsers.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GitHubUsers.Annotations;
using GitHubUsers.Events;
using GitHubUsers.Managers;

namespace GitHubUsers.ViewModel
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<UserViewModel> UserViewModelCollection { get; set; }
        public MainWindowViewModel()
        {
            UserViewModelCollection = new ObservableCollection<UserViewModel>();
            DataManager.Instance.UserPackageDownloaded+= UserPackageDownloaded;
        }

        private void UserPackageDownloaded(object sender, UserListDownloaded userListDownloaded)
        {
            //UserViewModelCollection.Concat(userListDownloaded.UserViewModelList);
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (var user in userListDownloaded.UserViewModelList)
                    {
                        UserViewModelCollection.Add(user);
                    }
                })
            );
            
            OnPropertyChanged(nameof(UserViewModelCollection));
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
