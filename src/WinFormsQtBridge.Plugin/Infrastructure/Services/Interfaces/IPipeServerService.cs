using System;
using WinFormsQtBridge.Plugin.Common.Models;

namespace WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces
{
    public interface IPipeServerService
    {
        Func<BridgeAction, string> OnActionReceived { get; set; }
        
        void Start();
    }
}