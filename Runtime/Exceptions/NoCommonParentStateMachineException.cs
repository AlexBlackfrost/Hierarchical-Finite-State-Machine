using System;

namespace HFSM {
    /// <summary>
    /// Thrown when <see cref="StateObject"/>s of a <see cref="Transition"/> do not share a common
    /// <see cref="StateMachine"/> ancestor, that is, a common parent state machine in the hierarchy.
    /// </summary>
    public class NoCommonParentStateMachineException : Exception {
        public NoCommonParentStateMachineException(string message) : base(message) { }
    }
}
