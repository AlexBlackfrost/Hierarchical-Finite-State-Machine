using System;

namespace HFSM {
    public class RootStateMachineNotInitializedException : Exception {
        public RootStateMachineNotInitializedException(string message) : base(message) { }
    }
}
