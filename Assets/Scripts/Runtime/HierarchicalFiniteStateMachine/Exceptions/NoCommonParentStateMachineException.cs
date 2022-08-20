using System;
public class NoCommonParentStateMachineException : Exception {

    public NoCommonParentStateMachineException() : base() { }
    public NoCommonParentStateMachineException(string message) : base(message) { }
}
