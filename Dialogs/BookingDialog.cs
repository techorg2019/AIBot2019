// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Cards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using SNOW.Logger;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class BookingDialog : CancelAndHelpDialog
    {

        protected readonly IConfiguration Configuration;
        protected readonly ILogger Logger;
        //BookingDetails bookingDetails = new BookingDetails();
        public BookingDialog()
            : base(nameof(BookingDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
              DestinationStepAsync,
              OriginStepAsync,
                TravelDateStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> DestinationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var bookingDetails = (BookingDetails)stepContext.Options;

         

            if (bookingDetails.Short_desc == null)
            {
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter short description of incident:") }, cancellationToken);
            }
            else
            {
                return await stepContext.NextAsync(bookingDetails, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> OriginStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
           var bookingDetails = (BookingDetails)stepContext.Options;

            bookingDetails.Short_desc = (string)stepContext.Result;

            if (bookingDetails.Descrip == null)
            {
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter incident details: ") }, cancellationToken);
            }
            else
            {
                return await stepContext.NextAsync(bookingDetails, cancellationToken);
            }
        }
        private async Task<DialogTurnResult> TravelDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (BookingDetails)stepContext.Options;

            bookingDetails.Descrip = (string)stepContext.Result;

            if (bookingDetails.Priority == null)
            {

                return await stepContext.PromptAsync(nameof(ChoicePrompt),
        new PromptOptions
        {
            Prompt = MessageFactory.Text("Please enter incident priority."),
            Choices = ChoiceFactory.ToChoices(new List<string> { "High", "Medium", "Low" }),
        }, cancellationToken);



            //    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter incident priority: \n 1. High \n 2.Medium \n 3.Low ") }, cancellationToken);
            }
            else
                {
                return await stepContext.NextAsync(bookingDetails, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (BookingDetails)stepContext.Options;

            bookingDetails.Priority = (string)stepContext.Context.Activity.Text;

            //var msg = $"Please confirm incident detail: \n Title: {bookingDetails.Short_desc} \n Description: {bookingDetails.Descrip} \n Priority: {bookingDetails.Priority}";
            //     var msg = $"Are you satisfied with the input? ";

            var attachments = new List<Attachment>();

            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);

            reply.Attachments.Add(Cards.GetHeroCardConfirm(bookingDetails.Short_desc, bookingDetails.Descrip, bookingDetails.Priority).ToAttachment());

            await stepContext.Context.SendActivityAsync(reply, cancellationToken);




            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please confirm if I can create this incident for you ?", "Confirmation","Confirmation") }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
               var bookingDetails = (BookingDetails)stepContext.Options;

                SNOWLogger nOWLogger1 = new SNOWLogger(Configuration);

                string incident1 = nOWLogger1.CreateIncidentServiceNow(bookingDetails.Short_desc, bookingDetails.Descrip, bookingDetails.GetPrioritycode(bookingDetails.Priority) );


                if (incident1 != null)
                {


                    // Cards are sent as Attachments in the Bot Framework.
                    // So we need to create a list of attachments for the reply activity.
                    var attachments = new List<Attachment>();

                    // Reply to the activity we received with an activity.
                    var reply = MessageFactory.Attachment(attachments);

                    reply.Attachments.Add(Cards.GetHeroCard(incident1, bookingDetails.Short_desc,bookingDetails.Descrip,bookingDetails.Priority).ToAttachment());

                    await stepContext.Context.SendActivityAsync(reply, cancellationToken);





                  //  await stepContext.Context.SendActivityAsync(
                  //MessageFactory.Text("Incident No: "+ incident1+" Created for: "+bookingDetails.Short_desc+ "\n is there anything I can help you with ?", null,null));
                    stepContext.Context.TurnState.Add("incID", bookingDetails);

                    return await stepContext.EndDialogAsync(bookingDetails, cancellationToken);
                }
                //   stepContext = null;
                return await stepContext.CancelAllDialogsAsync();
               // return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(
                  MessageFactory.Text("Incient creation aborted ! \n is there anything I can help you with ? ", null, null));
               // stepContext = null;


                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
        }

        private static bool IsAmbiguous(string timex)
        {
            var timexProperty = new TimexProperty(timex);
            return !timexProperty.Types.Contains(Constants.TimexTypes.Definite);
        }






           }

}
