using System;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using TestBot.Helpers;

namespace TestBot.Dialogs
{
    [Serializable]
    public class GraphDialog : IDialog<string>
    {
        private static string ConnectionName = ConfigurationManager.AppSettings["ConnectionName"];

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var message = activity.Text.ToLower();

            if (message.Equals("me"))
            {
                // Display information about the logged in user
               context.Call(CreateGetTokenDialog(), ListMe);
            }
            else if (message.Equals("signout"))
            {
                // Sign the user out from AAD
                await Signout(context);
                context.Done("");
            }
            else
            {
                await context.PostAsync("Graph command not found.");
                context.Done("");
            }
        }

        #region Graph function
        private async Task ListMe(IDialogContext context, IAwaitable<GetTokenResponse> tokenResponse)
        {
            var token = await tokenResponse;
            var client = new GraphClient(token.Token);

            var me = await client.GetMe();
            //var manager = await client.GetManager();

            //await context.PostAsync($"You are {me.DisplayName} and you report to {manager.DisplayName}.");
            await context.PostAsync($"You are {me.DisplayName}.");
            context.Done("");
        }
        #endregion

        #region Azure AD Functions
        public static async Task Signout(IDialogContext context)
        {
            await context.SignOutUserAsync(ConnectionName);
            await context.PostAsync($"You have been signed out.");
        }
        
        /// Creates a GetTokenDialog using custom strings
        private GetTokenDialog CreateGetTokenDialog()
        {
            return new GetTokenDialog(
                ConnectionName,
                $"Please sign in to {ConnectionName} to proceed.",
                "Sign In",
                2,
                "Hmm. Something went wrong, let's try again.");
        }
        #endregion
    }
}