using EGG_Haunolding_Magement_System.Class;
using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client;
using MQTTnet.Client.Internal;
using MQTTnet.Protocol;



// HEAVILY WIP!


DataItem dataItem = new DataItem();
//await Connect_Client();
//await Handle_Received_Application_Message();

// static async Task Connect_Client()
//{
//    var mqttFactory = new MqttFactory();

//    using (var mqttClient = mqttFactory.CreateMqttClient())
//    {
//        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("test.mosquitto.org").Build();


//        var response = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

//        Console.WriteLine("The MQTT client is connected.");

//        var mqttClientDisconnectOptions = mqttFactory.CreateClientDisconnectOptionsBuilder().Build();

//        await mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);
//    }
//}
//static async Task Handle_Received_Application_Message()
//{
//    /*
//     * This sample subscribes to a topic and processes the received message.
//     */

//    var mqttFactory = new MqttFactory();

//    using (var mqttClient = mqttFactory.CreateMqttClient())
//    {
//        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("test.mosquitto.org").Build();

//        // Setup message handling before connecting so that queued messages
//        // are also handled properly. When there is no event handler attached all
//        // received messages get lost.
//        mqttClient.ApplicationMessageReceivedAsync += e =>
//        {
//            Console.WriteLine("Received application message.");
//            //e.DumpToConsole();

//            return Task.CompletedTask;
//        };

//        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

//        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
//            .WithTopicFilter(
//                f =>
//                {
//                    f.WithTopic("zaehlerbroadcast");
//                })
//            .Build();

//        await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

//        Console.WriteLine("MQTT client subscribed to topic.");

//        Console.WriteLine("Press enter to exit.");
//        Console.ReadLine();
//    }
//static async Task Main(string[] args)
//{
//    var factory = new MqttFactory();
//    var client = factory.CreateMqttClient();
//    var options = new MqttClientOptionsBuilder().WithTcpServer("localhost", 1883).WithClientId("mqtt_consumer").Build();
//    client.UseConnectedHandler(async e => {
//        Console.WriteLine("Connected to MQTT broker.");
//        var topicFilter = new MqttTopicFilterBuilder().WithTopic("test/topic").Build();
//        await client.SubscribeAsync(new MqttClientSubscribeOptionsBuilder().WithTopicFilter(topicFilter).Build());
//    });
//    client.UseDisconnectedHandler(async e => {
//        Console.WriteLine("Disconnected from MQTT broker.");
//        await Task.Delay(TimeSpan.FromSeconds(5));
//        try
//        {
//            await client.ConnectAsync(options, CancellationToken.None);
//        }
//        catch
//        {
//            Console.WriteLine("Reconnecting to MQTT broker failed.");
//        }
//    });
//    client.UseApplicationMessageReceivedHandler(e => {
//        Console.WriteLine($"Received message on topic '{e.ApplicationMessage.Topic}': {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
//    });
//    try
//    {
//        await client.ConnectAsync(options, CancellationToken.None);
//    }
//    catch
//    {
//        Console.WriteLine("Connecting to MQTT broker failed.");
//    }
//    Console.ReadLine();
//}