using GitHubUsers.Command;
using GitHubUsers.Events;
using GitHubUsers.Managers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using GitHubUsers.Annotations;

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
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (var user in userListDownloaded.UserViewModelList)
                    {
                        UserViewModelCollection.Add(user);
                    }
                    Title = string.Format("GitHubUsers={0}", UserViewModelCollection.Count);
                    OnPropertyChanged(nameof(Title));
                })
            );
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

        public string Title { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
