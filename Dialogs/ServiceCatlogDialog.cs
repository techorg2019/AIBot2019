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
using Microsoft.Recognizers.Text;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class ServiceCatlogDialog : CancelAndHelpDialog
    {

        protected readonly IConfiguration Configuration;
        protected readonly ILogger Logger;
        public CoreBot.models.Incident_api_result incident_Api_Result = null;

        public CoreBot.models.apiresult apiresult = null;
        //BookingDetails bookingDetails = new BookingDetails();
        public ServiceCatlogDialog()
            : base(nameof(ServiceCatlogDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
             // DestinationStepAsync,
              OriginStepAsync,
              Commment_ascalateStepConfirmAsync,
           //   Commment_ascalateStepAsync,

            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        //private async Task<DialogTurnResult> DestinationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{

        //    var bookingDetails = (BookingDetails)stepContext.Options;



        //    if (bookingDetails.Incident_No == null)
        //    {
        //        return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter incident No:") }, cancellationToken);
        //    }
        //    else
        //    {
        //        return await stepContext.NextAsync(stepContext, cancellationToken);
        //    }
        //}

        private async Task<DialogTurnResult> OriginStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            string catlog1 = "1.AWS(type:t2.nano.Image:centos7)";
            string catlog2 = "2.AWS(type:t2.small.Image:centos7)";

            return await stepContext.PromptAsync(nameof(ChoicePrompt),
              new PromptOptions
              {
                  Prompt = MessageFactory.Text("Ok, Please select the request to be raised from the available catlog."),
                  Choices = ChoiceFactory.ToChoices(new List<string> { catlog1, catlog2 }),
                  Style = ListStyle.SuggestedAction
                  // Choices = ChoiceFactory.ToChoices(new List<string> { "Add my comment for SNOW team", "Escalate this issue", "I am Satisfied" }),
              }, cancellationToken);





            //var bookingDetails = (BookingDetails)stepContext.Options;
            //if (((BookingDetails)stepContext.Options).Incident_No == null)
            //{
            //    bookingDetails.Incident_No = (string)stepContext.Result.ToString().ToUpper();
            //}


            //string concat = "";


            //SNOWLogger nOWLoggerforinc = new SNOWLogger(Configuration);


            //if ((bookingDetails.Incident_No != null))
            //{
            //    if (!(bookingDetails.Incident_No.Equals("")))


            //    {


            //        incident_Api_Result = nOWLoggerforinc.GetIncident(bookingDetails.Incident_No.ToUpper());

            //       apiresult = nOWLoggerforinc.GetIncidentDetails(bookingDetails.Incident_No.ToUpper());



            //        if (incident_Api_Result != null && apiresult != null)
            //        {
            //            if (incident_Api_Result.Result.Count != 0 && apiresult.result.Count != 0)
            //            {
            //                for (int i = 0; i < incident_Api_Result.Result.Count; i++)
            //                {
            //                    concat += " \n " + apiresult.result[i].number + ":" + apiresult.result[i].short_description + " \n Status: " + incident_Api_Result.Result[i].value + " \n Active:" + apiresult.result[i].active;
            //                }


            //                // Cards are sent as Attachments in the Bot Framework.
            //                // So we need to create a list of attachments for the reply activity.
            //                var attachments = new List<Attachment>();

            //                // Reply to the activity we received with an activity.
            //                var reply = MessageFactory.Attachment(attachments);

            //                reply.Attachments.Add(Cards.GetHeroCardforStatus(apiresult.result[0].number, apiresult.result[0].short_description, incident_Api_Result.Result[0].value).ToAttachment());

            //                await stepContext.Context.SendActivityAsync(reply, cancellationToken);



            //                return await stepContext.PromptAsync(nameof(ChoicePrompt),
            //        new PromptOptions
            //        {
            //            Prompt = MessageFactory.Text("Are you satisfied ? if not please select."),
            //            Choices = ChoiceFactory.ToChoices(new List<string> { "comment", "Escalate", "Satisfied" }),
            //        }, cancellationToken);

            //                //await stepContext.Context.SendActivityAsync(MessageFactory.Text(concat), cancellationToken);
            //                //  return await stepContext.EndDialogAsync(null, cancellationToken);
            //              //  return await stepContext.NextAsync(stepContext, cancellationToken);

            //            }
            //            else
            //            {
            //                if (incident_Api_Result.Result.Count == 0)
            //                {
            //                    await stepContext.Context.SendActivityAsync(
            //              MessageFactory.Text(" Incident is not yet updated by team"), cancellationToken);

            //                    //  return await stepContext.EndDialogAsync(null, cancellationToken);
            //                    //return await stepContext.NextAsync(stepContext, cancellationToken);

            //                    return await stepContext.PromptAsync(nameof(ChoicePrompt),
            //  new PromptOptions
            //  {
            //      Prompt = MessageFactory.Text("Are you satisfied ? if Not please select."),
            //      Choices = ChoiceFactory.ToChoices(new List<string> { "Comment", "Escalate", "Satisfied" }),
            //     // Choices = ChoiceFactory.ToChoices(new List<string> { "Add my comment for SNOW team", "Escalate this issue", "I am Satisfied" }),
            //  }, cancellationToken);





            //                }
            //                else if (apiresult.result.Count == 0)
            //                {
            //                    await stepContext.Context.SendActivityAsync(
            //              MessageFactory.Text("No Incident update Found:"), cancellationToken);

            //                    //   return await stepContext.EndDialogAsync(null, cancellationToken);
            //                  //  return await stepContext.NextAsync(stepContext, cancellationToken);



            //                }
            //                }
            //            }
            //        else
            //        {


            //            await stepContext.Context.SendActivityAsync(
            //          MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

            //            //  return await stepContext.EndDialogAsync(null, cancellationToken);
            //            return await stepContext.NextAsync(stepContext, cancellationToken);

            //        }


            //        //        {

            //        //            //                        return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(" Sorry no details found for " + stepContext.Context.Activity.Text) }, cancellationToken);
            //        //            if (incident_Api_Result == null)
            //        //            {
            //        //                await stepContext.Context.SendActivityAsync(
            //        //          MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

            //        //                return await stepContext.EndDialogAsync(null, cancellationToken);
            //        //            }
            //        //            else

            //        //            if (incident_Api_Result != null)
            //        //            {

            //        //                if (incident_Api_Result.Result.Count != 0)
            //        //                {
            //        //                    await stepContext.Context.SendActivityAsync(
            //        //                    MessageFactory.Text(" Sorry no update found for: " + incident_Api_Result.Result[0].number.ToString()), cancellationToken);

            //        //                    return await stepContext.EndDialogAsync(null, cancellationToken);
            //        //                }
            //        //                else
            //        //                {
            //        //                    await stepContext.Context.SendActivityAsync(
            //        //                MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

            //        //                    return await stepContext.EndDialogAsync(null, cancellationToken);
            //        //                }

            //        //            }
            //        //        }
            //        //        return await stepContext.EndDialogAsync(null, cancellationToken);
            //        //    }
            //        //}
            //        //else
            //        //{

            //        //    await stepContext.Context.SendActivityAsync(
            //        //           MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

            //        //    return await stepContext.EndDialogAsync(null, cancellationToken);

            //        //}
















            //        //return await stepContext.EndDialogAsync(null, cancellationToken);

            //        //  return await stepContext.ContinueDialogAsync(cancellationToken);

            //        // return await stepContext.ReplaceDialogAsync("MainDialog",bookingDetails,cancellationToken);
            //    }

            //}


            //return await stepContext.EndDialogAsync(null, cancellationToken);
            //return await stepContext.NextAsync(stepContext, cancellationToken);
        }









        private async Task<DialogTurnResult> Commment_ascalateStepConfirmAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (BookingDetails)stepContext.Options;

            bookingDetails.catlog_option = (string)stepContext.Context.Activity.Text.ToLower();



            if (bookingDetails.catlog_option.Contains("1.aws(type:t2.nano.image:centos7)"))
            {

                await stepContext.Context.SendActivityAsync(
                                   MessageFactory.Text("Thanks Request has been raised for AWS(type:t2.nano.Image:centos7)"), cancellationToken);
                await stepContext.Context.SendActivityAsync(
                                   MessageFactory.Text("Request No: RITM0010384"), cancellationToken);
                await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Your request has been sent for approval.You will receive an email with VM credentials post approval.") }, cancellationToken);

                await stepContext.Context.SendActivityAsync(
                                  MessageFactory.Text("Anything else I can help you with?"), cancellationToken);
                return await stepContext.EndDialogAsync(null, cancellationToken);
                //  return await stepContext.NextAsync(stepContext, cancellationToken);
            }
            else if (bookingDetails.catlog_option.Contains("2.aws(type:t2.small.image:centos7)"))
            {
                await stepContext.Context.SendActivityAsync(
                                     MessageFactory.Text("Thanks Request has been raised for AWS(type:t2.small.Image:centos7)"), cancellationToken);
                await stepContext.Context.SendActivityAsync(
                                   MessageFactory.Text("Request No: RITM0010385"), cancellationToken);

                await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Your request has been sent for approval.You will receive an email with VM credentials post approval.") }, cancellationToken);

                await stepContext.Context.SendActivityAsync(
                                  MessageFactory.Text("Anything else I can help you with?"), cancellationToken);
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            
            
               // return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Anything else I can help you with? ") }, cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);


        }



        //private async Task<DialogTurnResult> Commment_ascalateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    var bookingDetails = (BookingDetails)stepContext.Options;
        //    if (((BookingDetails)stepContext.Options).Coment_for_Team == null)
        //    {
        //        bookingDetails.Coment_for_Team = (string)stepContext.Result;
        //    }


        //    string concat = "";


        //    SNOWLogger nOWLoggerforinc = new SNOWLogger(Configuration);


        //    if ((bookingDetails.Incident_No != null))
        //    {
        //        if (!(bookingDetails.Incident_No.Equals("")))


        //        {

        //            incident_Api_Result = nOWLoggerforinc.GetIncident(bookingDetails.Incident_No.ToUpper());
        //            string incident_Api_Result1 = nOWLoggerforinc.CommentONIncident(bookingDetails.Incident_No.ToUpper(), bookingDetails.Coment_for_Team);

        //            CoreBot.models.apiresult apiresult = nOWLoggerforinc.GetIncidentDetails(bookingDetails.Incident_No.ToUpper());



        //            if (incident_Api_Result != null && apiresult != null && incident_Api_Result1 !=null)
        //            {
        //                if (incident_Api_Result.Result.Count != 0 && apiresult.result.Count != 0)
        //                {
        //                    for (int i = 0; i < incident_Api_Result.Result.Count; i++)
        //                    {
        //                        concat += " \n " + apiresult.result[i].number + ":" + apiresult.result[i].short_description + " \n Status: " + incident_Api_Result.Result[i].value + " \n Active:" + apiresult.result[i].active;
        //                    }


        //                    // Cards are sent as Attachments in the Bot Framework.
        //                    // So we need to create a list of attachments for the reply activity.
        //                    var attachments = new List<Attachment>();

        //                    // Reply to the activity we received with an activity.
        //                    var reply = MessageFactory.Attachment(attachments);

        //                    reply.Attachments.Add(Cards.GetHeroCardforStatusUpdate(apiresult.result[0].number, incident_Api_Result.Result[0].value, bookingDetails.Coment_for_Team).ToAttachment());

        //                    await stepContext.Context.SendActivityAsync(reply, cancellationToken);




        //                    //await stepContext.Context.SendActivityAsync(MessageFactory.Text(concat), cancellationToken);
        //                    return await stepContext.EndDialogAsync(null, cancellationToken);
        //                }
        //                else
        //                {
        //                    if (incident_Api_Result.Result.Count == 0)
        //                    {
        //                        await stepContext.Context.SendActivityAsync(
        //                  MessageFactory.Text(" Incident is not updated to team"), cancellationToken);

        //                        return await stepContext.EndDialogAsync(null, cancellationToken);
        //                    }
        //                    else if (apiresult.result.Count == 0)
        //                    {
        //                        await stepContext.Context.SendActivityAsync(
        //                  MessageFactory.Text("Incident update Fail:"), cancellationToken);

        //                        return await stepContext.EndDialogAsync(null, cancellationToken);

        //                    }
        //                }
        //            }
        //            else
        //            {


        //                await stepContext.Context.SendActivityAsync(
        //              MessageFactory.Text(" Sorry no incident found to update"), cancellationToken);

        //                return await stepContext.EndDialogAsync(null, cancellationToken);

        //            }


        //            //        {

        //            //            //                        return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(" Sorry no details found for " + stepContext.Context.Activity.Text) }, cancellationToken);
        //            //            if (incident_Api_Result == null)
        //            //            {
        //            //                await stepContext.Context.SendActivityAsync(
        //            //          MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

        //            //                return await stepContext.EndDialogAsync(null, cancellationToken);
        //            //            }
        //            //            else

        //            //            if (incident_Api_Result != null)
        //            //            {

        //            //                if (incident_Api_Result.Result.Count != 0)
        //            //                {
        //            //                    await stepContext.Context.SendActivityAsync(
        //            //                    MessageFactory.Text(" Sorry no update found for: " + incident_Api_Result.Result[0].number.ToString()), cancellationToken);

        //            //                    return await stepContext.EndDialogAsync(null, cancellationToken);
        //            //                }
        //            //                else
        //            //                {
        //            //                    await stepContext.Context.SendActivityAsync(
        //            //                MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

        //            //                    return await stepContext.EndDialogAsync(null, cancellationToken);
        //            //                }

        //            //            }
        //            //        }
        //            //        return await stepContext.EndDialogAsync(null, cancellationToken);
        //            //    }
        //            //}
        //            //else
        //            //{

        //            //    await stepContext.Context.SendActivityAsync(
        //            //           MessageFactory.Text(" Sorry no incident update found"), cancellationToken);

        //            //    return await stepContext.EndDialogAsync(null, cancellationToken);

        //            //}
















        //            //return await stepContext.EndDialogAsync(null, cancellationToken);

        //            //  return await stepContext.ContinueDialogAsync(cancellationToken);

        //            // return await stepContext.ReplaceDialogAsync("MainDialog",bookingDetails,cancellationToken);
        //        }

        //    }
        //    return await stepContext.EndDialogAsync(null, cancellationToken);
        //}







    }


}
