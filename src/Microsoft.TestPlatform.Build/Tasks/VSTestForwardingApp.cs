﻿// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.TestPlatform.Build.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class VSTestForwardingApp
    {
        private const string hostExe = "dotnet";
        private const string vsTestAppName = "vstest.console.dll";
        private readonly List<string> allArgs = new List<string>();

        private bool traceEnabled;

        public VSTestForwardingApp(IEnumerable<string> argsToForward)
        {
            this.allArgs.Add("exec");
            this.allArgs.Add(GetVSTestExePath());
            this.allArgs.AddRange(argsToForward);

            var traceEnabledValue = Environment.GetEnvironmentVariable("VSTEST_TRACE_BUILD");
            this.traceEnabled = !string.IsNullOrEmpty(traceEnabledValue) && traceEnabledValue.Equals("1", StringComparison.OrdinalIgnoreCase);
        }

        public int Execute()
        {
            var processInfo = new ProcessStartInfo
                                  {
                                      FileName = hostExe,
                                      Arguments = string.Join(" ", this.allArgs),
                                      UseShellExecute = false,
                                      CreateNoWindow = true,
                                      RedirectStandardError = true,
                                      RedirectStandardOutput = true
                                  };

            this.Trace("VSTest: Starting vstest.console...");
            this.Trace("VSTest: Arguments: " + processInfo.Arguments);

            using (var process = new Process { StartInfo = processInfo })
            {
                process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
                process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();
                this.Trace("VSTest: Exit code: " + process.ExitCode);
                return process.ExitCode;
            }
        }

        private static string GetVSTestExePath()
        {
            return Path.Combine(AppContext.BaseDirectory, vsTestAppName);
        }

        private void Trace(string message)
        {
            if (this.traceEnabled)
            {
                Console.WriteLine(message);
            }
        }
    }
}