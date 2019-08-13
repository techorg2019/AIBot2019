// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
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

                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter incident priority: 1. High  \n 2.Medium  \n 3.Low ") }, cancellationToken);
            }
            else
                {
                return await stepContext.NextAsync(bookingDetails, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (BookingDetails)stepContext.Options;

            bookingDetails.Priority = (string)stepContext.Result;

            var msg = $"Please confirm incident detail:\n Title: {bookingDetails.Short_desc} Description: {bookingDetails.Descrip} Priority {bookingDetails.Priority}";
       //     var msg = $"Are you satisfied with the input? ";

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = MessageFactory.Text(msg) }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
               var bookingDetails = (BookingDetails)stepContext.Options;

                SNOWLogger nOWLogger1 = new SNOWLogger(Configuration);

                string incident1 = nOWLogger1.CreateIncidentServiceNow(bookingDetails.Short_desc, bookingDetails.Descrip, bookingDetails.Priority);


                if (incident1 != null)
                {


                   

                    await stepContext.Context.SendActivityAsync(
                  MessageFactory.Text("Incident No: "+ incident1+" Created for: "+bookingDetails.Short_desc,null,null));
                }
             //   stepContext = null;
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(
                  MessageFactory.Text("Incient creation aborted !", null, null));
                stepContext = null;
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
