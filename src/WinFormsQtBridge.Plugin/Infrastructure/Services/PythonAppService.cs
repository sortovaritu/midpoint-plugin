using System.Diagnostics;
using IP_Base;

namespace WinFormsQtBridge.Plugin.Infrastructure.Services
{
    public class PythonAppService
    {
        private void StartPythonApp()
        {
            const string pythonExe = @"C:\Repos\python-winform\.venv\Scripts\python.exe";
            const string scriptPath = @"C:\Repos\python-winform\main.py";
            
            if (!System.IO.File.Exists(pythonExe))
            {
                IP_Message.Error("Unable to find python.exe file");
                return;
            }

            if (!System.IO.File.Exists(scriptPath))
            {
                IP_Message.Error("Unable to find script file main.py");
                return;
            }

            var psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = $"\"{scriptPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = Process.Start(psi);
            process.BeginOutputReadLine();
        }
    }
}