using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace Bot_Application5.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // //calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            // // return our reply to the user
            // await context.PostAsync($"You sent {activity.Text} which was {length} characters");
            // context.Wait(MessageReceivedAsync);

            // return;

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            Activity replyToConversation = activity.CreateReply("Should go to conversation, with a hero card");
            replyToConversation.Recipient = activity.From;
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();

            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "https://<ImageUrl1>"));

            List<CardAction> cardButtons = new List<CardAction>();

            CardAction plButton = new CardAction()
            {
                Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                Type = "openUrl",
                Title = "WikiPedia Page"
            };
            cardButtons.Add(plButton);

            HeroCard plCard = new HeroCard()
            {
                Title = "I'm a hero card",
                Subtitle = "Pig Latin Wikipedia Page",
                Images = cardImages,
                Buttons = cardButtons
            };

            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);

            var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);

            context.Wait(MessageReceivedAsync);
        }
    }
}