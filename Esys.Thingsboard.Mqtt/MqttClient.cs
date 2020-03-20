using Esys.Thingsboard.Mqtt.Api;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esys.Thingsboard.Mqtt
{
    public class MqttClient : IDisposable
    {
        private readonly IMqttClientFactory factory;

        public MqttClient(IMqttClientFactory factory = null) => this.factory = factory ?? new MqttFactory();

        public string AccessToken { get; set; }

        public string Server { get; set; }

        public int? Port { get; set; }

        public bool UseTls { get; set; }

        private readonly SemaphoreSlim clientLock = new SemaphoreSlim(1, 1);
        private IManagedMqttClient client;

        public async Task StartAsync()
        {
            if (disposedValue) throw new InvalidOperationException("Client is disposed.");
            await clientLock.WaitAsync();
            try
            {
                var options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
                    .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithClientId(Guid.NewGuid().ToString())
                        .WithTcpServer(Server, Port)
                        .WithCredentials(AccessToken)
                        .WithTls(new MqttClientOptionsBuilderTlsParameters { UseTls = UseTls })
                        .Build())
                    .Build();
                client = factory.CreateManagedMqttClient();
                await client.StartAsync(options);
            }
            finally
            {
                clientLock.Release();
            }
        }

        public async Task StopAsync()
        {
            if (disposedValue) throw new InvalidOperationException("Client is disposed.");
            await clientLock.WaitAsync();
            try
            {
                if (client == null)
                    return;
                await client.StopAsync();
            }
            finally
            {
                clientLock.Release();
            }
        }

        public async Task SendAsync(IMessage message)
        {
            if (disposedValue) throw new InvalidOperationException("Client is disposed.");
            await clientLock.WaitAsync();
            try
            {
                if (client == null)
                    throw new InvalidOperationException($"{nameof(MqttClient)} has to be started to send messages.");
                var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(message.Topic)
                    .WithPayload(JsonConvert.SerializeObject(message))
                    //.WithExactlyOnceQoS()     // Fails with exception and disconnect!
                    .Build();
                await client.PublishAsync(mqttMessage);
            }
            finally
            {
                clientLock.Release();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
                    clientLock.Wait();
                    try
                    {
                        client.Dispose();
                    }
                    finally
                    {
                        clientLock.Release();
                    }
                }

                // TODO: nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer weiter unten überschreiben.
                // TODO: große Felder auf Null setzen.

                disposedValue = true;
            }
        }

        // TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
        // ~MqttClient()
        // {
        //   // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
        //   Dispose(false);
        // }

        // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        public void Dispose() =>
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
            Dispose(true);// TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.// GC.SuppressFinalize(this);
        #endregion
    }
}
