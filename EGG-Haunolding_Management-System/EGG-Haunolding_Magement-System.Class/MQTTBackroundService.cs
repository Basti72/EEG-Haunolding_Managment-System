using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace EGG_Haunolding_Management_System.Class
{
    public class MQTTBackroundService : BackgroundService
    {
        private readonly IMqttClient _mqttClient;
        private readonly MqttFactory _mqttFactory;
        private readonly IMQTTCom _mqttCom;

        public MQTTBackroundService(IMQTTCom mqttCom)
        {
            _mqttFactory = new MqttFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            _mqttCom = mqttCom;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("test.mosquitto.org").Build();
            // Message handling
            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                try
                {
                    Console.WriteLine("Received application message.");
                    var response = JsonSerializer.Deserialize<JsonDataItem>(Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.ToArray()));
                    var dataItem = response.ToDataItem(e.ApplicationMessage.Topic);
                    _mqttCom.InsertIntoDatabase(dataItem);
                    Console.WriteLine($"Item inserted: Origin={dataItem.Origin} | Time={dataItem.Time} | Saldo={dataItem.Saldo} | SaldoAvg={dataItem.SaldoAvg}");
                }
                catch (Exception ex)
                {

                }
                    
                return Task.CompletedTask;
            };

            // TryConnect
            await _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            // Setup Subscription
            var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(
                    f =>
                    {
                            f.WithTopic($"zaehlerbroadcast/#");
                    })
                .Build();

            // Subsribe to topic
            await _mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

            Console.WriteLine("MQTT client subscribed to topic.");

            //Console.WriteLine("Press enter to exit.");
            //Console.ReadLine();
        }
    }
}