using IP_Base;
using NetMQ;
using NetMQ.Sockets;
using NLog;
using MessagePack;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WinFormsQtBridge.Plugin.Common.Models;
using WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces;


namespace WinFormsQtBridge.Plugin.Infrastructure.Services
{
    public class ZeroMqServerService : IZeroMqServerService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ProjectTreeView _project;

        private Task _serverTask;

        public Func<BridgeActionRequest, string?> OnActionReceived { get; set; }

        public ZeroMqServerService(ProjectTreeView project)
        {
            _project = project;
        }

        public void Start()
        {
            Debug.WriteLine("Starting Server");
            _serverTask = Task.Run(() =>
            {
                try
                {
                    using var rep = new ResponseSocket("@tcp://127.0.0.1:5555");
                    while (true)
                    {
                        var requestJson = rep.ReceiveFrameString();
                        
                        Debug.WriteLine($"Message received: {requestJson}");
                        
                        var request = JsonConvert.DeserializeObject<BridgeActionRequest>(requestJson);
                        string? response = null;

                        _project.Invoke((Action)(() => { response = OnActionReceived.Invoke(request); }));

                        var safeResponse = response ?? string.Empty;
                        var responseBytes = MessagePackSerializer.Serialize(safeResponse);
                        rep.SendFrame(responseBytes);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.Error(CultureInfo.InvariantCulture, "Error in ZeroMQ server", ex);
                }
            });
        }
    }
}