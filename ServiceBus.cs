using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace ServerlessFunction
{
    public class ServiceBus()
    {
        public async Task SendMessageAsync(string message, string subject, string topic)
        {
            ServiceBusClient client;
            ServiceBusSender sender;

            try
            {
                var clientOptions = new ServiceBusClientOptions()
                {
                    TransportType = ServiceBusTransportType.AmqpWebSockets
                };
                client = new ServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConn", EnvironmentVariableTarget.Process), clientOptions);
                sender = client.CreateSender(topic);


                ServiceBusMessage msg = new ServiceBusMessage();
                msg.Subject = subject;
                msg.TimeToLive = TimeSpan.FromSeconds(3600);
                msg.PartitionKey = topic;
                msg.ContentType = "application/json";
                msg.Body = BinaryData.FromString(message);
                await sender.SendMessageAsync(msg);

                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
            catch (Exception ex)
            {
                await SendErrorMessageAsync(message, ex.Message);
            }
        }

        public async Task SendErrorMessageAsync(string message, string errorMessage)
        {
            ServiceBusClient client;
            ServiceBusSender sender;
            
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            client = new ServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConn", EnvironmentVariableTarget.Process), clientOptions);
            sender = client.CreateSender("erro");


            ServiceBusMessage msg = new ServiceBusMessage();
            msg.Subject = errorMessage;
            msg.TimeToLive = TimeSpan.FromSeconds(3600);
            msg.PartitionKey = "Erro";
            msg.ContentType = "application/json";
            msg.Body = BinaryData.FromString(message);
            await sender.SendMessageAsync(msg);

            await sender.DisposeAsync();
            await client.DisposeAsync();            
        }
    }
}
