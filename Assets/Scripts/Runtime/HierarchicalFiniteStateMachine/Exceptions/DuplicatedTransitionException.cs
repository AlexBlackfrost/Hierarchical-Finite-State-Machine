using System;

public class DuplicatedTransitionException : Exception {

    public DuplicatedTransitionException() : base() { }
    public DuplicatedTransitionException(string message) : base(message) { }
}
