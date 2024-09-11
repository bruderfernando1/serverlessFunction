using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ServerlessFunction
{
    public class Somar
    {
        [FunctionName("Somar")]
        public async Task RunAsync([ServiceBusTrigger("somar", Connection = "ServiceBusConn")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"Processar a mensagem: {myQueueItem}");

            ServiceBus sb = new ServiceBus();

            var message = JsonConvert.DeserializeObject<MessageModel>(myQueueItem);

            try
            {
                ResultModel result = new ResultModel();
                result.value1 = message.value1;
                result.value2 = message.value2;
                result.result = message.value1 + message.value2;
                result.type = message.type;

                await sb.SendMessageAsync(JsonConvert.SerializeObject(result),"Resultado Soma", "resultado");
            } catch(Exception ex)
            {
                await sb.SendErrorMessageAsync(myQueueItem, ex.Message);
            }

            log.LogInformation($"Mensagem processada");
        }
    }
}
