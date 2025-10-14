using System;
using WinFormsQtBridge.Plugin.Common.Models;

namespace WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces
{
    public interface IZeroMqServerService
    {
        Func<BridgeActionRequest, string> OnActionReceived { get; set; }
        
        void Start();
    }
}