using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace SNOW.Logger
{

    public class SNOWLogger
    {
        public SNOWLogger(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        protected readonly IConfiguration Configuration;
        public string CreateIncidentServiceNow(string shortDescription, string description)
        {
            try
            {
                
                string username = Configuration["ServiceNowUserName"];
                
                string password = Configuration["ServiceNowPassword"];
                string url = Configuration["ServiceNowUrl"];

                var auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));

                HttpWebRequest request = WebRequest.Create(url+"incident") as HttpWebRequest;
                request.Headers.Add("Authorization", auth);
                request.Method = "Post";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(new
                    {
                        description = shortDescription + Environment.NewLine + Environment.NewLine + description,
                        short_description = Configuration["ServiceNowTicketShortDescription"],
                        contact_type = Configuration["ServiceNowContactType"],
                        category = Configuration["ServiceNowCategory"],
                        subcategory = Configuration["ServiceNowSubCategory"],
                        assignment_group = Configuration["ServiceNowAssignmentGroup"],
                        impact = Configuration["ServiceNowIncidentImpact"],
                        priority = Configuration["ServiceNowIncidentPriority"],
                        caller_id = Configuration["ServiceNowCallerId"],
                        cmdb_ci = Configuration["ServiceNowCatalogueName"],
                        comments = Configuration["ServiceNowTicketShortDescription"]


                    });

                    streamWriter.Write(json);
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    var res = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    JObject joResponse = JObject.Parse(res.ToString());
                    JObject ojObject = (JObject)joResponse["result"];
                    string incNumber = ((JValue)ojObject.SelectToken("number")).Value.ToString();

                    return incNumber;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        public string KBSearchServiceNow(string shortDescription)
        {
            try
            {

                string username = Configuration["ServiceNowUserName"];

                string password = Configuration["ServiceNowPassword"];
                string url = Configuration["ServiceNowUrl"];

                var auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));

                //HttpWebRequest request = WebRequest.Create(url+"kb_knowledge?") as HttpWebRequest;
                HttpWebRequest request = WebRequest.Create("https://dev84141.service-now.com/api/now/table/kb_knowledge?sysparm_query=GOTO123TEXTQUERY321%3Dlaptop&sysparm_limit=10") as HttpWebRequest;

          //https://dev84141.service-now.com/api/now/table/kb_knowledge?sysparm_query=GOTO123TEXTQUERY321%3Dlaptop&sysparm_limit=10
                request.Headers.Add("Authorization",auth);
                request.Headers.Add("Content-Type","application/json");
                request.Headers.Add("Accept","application/json");
                    
                request.Method = "Post";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(new
                    {
                       // description = shortDescription + Environment.NewLine + Environment.NewLine + description,
                        //short_description = Configuration["ServiceNowTicketShortDescription"],
                        //contact_type = Configuration["ServiceNowContactType"],
                        //category = Configuration["ServiceNowCategory"],
                       // subcategory = Configuration["ServiceNowSubCategory"],
                       // assignment_group = Configuration["ServiceNowAssignmentGroup"],
                       // impact = Configuration["ServiceNowIncidentImpact"],
                       // priority = Configuration["ServiceNowIncidentPriority"],
                       // caller_id = Configuration["ServiceNowCallerId"],
                       // cmdb_ci = Configuration["ServiceNowCatalogueName"],
                        //comments = Configuration["ServiceNowTicketShortDescription"]
                    //   sysparm_query = shortDescription,
                   //    sysparm_fields = "number,short_description,text",
                   //    sysparm_limit = "10"
                    });

                    streamWriter.Write(json);
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    var res = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    JObject joResponse = JObject.Parse(res.ToString());
                    JObject ojObject = (JObject)joResponse["result"];
                    string incNumber = ((JValue)ojObject.SelectToken("number")).Value.ToString();

                    return incNumber;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }




    }
}
