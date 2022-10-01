using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HFSM {
    /// <summary>
    /// Thrown when a <see cref="StateMachine"/> has been created without passsing any
    /// <see cref="StateObject"/> as argument.
    /// </summary>
    public class StatelessStateMachineException : Exception {
        public StatelessStateMachineException(string message) : base(message) { }
    }
}

