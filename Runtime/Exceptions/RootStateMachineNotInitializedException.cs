using System;

namespace HFSM {
    /// <summary>
    /// Thrown when a <see cref="StateMachine"/> has been executed without initializing it first, that is,
    /// calling <see cref="StateMachine.Update"/>, <see cref="StateMachine.FixedUpdate"/> or 
    /// <see cref="StateMachine.LateUpdate"/> without calling <see cref="StateMachine.Init"/> first.
    /// </summary>
    public class RootStateMachineNotInitializedException : Exception {
        public RootStateMachineNotInitializedException(string message) : base(message) { }
    }
}
