using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace TestBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as Activity;
            var messageTextLower = message.Text.ToLower();

            if (messageTextLower.Equals("graph"))
            {
                context.Call(new GraphDialog(), ResumeAfterDialog);
            }
            else if (messageTextLower.StartsWith("graph"))
            {
                message.Text = message.Text.Replace("graph", "").Trim();
                await context.Forward(new GraphDialog(), ResumeAfterDialog, message, CancellationToken.None);
            }
            else if (messageTextLower.Equals("count"))
            {
                context.Call(new CountDialog(), ResumeAfterDialog);
            }
            else if (messageTextLower.StartsWith("count"))
            {
                message.Text = message.Text.Replace("count", "").Trim();
                await context.Forward(new CountDialog(), ResumeAfterDialog, message, CancellationToken.None);
            }
            else if (messageTextLower.Equals("echo"))
            {
                context.Call(new EchoDialog(), ResumeAfterDialog);
            }
            else if (messageTextLower.StartsWith("echo"))
            {
                message.Text = message.Text.Replace("echo", "").Trim();
                await context.Forward(new EchoDialog(), ResumeAfterDialog, message, CancellationToken.None);
            }
            else if (messageTextLower.Equals("luis"))
            {
                context.Call(new LuisDialog2(), ResumeAfterDialog);
            }
            else if (messageTextLower.StartsWith("luis"))
            {
                message.Text = message.Text.Replace("luis", "").Trim();
                await context.Forward(new LuisDialog2(), ResumeAfterDialog, message, CancellationToken.None);
            }
            else if (messageTextLower.Contains("luis"))
            {
                await context.Forward(new LuisDialog2(), ResumeAfterDialog, message, CancellationToken.None);
            }
            else if (messageTextLower.Equals("old luis"))
            {
                context.Call(new LuisDialog(), ResumeAfterDialog);
            }
            else if (messageTextLower.StartsWith("old luis"))
            {
                message.Text = message.Text.Replace("old luis", "").Trim();
                await context.Forward(new LuisDialog(), ResumeAfterDialog, message, CancellationToken.None);
            }
            else
            {
                // User typed something else; for simplicity, ignore this input and wait for the next message.
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterDialog(IDialogContext context, IAwaitable<string> result)
        {
            var resultFromDialog = await result;

            //await context.PostAsync($"The dialog just told me this: {resultFromDialog}");

            // Again, wait for the next message from the user.
            context.Wait(MessageReceivedAsync);
        }
    }
}