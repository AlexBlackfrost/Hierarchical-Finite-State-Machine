using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HFSM {
    public class StatelessStateMachineException : Exception {
        public StatelessStateMachineException(string message) : base(message) { }
    }
}

