using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Host
{
    sealed class CaptiveProcess : IDisposable
    {
        readonly bool _captureOutput;
        readonly string _exeDirectory;
        readonly Process _process;
        readonly ManualResetEvent _outputComplete = new ManualResetEvent(false);
        readonly ManualResetEvent _errorComplete = new ManualResetEvent(false);

        readonly object _sync = new object();
        readonly StringWriter _output = new StringWriter();

        public CaptiveProcess(
            string fullExePath,
            string args = null,
            IDictionary<string, string> environment = null,
            bool captureOutput = true)
        {
            if (fullExePath == null) throw new ArgumentNullException(nameof(fullExePath));
            _captureOutput = captureOutput;

            _exeDirectory = Path.GetDirectoryName(fullExePath);

            var startInfo = new ProcessStartInfo
                      {
                          StandardOutputEncoding = new UTF8Encoding(false),
                          StandardErrorEncoding = new UTF8Encoding(false),
                          UseShellExecute = false,
                          RedirectStandardError = captureOutput,
                          RedirectStandardOutput = captureOutput,
                          WindowStyle = ProcessWindowStyle.Hidden,
                          CreateNoWindow = true,
                          ErrorDialog = false,
                          FileName = fullExePath,
                          Arguments = args ?? ""
                      };
            
            if (environment != null)
            {
                foreach (var (key, value) in environment)
                {
                    startInfo.Environment.Add(key, value);
                }
            }

            _process = Process.Start(startInfo);
            if (_process == null)
                throw new InvalidOperationException("The process did not start.");

            if (captureOutput)
            {
                _process.OutputDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                        _outputComplete.Set(); 
                    else
                        WriteOutput(e.Data);
                };
                _process.BeginOutputReadLine();

                _process.ErrorDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                        _errorComplete.Set();
                    else
                        WriteOutput(e.Data);
                };
                _process.BeginErrorReadLine();
            }
        }

        void WriteOutput(string o)
        {
            lock (_sync)
                _output.WriteLine(o);
        }

        public string Output
        {
            get
            {
                lock (_sync)
                    return _output.ToString();
            }
        }

        public int WaitForExit(TimeSpan? timeout = null)
        {
            var processExitTimeout = timeout ?? Timeout.InfiniteTimeSpan;     
            _process.WaitForExit((int)processExitTimeout.TotalMilliseconds);

            if (_captureOutput)
            {
                if (!_outputComplete.WaitOne(TimeSpan.FromSeconds(30)))
                    throw new IOException("STDOUT did not complete in the fixed 30 second window.");
                
                if (!_errorComplete.WaitOne(TimeSpan.FromSeconds(30)))
                    throw new IOException("STDERR did not complete in the fixed 30 second window.");
            }

            return _process.ExitCode;
        }

        public string GetLocalPath(string path)
        {
            return Path.IsPathRooted(path) ? path : Path.Combine(_exeDirectory, path);
        }

        public void Dispose()
        {
            try
            {
                _process.Kill();
                WaitForExit();
            }
            catch
            {
                // Ignored
            }
        }
    }
}
