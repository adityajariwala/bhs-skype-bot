using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace TestBot.Dialogs
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class LuisDialog2 : LuisDialog<string>
    {
        static string app = "2da737f8-f8a5-4e0e-9a04-3a173322e204";
        static string subKey = "3d19c5ccff8c4141820fa01beafa7246";
        static string domain = "westus.api.cognitive.microsoft.com";
        public LuisDialog2() : base(new LuisService(new LuisModelAttribute(
                app,
                subKey,
                domain: domain)))
            {
        }

        [LuisIntent("Web.Navigate")]
        public async Task WebNavigate(IDialogContext context, LuisResult result)
        {
            await ShowLuisResult(context, result);
        }

        [LuisIntent("Calendar.Find")]
        public async Task CalendarFind(IDialogContext context, LuisResult result)
        {
            var entities = result.Entities;
            for (int i = 0; i < entities.Count; i++)
            {
                await context.PostAsync($"Entity {i}: {entities[i].Entity}, score {entities[i].Score}");
            }
            await ShowLuisResult(context, result);
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task DidntUnderstand(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Your intent was {result.Intents[0].Intent}. But I am not able to do anything for that.");
            context.Done("");
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Done("");
        }
    }
}