using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Internal;
using MQTTnet.Protocol;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using EGG_Haunolding_Management_System.Class;


//HEAVILY WIP!

List<DataItem> DataItems = new();
DataItems.Add(await GetDataItem());
// WIP GetDataItems()
static async Task<DataItem> GetDataItem()
{
    JsonDataItem jsonDataItem = new JsonDataItem();
    DataItem dataItem = new DataItem();
    await Handle_Received_Application_Message(dataItem);
    return dataItem;
}

 static async Task Handle_Received_Application_Message(DataItem dataItem)
{
    var mqttFactory = new MqttFactory();

    using (var mqttClient = mqttFactory.CreateMqttClient())
    {

        // Setup connection w/ ip
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("test.mosquitto.org").Build();


        // Message handling
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine("Received application message.");
            var yeet = JsonSerializer.Deserialize<JsonDataItem>(Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.ToArray()));
            Console.WriteLine(yeet.zeittext);
            dataItem = yeet.ToDataItem();
            Console.WriteLine(dataItem.Time);
            return Task.CompletedTask;
        };

        // TryConnect
        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        // Setup Subscription
        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("zaehlerbroadcast/#");
                })
            .Build();

        // Subsribe to topic
        await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

        Console.WriteLine("MQTT client subscribed to topic.");

        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
    }
}