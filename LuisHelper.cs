// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.BotBuilderSamples.Dialogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.BotBuilderSamples
{
    public static class LuisHelper
    {
     //   public static object RecognizerResult { get; private set; }

        public static async Task<BookingDetails> ExecuteLuisQuery(IConfiguration configuration, ILogger logger, ITurnContext turnContext, /*RecognizerResult luisresult,*/ CancellationToken cancellationToken)
        {
            var bookingDetails = new BookingDetails();


          //  RecognizerResult = luisresult;

            try
            {
                // Create the LUIS settings from configuration.
                var luisApplication = new LuisApplication(
                    configuration["LuisAppIdForSnow"],
                    configuration["LuisAPIKey"],
                    configuration["LuisAPIHostNameforiter"]
                  // $"https://{configuration["LuisAPIHostName"]}.api.cognitive.microsoft.com"))
                );





                var recognizer = new LuisRecognizer(luisApplication);

                // The actual call to LUIS
                var recognizerResult = await recognizer.RecognizeAsync(turnContext, cancellationToken);


              //  var intent = luisresult.GetTopScoringIntent();

                //  var (intent, score) = luisresult.GetTopScoringIntent();

                 var (intent, score) = recognizerResult.GetTopScoringIntent();
                if (intent == "Book_flight")
                {
                    // We need to get the result from the LUIS JSON which at every level returns an array.
                    bookingDetails.Destination = recognizerResult.Entities["To"]?.FirstOrDefault()?["Airport"]?.FirstOrDefault()?.FirstOrDefault()?.ToString();
                    bookingDetails.Origin = recognizerResult.Entities["From"]?.FirstOrDefault()?["Airport"]?.FirstOrDefault()?.FirstOrDefault()?.ToString();

                    // This value will be a TIMEX. And we are only interested in a Date so grab the first result and drop the Time part.
                    // TIMEX is a format that represents DateTime expressions that include some ambiguity. e.g. missing a Year.
                    bookingDetails.TravelDate = recognizerResult.Entities["datetime"]?.FirstOrDefault()?["timex"]?.FirstOrDefault()?.ToString().Split('T')[0];
                }

                if (intent == "create_incident")
                {

                    bookingDetails.Create_incident = "true";
                    
                    // We need to get the result from the LUIS JSON which at every level returns an array.
                //    bookingDetails.Destination = recognizerResult.Entities["To"]?.FirstOrDefault()?["Airport"]?.FirstOrDefault()?.FirstOrDefault()?.ToString();
                  //  bookingDetails.Origin = recognizerResult.Entities["From"]?.FirstOrDefault()?["Airport"]?.FirstOrDefault()?.FirstOrDefault()?.ToString();

                    // This value will be a TIMEX. And we are only interested in a Date so grab the first result and drop the Time part.
                    // TIMEX is a format that represents DateTime expressions that include some ambiguity. e.g. missing a Year.
                    //bookingDetails.TravelDate = recognizerResult.Entities["datetime"]?.FirstOrDefault()?["timex"]?.FirstOrDefault()?.ToString().Split('T')[0];


                  //  BeginDialogAsync(nameof(BookingDialog), bookingDetails, cancellationToken);
                }
                else
                {
                    bookingDetails.Create_incident = "false";

                }

                if (intent == "None")
                {

                    bookingDetails.None = "true";

                    
                }
                else
                {
                    bookingDetails.None = "false";

                }

                if (intent == "incident_status")
                {

                    bookingDetails.Incident_status = "true";

                    bookingDetails.Incident_No = recognizerResult.Entities["incident_no"]?.FirstOrDefault().ToString();


                }
                else
                {
                    bookingDetails.Incident_status = "false";

                }


            }
            catch (Exception e)
            {
                logger.LogWarning($"LUIS Exception: {e.Message} Check your LUIS configuration.");
            }

            return bookingDetails;
        }
    }
}
