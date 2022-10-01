using HFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootStateMachine : StateMachine {
    public RootStateMachine(params StateObject[] stateObjects) : base(stateObjects) {
    }
}

