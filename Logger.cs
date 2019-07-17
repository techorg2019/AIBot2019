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

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Headers.Add("Authorization", auth);
                request.Method = "Post";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(new
                    {
                        description = shortDescription + Environment.NewLine + Environment.NewLine + description,
                        // short_description = Configuration["ServiceNowTicketShortDescription"],
                        short_description = shortDescription,
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
    }
}
