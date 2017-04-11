using GitHubUsers.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Media.Imaging;

namespace GitHubUsers.Model
{
    public class DataManager
    {
        private static DataManager _instance = null;
        private static object _syncRoot = new object();
        private readonly string _url = "https://api.github.com/users";
        private readonly JavaScriptSerializer _javaScriptSerializer = new JavaScriptSerializer();
        private DataManager()
        {
        }

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

        public async void DownloadUsers(string path=null)
        {
            path = path ?? _url;
            var users = await DownloadJson(path,JsonType.Users);
            var userList = _javaScriptSerializer.Deserialize<List<UserModel>>(users);
            List<Task<string>> taskListrepo = new List<Task<string>>();
            List<Task<BitmapImage>> taskListImage = new List<Task<BitmapImage>>();
            //foreach(var user in userList)
            //{
            //    taskListrepo.Add(DownloadJson(user.repos_url,JsonType.Repository));
            //    taskListImage.Add(GetImageFromUrl(user.avatar_url));
            //}
            
            //Task.WaitAll(taskListImage.ToArray());
            //Task.WaitAll(taskListrepo.ToArray());
            var userViewModelList = new List<UserViewModel>();
            for(int i=0;i<userList.Count;i++)
            {
                string reposjson = taskListrepo[i].Result;
                var repoList = _javaScriptSerializer.Deserialize<List<RepoModel>>(reposjson);
                var userViewModel = new UserViewModel(userList[i].login, taskListImage[i].Result, repoList.Count);
                userViewModelList.Add(userViewModel);
            }
        }


        private async Task<string> DownloadJson(string path, JsonType jsonType)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(_url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Proxy = null;
            httpWebRequest.Credentials = new NetworkCredential("grexiox", "1z10Wygrana");
            httpWebRequest.UserAgent = "Mozilla/5.0 (Android 4.4; Mobile; rv:41.0) Gecko/41.0 Firefox/41.0";
            using (WebResponse response = await httpWebRequest.GetResponseAsync())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
             //       Console.WriteLine(response.Headers["Link"]);
                    return reader.ReadToEnd();
                }
            }
        }

        private async Task<BitmapImage> GetImageFromUrl(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "image/*";
            httpWebRequest.Proxy = null;
            httpWebRequest.Credentials = new NetworkCredential("grexiox", "1z10Wygrana");
            httpWebRequest.UserAgent = "Mozilla/5.0 (Android 4.4; Mobile; rv:41.0) Gecko/41.0 Firefox/41.0";
            try
            {
                BitmapImage img = new BitmapImage();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.BeginInit();
                img.StreamSource = await httpWebRequest.GetRequestStreamAsync();
                img.EndInit();
                return img;
            }
            catch (HttpRequestException ex)
            {
                // the download failed, log error
                return null;
            }
        }

        enum JsonType
        {
            Users,
            Repository
        }
    }
}
