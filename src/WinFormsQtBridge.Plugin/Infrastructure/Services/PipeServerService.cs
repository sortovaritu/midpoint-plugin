using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces;

namespace WinFormsQtBridge.Plugin.Infrastructure.Services
{
    public class PipeServerService : IPipeServerService
    {
        private const string PipeName = "WinFormsQtBridge.PipeServerService";
        
        public Func<string, string> OnActionReceived { get; set; }

        public void Start()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    using (var server = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Message))
                    {
                        await server.WaitForConnectionAsync();

                        using (var reader = new StreamReader(server))
                        using (var writer = new StreamWriter(server))
                        {
                            writer.AutoFlush = true;

                            while (server.IsConnected)
                            {
                                var message = await reader.ReadLineAsync();

                                if (message == null)
                                {
                                    break;
                                }

                                var response = OnActionReceived?.Invoke(message);

                                await writer.WriteLineAsync(response);
                            }
                        }
                    }
                }
            });
        }
    }
}