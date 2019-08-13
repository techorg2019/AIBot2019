// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CoreBot.models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Microsoft.BotBuilderSamples
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
           

           //Callschedule callschedule = new Callschedule();

            //callschedule.Callschedulenow();



            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((logging) =>
                {
                    logging.AddDebug();
                    logging.AddConsole();

                })
                .UseStartup<Startup>();
                


        

        private class Callschedule
        {


          
            public void Callschedulenow()
            {


                // For Interval in Seconds 
                // This Scheduler will start at 11:10 and call after every 15 Seconds
                // IntervalInSeconds(start_hour, start_minute, seconds)
                DateTime now1 = DateTime.Now;
                DateTime x1MinsLater = now1.AddMinutes(1);
                MyScheduler.IntervalInSeconds(now1.Hour, x1MinsLater.Minute, 55,
                () =>
                {
                    Debug.WriteLine("calling Service now");
                    Debug.WriteLine("Current Time:" + now1.Hour + ":" + now1.Minute + ":" + now1.Second);
                    Debug.WriteLine("added5Minute:" + x1MinsLater.Minute);

                    
                    Trace.WriteLine("calling Service now");
                    Trace.WriteLine("Current Time:" + now1.Hour + ":" + now1.Minute + ":" + now1.Second);
                    Trace.WriteLine("added5Minute:" + x1MinsLater.Minute);
                    //Debug.WriteLine("sec"+now1.Second);
                    //Debug.WriteLine("sec+1"+now1.Second+1);



                    // string username = Configuration["ServiceNowUserName"];
                    //string password = Configuration["ServiceNowPassword"];
                    //string url = Configuration["ServiceNowUrl"];
                    var auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("admin" + ":" + "Passw0rd!"));



                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create("https://dev84141.service-now.com/api/now/table/kb_knowledge?sysparm_query=GOTO123TEXTQUERY321=laptop&sysparm_fields=number,short_description,text&sysparm_limit=10") as HttpWebRequest;

                    //https://dev84141.service-now.com/api/now/table/kb_knowledge?sysparm_query=GOTO123TEXTQUERY321%3Dlaptop&sysparm_limit=10
                    request.Headers.Add("Authorization", auth);
                    request.Headers.Add("Content-Type", "application/json");
                    request.Headers.Add("Accept", "application/json");
                    request.Method = "Get";

                    using (System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse)
                    {
                        var res = new StreamReader(response.GetResponseStream()).ReadToEnd();


                        apiresult rootObject = JsonConvert.DeserializeObject<apiresult>(res);
                    

                    if (rootObject.result.Count!=0)
                    {
                            DateTime now = DateTime.Now;
                            Debug.WriteLine("Service now is awaked:" + now.Hour + ":" + now.Minute + ":" + now.Second);
                            Trace.WriteLine("Service now is awaked:" + now.Hour + ":" + now.Minute + ":" + now.Second);
                        }
                    else

                    {

                            Debug.WriteLine("Service now is sleeping");
                            Trace.WriteLine("Service now is sleeping");
                        }


                }

                });
                // For Interval in Minutes 
                // This Scheduler will start at 22:00 and call after every 30 Minutes
                // IntervalInSeconds(start_hour, start_minute, minutes)
                //MyScheduler.IntervalInMinutes(22, 00, 30,
                //() => {
                //    Console.WriteLine("//here write the code that you want to schedule");
                //});
                //// For Interval in Hours 
                // This Scheduler will start at 9:44 and call after every 1 Hour
                // IntervalInSeconds(start_hour, start_minute, hours)
                //MyScheduler.IntervalInHours(9, 44, 1,
                //() => {
                //    Console.WriteLine("//here write the code that you want to schedule");
                //});
                //// For Interval in Seconds 
                // This Scheduler will start at 17:22 and call after every 3 Days
                // IntervalInSeconds(start_hour, start_minute, days)
                //MyScheduler.IntervalInDays(17, 22, 3,
                //() => {
                //    Console.WriteLine("//here write the code that you want to schedule");
                //});
                //  Console.ReadLine();

            }
        }
    }
}
