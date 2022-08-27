using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET;
using Alexa.NET.Profile;

namespace Home.Azure.Functions
{
    public static class AlexaInvoke
    {
        [FunctionName("AlexaInvoke")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);
            return await ProcessRequest(skillRequest);
        }


        private async static Task<IActionResult> ProcessRequest(SkillRequest skillRequest)
        {
            
            var requestType = skillRequest.GetRequestType();
            SkillResponse response = null;
            if (requestType == typeof(LaunchRequest))
            {
                response = ResponseBuilder.Tell("Salut à toi ô mon frère !");
                response.Response.ShouldEndSession = false;
            }
            else if (requestType == typeof(IntentRequest))
            {
                //Scope sc;
                //if(!skillRequest.Session.User.Permissions.Scopes.TryGetValue(CustomerProfilePermissions.FullName, out sc))
                //{
                //    response = ResponseBuilder.TellWithAskForPermissionConsentCard(
                //        "Pour pouvoir vous répondre, je vais avoir besoin de votre autorisation pour accèder à certaines informations",
                //        new[]{
                //            CustomerProfilePermissions.FullName,
                //            CustomerProfilePermissions.GivenName,
                //        }
                //    );
                //    response.Response.ShouldEndSession = false;
                //    return new OkObjectResult(response);
                //}

                //var client = new PersonProfileClient(skillRequest);
                //var fullName = await client.FullName();

                var intentRequest = skillRequest.Request as IntentRequest;
                if (intentRequest.Intent.Name.Equals("verrouiller", StringComparison.InvariantCultureIgnoreCase))
                {
                    response = ResponseBuilder.Tell("C'est parti mon kiki pour : " + intentRequest.Intent.Name);
                    response.Response.ShouldEndSession = false;
                }
                else if (intentRequest.Intent.Name.Equals("etatGeneral", StringComparison.InvariantCultureIgnoreCase))
                {
                    response = ResponseBuilder.Tell("Ca va comme un ... je sais plus, un truc en di ou alors qui commence par di...");
                    response.Response.ShouldEndSession = false;
                }
                else if (intentRequest.Intent.Name.Equals("StartOfWorkday", StringComparison.InvariantCultureIgnoreCase))
                {
                    response = ResponseBuilder.Tell("Bon débarras !");
                    response.Response.ShouldEndSession = false;
                }
                else if (intentRequest.Intent.Name.Equals("ExecuteScenario", StringComparison.InvariantCultureIgnoreCase))
                {
                    var scenario = (from z in intentRequest.Intent.Slots
                                    where z.Key.Equals("scenario", StringComparison.InvariantCultureIgnoreCase)
                                    select z.Value.Value).FirstOrDefault();

                    if (string.IsNullOrEmpty(scenario))
                    {
                        response = ResponseBuilder.Tell("Oh ! mon pote, tu demande un scénario et tu me le dit pas ? C'est quoi ce naze ?");
                        response.Response.ShouldEndSession = false;
                    }
                    else 
                    {
                        response = ResponseBuilder.Tell("J'ai demandé à l'autre bidule de lancer le scénar : " + scenario);
                        response.Response.ShouldEndSession = false;
                    }
                }

            }
            else if (requestType == typeof(SessionEndedRequest))
            {
                response = ResponseBuilder.Tell("Allez, à la revoyure !");
                response.Response.ShouldEndSession = true;
            }
            return new OkObjectResult(response);
        }
    }
}
