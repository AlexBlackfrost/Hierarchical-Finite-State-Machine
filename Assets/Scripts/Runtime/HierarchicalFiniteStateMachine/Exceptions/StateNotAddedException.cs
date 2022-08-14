using System;

public class StateNotAddedException : Exception {

    public StateNotAddedException() : base() { }
    public StateNotAddedException(string message) : base(message) { }
}
