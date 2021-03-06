﻿using System;
using System.Collections.Generic;
using System.IO;
using EventStreams.Core;
using EventStreams.Persistence.Serialization.Events;

namespace EventStreams.Persistence.Streams.SelfHealing {

    public class EventStreamSelfHealingWriter : IEventStreamWriter {
        private readonly IEventStreamWriter _innerWriter;
        private readonly Func<Stream, IEventReader, IEventStreamVerifier> _eventStreamVerifierFactory;

        public EventStreamSelfHealingWriter(IEventStreamWriter innerWriter)
            : this(innerWriter, EventStreamVerifier.Factory) { }

        public EventStreamSelfHealingWriter(IEventStreamWriter innerWriter, Func<Stream, IEventReader, IEventStreamVerifier> eventStreamVerifierFactory) {
            if (innerWriter == null) throw new ArgumentNullException("innerWriter");
            if (eventStreamVerifierFactory == null) throw new ArgumentNullException("eventStreamVerifierFactory");
            _innerWriter = innerWriter;
            _eventStreamVerifierFactory = eventStreamVerifierFactory;
        }

        public Stream InnerStream { get { return _innerWriter.InnerStream; } }
        public IEventWriter EventWriter { get { return _innerWriter.EventWriter; } }

        public void Write(IEnumerable<IStreamedEvent> streamedEvents) {
            try {
                // ReSharper disable PossibleMultipleEnumeration
                _innerWriter.Write(streamedEvents);
                // ReSharper restore PossibleMultipleEnumeration

            } catch (DataVerificationPersistenceException) {
                // Attempt to repair the corruption
                // by running a full stream verification.
                _eventStreamVerifierFactory(InnerStream, EventWriter.Opposite)
                    .Verify();

                try {
                    // ReSharper disable PossibleMultipleEnumeration
                    _innerWriter.Write(streamedEvents);
                    // ReSharper restore PossibleMultipleEnumeration

                } catch (DataVerificationPersistenceException x) {
                    throw new IrreparableCorruptionPersistenceException(x);
                }
            }
        }

        public void Dispose() {
            _innerWriter.Dispose();
        }
    }
}