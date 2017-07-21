using System;
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

        // Implements IDialog interface and it is called when the root dialog becomes active
        // IDIalogContext used to manage the conversation
        public Task StartAsync(IDialogContext context)
        {
            // Wait till first message is received from the conversation and call MessageReceivedAsync to process it.
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

            await this.SendWelcomeMessageAsync(context);
            //context.Wait(MessageReceivedAsync);

        }

        private async Task SendWelcomeMessageAsync(IDialogContext context) {
            await context.PostAsync("Welcome to CSA Group. I will help you find certification information.");
            context.Call(new CertifierDialog(), this.CertifierDialogResumeAfter);
        }

        private async Task CertifierDialogResumeAfter(IDialogContext context, IAwaitable<string> result) {
            try
            {
                this.certifierName = await result;
                context.Call(new CustomerDialog(this.certifierName), this.CustomerDialogResumeAfter);
            }
            catch (TooManyAttemptsException) {
                await context.PostAsync("I'm sorry, I can't understand you. Let's try again. ");
                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task CustomerDialogResumeAfter(IDialogContext context, IAwaitable<string> result) {
            try
            {
                this.customerName = await result;
                await context.PostAsync($"Your name is { this.certifierName } and the customer is { this.customerName}. I am searching for relevant documents");
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("OK, this is going nowhere. Let's try again.");
            }
            finally
            {
                await this.SendWelcomeMessageAsync(context);
            }
        }
    }
}