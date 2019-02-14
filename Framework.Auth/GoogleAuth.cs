using Framework.Auth.Config;
using Framework.Auth.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Auth
{
    public class GoogleAuth
    {
         public async Task<GoogleProfileData> AuthWithGoogle(string code, string redirectUrl)
         {
            // get the access token
            HttpWebRequest webRequest    = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
            webRequest.Method            = "POST";
            var Parameters               = "code=" + code + "&client_id=" + GoogleConfiguration.GOOGLE_CLIENT_ID + "&client_secret=" + GoogleConfiguration.GOOGLE_CLIENT_SECRECT + "&redirect_uri=" + redirectUrl + "&grant_type=authorization_code";
            byte[] byteArray             = Encoding.UTF8.GetBytes(Parameters);
            webRequest.ContentType       = "application/x-www-form-urlencoded";
            webRequest.ContentLength     = byteArray.Length;
            Stream postStream            = webRequest.GetRequestStream();
            // Add the post data to the web request
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();
            WebResponse response         = webRequest.GetResponse();
            postStream                   = response.GetResponseStream();
            StreamReader reader          = new StreamReader(postStream);
            string responseFromServer    = reader.ReadToEnd();
            var serStatus                = JsonConvert.DeserializeObject<GoogleAuthResponse>(responseFromServer);
            GoogleProfileData profile    = new GoogleProfileData();
            if (serStatus != null)
            {
                string accessToken = string.Empty;
                accessToken = serStatus.access_token;

                if (!string.IsNullOrEmpty(accessToken))
                {
                    // This is where you want to add the code if login is successful.
                    profile = await GetGoogleProfileData(accessToken);
                }
            }
            return profile;
        }

        private async Task<GoogleProfileData> GetGoogleProfileData(string access_token)
        {
            GoogleProfileData serStatus = new GoogleProfileData();
            try
            {
                HttpClient client = new HttpClient();
                var urlProfile    = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + access_token;
                client.CancelPendingRequests();
                HttpResponseMessage output = await client.GetAsync(urlProfile);
                
                if (output.IsSuccessStatusCode)
                {
                    string outputData = await output.Content.ReadAsStringAsync();
                    serStatus         = JsonConvert.DeserializeObject<GoogleProfileData>(outputData);                                       
                }                
            }
            catch (Exception ex)
            {
                //catching the exception
            }
            return serStatus;
        }
    }
}
