using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
            var task1 = DownloadJson(path,JsonType.Users);
            Task.WaitAll(task1);
            var list = _javaScriptSerializer.Deserialize<List<UserModel>>(task1.Result);
        }

        //  private async Task<int> DownloadNumberOfRepositories(string path)
        //  {

        //        }
        private async Task<string> DownloadJson(string path, JsonType jsonType)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(_url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Proxy = null;
            httpWebRequest.UserAgent = "Mozilla/5.0 (Android 4.4; Mobile; rv:41.0) Gecko/41.0 Firefox/41.0";
            using (WebResponse response = await httpWebRequest.GetResponseAsync())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    Console.WriteLine(response.Headers["Link"]);
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
