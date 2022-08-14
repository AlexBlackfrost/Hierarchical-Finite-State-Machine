using System;

public class RootStateMachineNotInitializedException : Exception {
    
    public RootStateMachineNotInitializedException() : base() { }
    public RootStateMachineNotInitializedException(string message) : base(message) { }
}

