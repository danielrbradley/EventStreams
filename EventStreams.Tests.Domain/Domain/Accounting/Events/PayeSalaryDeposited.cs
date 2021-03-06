﻿using System;
using System.Runtime.Serialization;

namespace EventStreams.Domain.Accounting.Events {
    [DataContract(Namespace = "")]
    public class PayeSalaryDeposited : Credited {

        [DataMember]
        public string Source { get; private set; }

        public PayeSalaryDeposited() { }

        public PayeSalaryDeposited(decimal takeHomeValue, string source)
            : base(takeHomeValue) {

            Source = source;
        }
    }
}
