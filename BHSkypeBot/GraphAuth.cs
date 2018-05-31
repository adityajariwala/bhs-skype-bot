using BotAuth;
using BotAuth.AADv2;
using BotAuth.Dialogs;
using BotAuth.Models;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BHSkypeBot
{
    [Serializable]
    public class GraphAuth
    {
        public async Task GraphLogIn() { 
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

            queryString["client_id"] = "14c0c9ab-3cac-48ef-9692-f88d0e60bdbd";
            queryString["scope"] = "user.read";
            queryString["redirect_uri"] = "http%3A%2F%2Flocalhost%3A3978%2Fapi%2Fmessages%2F";
            queryString["grant_type"] = "authorization_code";
            queryString["client_secret"] = "JqQX2PNo9bpM0uEihUPzyrh";

            var data = "client_id=14c0c9ab-3cac-48ef-9692-f88d0e60bdbd&scope=user.read&redirect_uri=http%3A%2F%2Flocalhost%3A3978%2Fapi%2Fmessages%2F&grant_type=authorization_code&client_secret=JqQX2PNo9bpM0uEihUPzyrh";

            StringContent qs = new StringContent(data);

            var uri = "https://login.microsoftonline.com/common/oauth2/v2.0/token?";
            var response = await client.PostAsync(uri, qs);
            var strResponseContent = await response.Content.ReadAsStringAsync();
            dynamic ans = JsonConvert.DeserializeObject(strResponseContent);
        }
    }
}
