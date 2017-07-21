using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace azureformbot.Dialogs
{
    [Serializable]
    public class NameDialog : IDialog<string>
    {
        private int attempts = 3;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("what is your name ?");
            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // check if message is valid, name is OK an dreturn to calling dialog
            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {
                // completes the dialog, removes it from the dialog stack and returns the result to the calling/parent dialog
                context.Done(message.Text);
            }
            // prompt user again if message invalid
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I'm sorry, but I don't understand you reply. What is your name ?");
                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    // finsih with failure current dialog, remove it from dialog stack and return exception to the calling/parent dialog
                    context.Fail(new TooManyAttemptsException("message was not a string or it was empty."));
                }
            }
        }
    }
}