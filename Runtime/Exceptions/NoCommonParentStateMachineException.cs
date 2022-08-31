using System;

namespace HFSM {
    public class NoCommonParentStateMachineException : Exception {
        public NoCommonParentStateMachineException(string message) : base(message) { }
    }
}
