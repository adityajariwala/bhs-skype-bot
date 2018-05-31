using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace BHSkypeBot
{
    public class LuisBot : IBot
    {

        public async Task OnTurn(ITurnContext context)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var luisAppId = "2da737f8-f8a5-4e0e-9a04-3a173322e204";
            var subscriptionKey = "3d19c5ccff8c4141820fa01beafa7246";

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // This bot is only handling Messages
            if (context.Activity.Type == ActivityTypes.Message)
            {
                // Get the conversation state from the turn context
                var state = context.GetConversationState<EchoState>();

                // Bump the turn count. 
                state.TurnCount++;

                queryString["q"] = context.Activity.Text;
                queryString["timezoneOffset"] = "-480";
                queryString["verbose"] = "false";
                queryString["spellCheck"] = "false";
                queryString["staging"] = "false";

                var uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + luisAppId + "?" + queryString;
                var response = await client.GetAsync(uri);
                var strResponseContent = await response.Content.ReadAsStringAsync();
                dynamic ans = JsonConvert.DeserializeObject(strResponseContent);
                int index = 1;

                StringBuilder sb = new StringBuilder();
                sb.Append(
                    $"Query: {ans.query}\n" +
                    $"Top Intent: {ans.topScoringIntent.intent}\n" +
                    $"Top Intent Score: {ans.topScoringIntent.score}\n"
                );

                foreach (dynamic o in ans.entities)
                {
                    sb.Append(
                     $"Entity {index}: {o.entity}\n" +
                     $"Entity {index} Type: {o.type}\n" +
                     $"Entity {index} Score: {o.score}\n"
                    );
                    index++;
                }

                sb.Append(
                    $"Sentiment: {ans.sentimentAnalysis.label}\n" +
                    $"Sentiment Score: {ans.sentimentAnalysis.score}"
                );

                await context.SendActivity(sb.ToString());
            }
        }
    }
}
