using IP_Base;
using NetMQ;
using NetMQ.Sockets;
using NLog;
using MessagePack;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WinFormsQtBridge.Plugin.Common.Models;
using WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces;


namespace WinFormsQtBridge.Plugin.Infrastructure.Services
{
    public class ZeroMqServerService : IZeroMqServerService
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        
        private readonly ProjectTreeView _project;
        
        private Task _serverTask;
        
        public Func<BridgeActionRequest, string> OnActionReceived { get; set; }

        public ZeroMqServerService(ProjectTreeView project)
        {
            _project = project;
        }

        public void Start()
        {
            _serverTask = Task.Run(() =>
            {
                using (var rep = new ResponseSocket("@tcp://127.0.0.1:5555"))
                {
                    while (true)
                    {
                        try
                        {
                            var requestJson = rep.ReceiveFrameString();
                            // _logger.Info("Received request: " + requestJson);
                        
                            var request = JsonConvert.DeserializeObject<BridgeActionRequest>(requestJson);

                            string response = null;

                            _project.Invoke((Action)(() =>
                            {
                                response = OnActionReceived?.Invoke(request);
                            }));
                            
                            var responseBytes = MessagePackSerializer.Serialize(response);
                            rep.SendFrame(responseBytes);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex, "Error in ZeroMQ server");
                        }
                    }
                }
            });
        }
    }
}