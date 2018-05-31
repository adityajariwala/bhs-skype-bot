using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHSkypeBot.BotActions
{
    public class LinkCard
    {
        public IMessageActivity SendLinkCard(dynamic response)
        {
            IMessageActivity message = Activity.CreateMessageActivity();
            message.TextFormat = "plain";
            message.Locale = "en-Us";

            List<CardAction> cardButtons = new List<CardAction>();

            CardAction ca = new CardAction
            {
                Title = "Launch Link",
                Type = "openUrl",
                Value = $"{response.entities[0].entity}"
            };

            cardButtons.Add(ca);

            ThumbnailCard plCard = new ThumbnailCard()
            {
                Title = $"Hello! Would you like to open:",
                Subtitle = $"{response.entities[0].entity}",
                Buttons = cardButtons
            };

            Attachment acAttach = plCard.ToAttachment();
            message.Attachments.Add(acAttach);

            return message;
        }
    }
}
