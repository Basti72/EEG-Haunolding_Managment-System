using EGG_Haunolding_Magement_System.Class;
using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Internal;
using MQTTnet.Protocol;



//HEAVILY WIP!

await Connect_Client();
await Handle_Received_Application_Message();
static async Task Connect_Client()
{
    /*
     * This sample creates a simple MQTT client and connects to a public broker.
     *
     * Always dispose the client when it is no longer used.
     * The default version of MQTT is 3.1.1.
     */

    var mqttFactory = new MqttFactory();

    using (var mqttClient = mqttFactory.CreateMqttClient())
    {
        // Use builder classes where possible in this project.
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("test.mosquitto.org").Build();

        // This will throw an exception if the server is not available.
        // The result from this message returns additional data which was sent 
        // from the server. Please refer to the MQTT protocol specification for details.
        var response = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        Console.WriteLine("The MQTT client is connected.");

       // response.DumpToConsole();

        // Send a clean disconnect to the server by calling _DisconnectAsync_. Without this the TCP connection
        // gets dropped and the server will handle this as a non clean disconnect (see MQTT spec for details).
        var mqttClientDisconnectOptions = mqttFactory.CreateClientDisconnectOptionsBuilder().Build();

        await mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);
    }
}
static async Task Handle_Received_Application_Message()
{
    /*
     * This sample subscribes to a topic and processes the received message.
     */

    var mqttFactory = new MqttFactory();

    using (var mqttClient = mqttFactory.CreateMqttClient())
    {
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("test.mosquitto.org").Build();

        // Setup message handling before connecting so that queued messages
        // are also handled properly. When there is no event handler attached all
        // received messages get lost.
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine("Received application message.");
            //e.DumpToConsole();
            //e.

            return Task.CompletedTask;
        };

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("zeahlerbroadcast/#");
                })
            .Build();

        var result = mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        foreach(var message in result.Result.Items)
        {
            Console.WriteLine(message.ResultCode);
        }
        Console.WriteLine("MQTT client subscribed to topic.");

        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
    }
}
