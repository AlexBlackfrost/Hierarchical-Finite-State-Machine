using System;

public class DuplicatedTransitionException : Exception {
    public DuplicatedTransitionException(string message) : base(message) { }
}
