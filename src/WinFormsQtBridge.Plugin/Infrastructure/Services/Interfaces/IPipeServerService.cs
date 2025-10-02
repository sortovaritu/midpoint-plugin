using System;

namespace WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces
{
    public interface IPipeServerService
    {
        Func<string, string> OnActionReceived { get; set; }
        
        void Start();
    }
}