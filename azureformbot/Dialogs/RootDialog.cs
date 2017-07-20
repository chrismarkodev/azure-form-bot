﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace azureformbot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        // Dialog's variables
        private string customerName;
        private string certifierName;

        // Implements IDialog interface and it is called when the dialog becomes active
        // IDIalogContect used to manage the conversation
        public Task StartAsync(IDialogContext context)
        {
            // Wait till first message is received from the conversation and call MessageREceivedAsync to process it.
            context.Wait(MessageReceivedAsync);
            // completes only after the bot process ends.
            return Task.CompletedTask;
        }

        // When called, it's passwed the dialog context and an IAwaitable of type IMessageActivity
        // To get the message, await the result.
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            // int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            // await context.PostAsync($"You sent {activity.Text} which was {length} characters");
            await context.PostAsync("Welcome to CSA Group. I will help you find certification information.");

            context.Wait(MessageReceivedAsync);

        }
    }
}