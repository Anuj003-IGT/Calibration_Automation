#region (C) Copyright 2024 Philips Medical Systems Nederland B.V.
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written permission of the copyright owner.
//
#endregion

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
            _remoteAccessTool = Process.Start(RemoteAccessServerToolPath);
        }

        public bool StartProcessAndWait(string remoteSourceFilePath, int waitTimeInSeconds, string args)
        {
            return Execute("StartProcessAndWait", $"{remoteSourceFilePath} {waitTimeInSeconds.ToString()} {args}");
        }

        public bool CopyDirectory(string sourceDirectoryPath, string targetDirectoryPath,
            bool isCopyToTargetPC = true)
        {
            var command = isCopyToTargetPC ? "CopyDirectoryToRecursive" : "CopyDirectoryFrom";
            return Execute(command, $"{sourceDirectoryPath} {targetDirectoryPath}");
        }

        public bool CopyFile(string remoteSourceFilePath, string localTargetDirectoryPath)
        {
            return Execute("CopyFileFrom", $"{remoteSourceFilePath} {localTargetDirectoryPath}");
        }

        public bool DeleteFile(string remoteSourceFilePath)
        {
            return Execute("DeleteFile", $"{remoteSourceFilePath}");
        }

        public bool CreateDirectory(string remoteSourceFilePath)
        {
            return Execute("CreateDir", $"{remoteSourceFilePath}");
        }

        public bool DeleteDirectory(string remoteSourceFilePath)
        {
            return Execute("DeleteDir", $"{remoteSourceFilePath}");
        }

        private static bool Execute(string command, string args)
        {
            var sb = new StringBuilder();
            sb.Append(command + " ");
            sb.Append(TestTargetSystem.GetTargetIp());
            sb.Append(" " + args);

            var p = sb.ToString();
            var process = Process.Start(RemoteAccessClientToolPath, p);
            process?.WaitForExit();
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
