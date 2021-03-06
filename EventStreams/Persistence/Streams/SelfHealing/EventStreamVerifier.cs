﻿using System;
using System.Diagnostics;
using System.IO;

namespace EventStreams.Persistence.Streams.SelfHealing {
    using Decorators;
    using Serialization.Events;

    public class EventStreamVerifier : IEventStreamVerifier {

        public static readonly Func<Stream, IEventReader, IEventStreamVerifier> Factory =
            (innerStream, eventReader) => new EventStreamVerifier(innerStream, eventReader);

        private readonly Stream _innerStream;
        private readonly IEventReader _eventReader;

        public EventStreamVerifier(Stream innerStream, IEventReader eventReader) {
            if (innerStream == null) throw new ArgumentNullException("innerStream");
            if (eventReader == null) throw new ArgumentNullException("eventReader");

            _innerStream = innerStream;
            _eventReader = eventReader;

            Debug.Assert(innerStream.CanRead);
            Debug.Assert(innerStream.CanWrite);
            Debug.Assert(innerStream.CanSeek);
        }

        public void Verify() {
            _innerStream.Position = 0;

            using (var esr = new EventStreamReader(_innerStream.PreventClosure(), _eventReader)) {
                while (_innerStream.Position < _innerStream.Length) {
                    try {
                        esr.Next();

                    } catch (TruncationVerificationPersistenceException x) {
                        // A trailing commit was unfinished possibly due to power cut or system crash.
                        // This can be repaired easily just by truncating the stream.
                        _innerStream.SetLength(x.Offset);
                    }
                }
            }

            _innerStream.Position = _innerStream.Length;
        }
    }
}