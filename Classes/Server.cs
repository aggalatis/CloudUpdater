using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Script.Serialization;


namespace CloudUpdater.Classes
{
    class Server
    {

        public string serverUrl = String.Empty;
        public string storeID = String.Empty;
        public string token = String.Empty;
        public string dbuser = String.Empty;
        public string dbpass = String.Empty;
        public string dbport = String.Empty;
        public string dbhost = String.Empty;
        public string dbname = String.Empty;

        public Server(Helpers helpers) {
            this.serverUrl = helpers.parameters["url"];
        }

        public class WebClientWithTimeout : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest wr = base.GetWebRequest(address);
                wr.Timeout = 10000; // timeout in milliseconds (ms)
                return wr;
            }
        }

        public bool authecticated(Helpers Helpers) {

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(this.serverUrl + "clients/authenticate");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                       username = Helpers.parameters["username"],
                       password = Helpers.parameters["password"]

                    });
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic userData = JObject.Parse(result);
                    if (userData.token != null) {
                        NameValueCollection userResponse = new NameValueCollection();
                        this.storeID = userData.user.store_id;
                        this.token = userData.token;
                        this.dbname = userData.user.dbname;
                        this.dbuser = userData.user.dbuser;
                        this.dbpass = userData.user.dbpass;
                        this.dbhost = userData.user.dbhost;
                        this.dbport = userData.user.dbport;
                        return true;

                    }
                    return false;

                }
            }
            catch (Exception exp)
            {
                Helpers.writeLog("Exception on authentcation: " + exp.ToString());
                return false;
            }
        }

        public int retriveLastID(Helpers Helpers) {
            
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(this.serverUrl + "data/lastid/" + this.storeID);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization", $"Bearer {this.token}");
                httpWebRequest.Method = "POST";
                using (System.IO.Stream s = httpWebRequest.GetResponse().GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        dynamic orderData = JObject.Parse(jsonResponse);
                        return orderData.lastID;
                    }
                }
            }
            catch (Exception exp)
            {
                Helpers.writeLog("Exception on LastID: " + exp.ToString());
                return 0;
            }
        }

    }
}
