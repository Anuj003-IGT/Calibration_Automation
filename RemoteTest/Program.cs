using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Utility
{
    public class RemoteAccessToolHelper
    {
        private Process _remoteAccessTool;

        private static string RemoteAccessDirectoryPath => $@"{Path.GetFullPath(Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\..\"))}Tools";
        private static string RemoteAccessClientToolPath => Path.Combine(RemoteAccessDirectoryPath, "IAC.RemoteAccess.Client.exe");
        private static string RemoteAccessServerToolPath => Path.Combine(RemoteAccessDirectoryPath, "IAC.RemoteAccess.exe");

        public void Start()
        {
            Console.WriteLine("[Start] RemoteAccessServerToolPath: " + RemoteAccessServerToolPath);

            _remoteAccessTool = Process.Start(RemoteAccessServerToolPath);
        }

        public bool StartProcessAndWait(string remoteSourceFilePath, int waitTimeInSeconds, string args)
        {
            Console.WriteLine("[StartProcessAndWait] FilePath: " + remoteSourceFilePath);
            Console.WriteLine("[StartProcessAndWait] Args: " + args);
            return Execute("StartProcessAndWait", $"{remoteSourceFilePath} {waitTimeInSeconds} {args}");
        }

        public bool CopyDirectory(string sourceDirectoryPath, string targetDirectoryPath,
            bool isCopyToTargetPC = true)
        {
            Console.WriteLine("[CopyDirectory] Source: " + sourceDirectoryPath);
            Console.WriteLine("[CopyDirectory] Target: " + targetDirectoryPath);
            var command = isCopyToTargetPC ? "CopyDirectoryToRecursive" : "CopyDirectoryFrom";
            return Execute(command, $"{sourceDirectoryPath} {targetDirectoryPath}");
        }

        public bool CopyFile(string remoteSourceFilePath, string localTargetDirectoryPath)
        {
            Console.WriteLine("[CopyFile] Source: " + remoteSourceFilePath);
            Console.WriteLine("[CopyFile] Target: " + localTargetDirectoryPath);
            return Execute("CopyFileFrom", $"{remoteSourceFilePath} {localTargetDirectoryPath}");
        }

        public bool DeleteFile(string remoteSourceFilePath)
        {
            Console.WriteLine("[DeleteFile] Path: " + remoteSourceFilePath);
            return Execute("DeleteFile", $"{remoteSourceFilePath}");
        }

        public bool CreateDirectory(string remoteSourceFilePath)
        {
            Console.WriteLine("[CreateDirectory] Path: " + remoteSourceFilePath);
            return Execute("CreateDir", $"{remoteSourceFilePath}");
        }

        public bool DeleteDirectory(string remoteSourceFilePath)
        {
            Console.WriteLine("[DeleteDirectory] Path: " + remoteSourceFilePath);
            return Execute("DeleteDir", $"{remoteSourceFilePath}");
        }

        private static bool Execute(string command, string args)
        {
            var fullArgs = new StringBuilder();
            fullArgs.Append(command + " ");
            fullArgs.Append(TestTargetSystem.GetTargetIp());
            fullArgs.Append(" " + args);

            string fullCommandArgs = fullArgs.ToString();

            Console.WriteLine("[Execute] RemoteAccessClientToolPath: " + RemoteAccessClientToolPath);
            Console.WriteLine("[Execute] Full command: " + fullCommandArgs);

            var process = Process.Start(RemoteAccessClientToolPath, fullCommandArgs);
            process?.WaitForExit();

            Console.WriteLine("[Execute] ExitCode: " + process?.ExitCode);

            return process?.ExitCode == 0;
        }

        public void Stop()
        {
            if (_remoteAccessTool == null)
                throw new InvalidOperationException("RemoteAccessTool was never running");
            if (_remoteAccessTool.HasExited)
                throw new InvalidOperationException("RemoteAccessTool exited unexpectedly");

            _remoteAccessTool.Kill();
            _remoteAccessTool = null;
        }
    }
}
