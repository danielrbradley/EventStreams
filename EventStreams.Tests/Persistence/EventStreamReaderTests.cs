﻿using System;
using System.Linq;
using System.IO;

using NUnit.Framework;

namespace EventStreams.Persistence {
    using Serialization.Events;
    using Resources;

    [TestFixture]
    internal class EventStreamReaderTests {

        [Test]
        public void Given_first_set_when_read_back_then_object_model_is_as_expected() {
            using (var ms = new MemoryStream()) {
                ResourceProvider.AppendTo(ms, "First.e");
                ms.Position = 0;

                using (var esr = new EventStreamReader(ms, new NullEventReader())) {
                    var firstExpected = MockEventStreams.First.ElementAt(0);
                    var firstActual = esr.Next();

                    Assert.AreEqual(firstActual.Id, firstExpected.Id);
                    Assert.AreEqual(firstActual.Timestamp, firstExpected.Timestamp);
                    Assert.AreEqual(firstActual.Arguments.GetType(), firstExpected.Arguments.GetType());

                    var secondExpected = MockEventStreams.First.ElementAt(1);
                    var secondActual = esr.Next();

                    Assert.AreEqual(secondActual.Id, secondExpected.Id);
                    Assert.AreEqual(secondActual.Timestamp, secondExpected.Timestamp);
                    Assert.AreEqual(secondActual.Arguments.GetType(), secondExpected.Arguments.GetType());
                }
            }
        }
    }
}