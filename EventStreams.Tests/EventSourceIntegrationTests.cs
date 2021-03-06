﻿using System;
using System.IO;
using System.Linq;

using NUnit.Framework;

namespace EventStreams {
    using Domain.Accounting;
    using Persistence.FileSystem;
    using Persistence.Serialization.Events;

    [TestFixture]
    public class EventSourceIntegrationTests {

        private RepositoryHierarchy _repositoryHierarchy;
        private EventSource _eventSource;
        private readonly Guid _a = new Guid("a2d06e1b-a311-45c7-9097-d288d61a8c33");
        private readonly Guid _b = new Guid("1c8854ec-6399-4464-ade8-3a0e9c914b6c");

        [SetUp]
        public void Setup() {
            _repositoryHierarchy = new RepositoryHierarchy("C:\\EventStreams\\test-" + Guid.NewGuid());
            _eventSource = new EventSource(new FileSystemPersistenceStrategy(_repositoryHierarchy, EventReaderWriterPair.Json));
        }

        [TearDown]
        public void Teardown() {
            Directory.Delete(_repositoryHierarchy.RootPath, true);
        }

        [Test]
        public void Create_a_brand_new_stream_and_issue_some_commands() {
            using (var ba = _eventSource.Create<BankAccount>(_a)) {
                ba.Credit(150);
                ba.DepositPayeSalary(1500, "Acme Corp");
                ba.Purchase(20, "Steak");
                ba.Purchase(10, "Spam");
                ba.Purchase(10, "Noodles");

                Assert.That(ba.Identity == _a);
                Assert.That(ba.Balance == 1610);
            }
        }

        [Test]
        public void Open_a_stream_with_a_specific_identifier_and_issue_a_command() {
            Create_a_brand_new_stream_and_issue_some_commands();

            using (var ba = _eventSource.Open<BankAccount>(_a)) {
                ba.Purchase(5, "Broadband");

                Assert.That(ba.Identity == _a);
                Assert.AreEqual(1605, ba.Balance);
            }
        }

        [Test]
        public void Open_or_create_a_stream_with_a_specific_identifier_and_issue_a_command() {
            using (var ba = _eventSource.OpenOrCreate<BankAccount>(_b)) {
                Assert.AreEqual(ba.Identity, _b);
                ba.Credit(40);
                ba.Purchase(40, "Sky+");

                Assert.That(ba.Identity == _b);
                Assert.AreEqual(0, ba.Balance);
            }
        }

        [Test]
        public void Read_a_stream_with_a_specific_identifier_into_a_particular_read_model() {
            Create_a_brand_new_stream_and_issue_some_commands();

            var ba = _eventSource.Read<PurchasesMadeOnBankAccount>(_a);

            Assert.AreEqual("Steak", ba.ItemNames.ElementAt(0));
            Assert.AreEqual("Spam", ba.ItemNames.ElementAt(1));
            Assert.AreEqual("Noodles", ba.ItemNames.ElementAt(2));
            Assert.AreEqual(40, ba.TotalValue);
        }
    }
}
