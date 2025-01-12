using DevOpsExmaProject.Mp3Api.Dtos.Mp3Dtos;
using DevOpsExmaProject.Mp3Api.Services.Abstracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DevOpsExmaProject.Mp3Api.Services.Concretes
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ConnectionFactory _factory;
        private const string QueueName = "mp3_comments";

        public RabbitMQService()
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://gbpuukwl:AQ7-D2W_b8-chIj2ObgijZqTY7ctECPd@ostrich.lmq.cloudamqp.com/gbpuukwl")
            };
        }

        public async Task AddMp3Comment(string ownerUserName, int mp3Id, string comment)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = new
            {
                OwnerUserName = ownerUserName,
                Mp3Id = mp3Id,
                Comment = comment,
                Timestamp = DateTime.UtcNow
            };

            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));



        
            var properties = new BasicProperties
            {
                Persistent = true 
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: QueueName,
                basicProperties: properties,
                body: messageBody,
                mandatory: false

                );

            await Task.CompletedTask;
        }


        public async Task<List<Mp3CommentDto>> GetMp3Comments(int mp3Id)
        {
            var comments = new List<Mp3CommentDto>();

            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var deserializedMessage = JsonSerializer.Deserialize<Dictionary<string, object>>(message);

                if (deserializedMessage != null && Convert.ToInt32(deserializedMessage["Mp3Id"]) == mp3Id)
                {
                    comments.Add(new Mp3CommentDto
                    {
                        OwnerUserName = deserializedMessage["OwnerUserName"].ToString(),
                        Comment = deserializedMessage["Comment"].ToString()
                    });
                }

                await Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queue: QueueName, autoAck: true, consumer: consumer);

            await Task.Delay(1000);

            return comments;
        }
    }
}
