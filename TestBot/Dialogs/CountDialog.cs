using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace TestBot.Dialogs
{
    [Serializable]
    public class CountDialog : IDialog<string>
    {
        protected int count = 1;

        public async Task StartAsync(IDialogContext context)
        {
            count = context.ConversationData.GetValueOrDefault("MESSAGE_COUNT", 1);
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message.Text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Are you sure you want to reset the count?",
                    "I don't understand! Can you rephrase that?",
                    promptStyle: PromptStyle.None);
            }
            else
            {
                await context.PostAsync($"Your message count is: {count++}.");
                context.ConversationData.SetValue("MESSAGE_COUNT", count);
                context.Done("");
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                count = 1;
                context.ConversationData.SetValue("MESSAGE_COUNT", count);
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Done("");
        }
    }
}