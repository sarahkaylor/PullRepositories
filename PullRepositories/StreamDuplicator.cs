using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PullRepositories {
    internal class StreamDuplicator : IDisposable {
        private readonly StreamReader _source;
        private readonly TextWriter _dest;
        private readonly Process _owner;
        private readonly Thread _thread;
        private readonly List<Exception> _errors;
        private volatile bool _keepGoing;


        public StreamDuplicator(StreamReader source, TextWriter dest, Process owner) {
            _source = source;
            _dest = dest;
            _owner = owner;
            _keepGoing = true;
            _errors = new List<Exception>();
            _thread = new Thread(DoDuplication) {IsBackground = true};
            _thread.Start();
        }

        private void DoDuplication() {
            try {
                while (KeepGoing()) {
                    var line = _source.ReadLine();
                    if (line == null) {
                        return;
                    }
                    _dest.WriteLine(line);
                }
            } catch (Exception e) {
                _errors.Add(e);
            }
        }

        private bool KeepGoing() {
            Thread.MemoryBarrier();
            return _keepGoing;
        }

        public void Dispose() {
            _keepGoing = false;
            Thread.MemoryBarrier();
            _thread.Join();
            foreach (var error in _errors) {
                throw new Exception(error.Message, error);
            }
        }
    }
}