using DevOpsExmaProject.Mp3WebApi.Dtos.Mp3Dtos;
using DevOpsExmaProject.Mp3WebApi.Services.Abstracts;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace DevOpsExmaProject.Mp3WebApi.Services.Concretes
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ConnectionFactory _factory;
        private readonly IConfiguration _configuration;
        private const string QueueName = "mp3_comments";

        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;

            _factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"]!,
                Port = int.Parse(_configuration["RabbitMQ:Port"]!),
                UserName = _configuration["RabbitMQ:UserName"]!,
                Password = _configuration["RabbitMQ:Password"]!
            };
        }

        public async Task AddMp3Comment(string ownerUserName, int mp3Id, string comment)
        {
            try
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
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd")
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
                    mandatory: false);

                Console.WriteLine("Message successfully added to queue.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddMp3Comment: {ex.Message}");
                throw new Exception("An error occurred while adding the comment to RabbitMQ.");
            }
        }

        public async Task<List<Mp3CommentDto>> GetMp3Comments(int mp3Id)
        {
            var comments = new List<Mp3CommentDto>();

            try
            {
                using var connection = await _factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                while (true)
                {
                    var result = await channel.BasicGetAsync(QueueName, autoAck: false);

                    if (result == null)
                    {
                        Console.WriteLine("No more messages in the queue.");
                        break;
                    }

                    var body = result.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    try
                    {
                        var deserializedMessage = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);

                        if (deserializedMessage != null &&
                            deserializedMessage["Mp3Id"].GetInt32() == mp3Id)
                        {
                            comments.Add(new Mp3CommentDto
                            {
                                OwnerUserName = deserializedMessage["OwnerUserName"].GetString(),
                                Comment = deserializedMessage["Comment"].GetString(),
                                DateTime = deserializedMessage["Timestamp"].GetString()
                            });
                        }

                        //await channel.BasicAckAsync(result.DeliveryTag, multiple: false); // bu sadece test ucunu
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"Error deserializing message: {jsonEx.Message}");
                    }
                }

                return comments;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMp3Comments: {ex.Message}");
                throw new Exception("An error occurred while fetching comments from RabbitMQ.");
            }
        }


    }
}
