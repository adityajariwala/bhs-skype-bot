using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace TestBot.Dialogs
{
    [Serializable]
    public class LuisDialog : IDialog<string>
    {
        private static readonly HttpClient client = new HttpClient();

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as Activity;
            
            string app = "2da737f8-f8a5-4e0e-9a04-3a173322e204";
            string subKey = "689cf652710d4959bd3c3d81e78e01ff";

            var responseString = await client.GetStringAsync("https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + app + "?subscription-key=" + subKey + "&verbose=true&timezoneOffset=0&q=" + message.Text);

            // Return our reply to the user
            await context.PostAsync(responseString);

            context.Done("");
        }
    }
}