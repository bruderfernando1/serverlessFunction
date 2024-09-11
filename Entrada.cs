using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ServerlessFunction
{
    public static class Entrada
    {
        [FunctionName("Entrada")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Mensagem recebida");
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ServiceBus sb = new ServiceBus();

            try
            {
                var message = JsonConvert.DeserializeObject<MessageModel>(requestBody);                
                await sb.SendMessageAsync(requestBody,"Entrada", message.type);
            } catch(Exception ex)
            {
                await sb.SendErrorMessageAsync(requestBody,ex.Message);
            }

            log.LogInformation("Mensagem encaminhada");
            return new OkObjectResult("Mensagem encaminhada");
        }
    }
}
