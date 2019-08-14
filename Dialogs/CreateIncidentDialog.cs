// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using CoreBot.Cards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using SNOW.Logger;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class IncDialog : CancelAndHelpDialog
    {

        protected readonly IConfiguration Configuration;
        protected readonly ILogger Logger;
        //BookingDetails bookingDetails = new BookingDetails();
        public IncDialog()
            : base(nameof(IncDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
              DestinationStepAsync,
              OriginStepAsync,

            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> DestinationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var bookingDetails = (BookingDetails)stepContext.Options;



            if (bookingDetails.Incident_No == null)
            {
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter incident No:") }, cancellationToken);
            }
            else
            {
                return await stepContext.NextAsync(stepContext, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> OriginStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (BookingDetails)stepContext.Options;
            if (((BookingDetails)stepContext.Options).Incident_No == null)
            {
                bookingDetails.Incident_No = (string)stepContext.Result;
            }


            string concat = "";


            SNOWLogger nOWLoggerforinc = new SNOWLogger(Configuration);


            if ((bookingDetails.Incident_No != null))
            {
                if (!(bookingDetails.Incident_No.Equals("")))


                {


                    CoreBot.models.Incident_api_result incident_Api_Result = nOWLoggerforinc.GetIncident(bookingDetails.Incident_No);

                    CoreBot.models.apiresult apiresult = nOWLoggerforinc.GetIncidentDetails(bookingDetails.Incident_No);



                    if (incident_Api_Result != null && apiresult != null)
                    {
                        if (incident_Api_Result.Result.Count != 0 && apiresult.result.Count != 0)
                        {
                            for (int i = 0; i < incident_Api_Result.Result.Count; i++)
                            {
                                concat += " \n " + apiresult.result[i].number + ":" + apiresult.result[i].short_description + " \n Status: " + incident_Api_Result.Result[i].value + " \n Active:" + apiresult.result[i].active;
                            }


                            // Cards are sent as Attachments in the Bot Framework.
                            // So we need to create a list of attachments for the reply activity.
                            var attachments = new List<Attachment>();

                            // Reply to the activity we received with an activity.
                            var reply = MessageFactory.Attachment(attachments);

                            reply.Attachments.Add(Cards.GetHeroCardforStatus(apiresult.result[0].number, apiresult.result[0].short_description, incident_Api_Result.Result[0].value).ToAttachment());

                            await stepContext.Context.SendActivityAsync(reply, cancellationToken);




                            //await stepContext.Context.SendActivityAsync(MessageFactory.Text(concat), cancellationToken);
                            return await stepContext.EndDialogAsync(null, cancellationToken);
                        }else
                        {
                            if (incident_Api_Result.Result.Count == 0)
                            {
                                await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text(" Incident is not yet updated by team"), cancellationToken);

                                return await stepContext.EndDialogAsync(null, cancellationToken);
                            }
                            else if (apiresult.result.Count == 0)
                            {
                                await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text("No Incident update Found:"), cancellationToken);

                                return await stepContext.EndDialogAsync(null, cancellationToken);

                            }
                            }
                        }
                    else
                    {


                        await stepContext.Context.SendActivityAsync(
                      MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

                        return await stepContext.EndDialogAsync(null, cancellationToken);

                    }


                    //        {

                    //            //                        return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(" Sorry no details found for " + stepContext.Context.Activity.Text) }, cancellationToken);
                    //            if (incident_Api_Result == null)
                    //            {
                    //                await stepContext.Context.SendActivityAsync(
                    //          MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

                    //                return await stepContext.EndDialogAsync(null, cancellationToken);
                    //            }
                    //            else

                    //            if (incident_Api_Result != null)
                    //            {

                    //                if (incident_Api_Result.Result.Count != 0)
                    //                {
                    //                    await stepContext.Context.SendActivityAsync(
                    //                    MessageFactory.Text(" Sorry no update found for: " + incident_Api_Result.Result[0].number.ToString()), cancellationToken);

                    //                    return await stepContext.EndDialogAsync(null, cancellationToken);
                    //                }
                    //                else
                    //                {
                    //                    await stepContext.Context.SendActivityAsync(
                    //                MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

                    //                    return await stepContext.EndDialogAsync(null, cancellationToken);
                    //                }

                    //            }
                    //        }
                    //        return await stepContext.EndDialogAsync(null, cancellationToken);
                    //    }
                    //}
                    //else
                    //{

                    //    await stepContext.Context.SendActivityAsync(
                    //           MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

                    //    return await stepContext.EndDialogAsync(null, cancellationToken);

                    //}
















                    //return await stepContext.EndDialogAsync(null, cancellationToken);

                    //  return await stepContext.ContinueDialogAsync(cancellationToken);

                    // return await stepContext.ReplaceDialogAsync("MainDialog",bookingDetails,cancellationToken);
                }
                
            }
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }


}
