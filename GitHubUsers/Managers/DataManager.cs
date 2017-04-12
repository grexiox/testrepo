using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using GitHubUsers.Events;
using GitHubUsers.Model;
using GitHubUsers.ViewModel;

namespace GitHubUsers.Managers
{
    public class DataManager
    {
        private static DataManager _instance = null;
        private static object _syncRoot = new object();
        private readonly string _url = "https://api.github.com/users";
        private const string Token = "e7f2a01795e0ab789e383ed4131fc27ad8fff0ce";
        private readonly JavaScriptSerializer _javaScriptSerializer = new JavaScriptSerializer();
        
        private DataManager()
        {
        }

        public EventHandler<UserListDownloaded> UserPackageDownloaded;

        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DataManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public async void DownloadUsers(string url=null)
        {
            url = url ?? _url;
            var users = await DownloadJson(url,JsonType.Users);
            var userList = _javaScriptSerializer.Deserialize<List<UserModel>>(users);
            List<Task<string>> taskListrepo = new List<Task<string>>();
            foreach (var user in userList)
            {
                taskListrepo.Add(DownloadJson(user.url, JsonType.Repository));
            }

            var userViewModelList = new List<UserViewModel>();
            for(int i=0;i<userList.Count;i++)
            {
                string reposjson = taskListrepo[i].Result;
                var repoModel = _javaScriptSerializer.Deserialize<RepoModel>(reposjson);
                var userViewModel = new UserViewModel(userList[i].login,userList[i].avatar_url, repoModel.public_repos);
                userViewModelList.Add(userViewModel);
            }
            UserPackageDownloaded(this, new UserListDownloaded(userViewModelList));
        }


        private async Task<string> DownloadJson(string url, JsonType jsonType)
        {
            url += string.Format("?access_token={0}", Token);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Proxy = null;
            httpWebRequest.UserAgent = "Mozilla/5.0 (Android 4.4; Mobile; rv:41.0) Gecko/41.0 Firefox/41.0";
            using (WebResponse response = await httpWebRequest.GetResponseAsync())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
        }
        enum JsonType
        {
            Users,
            Repository
        }
    }
}
