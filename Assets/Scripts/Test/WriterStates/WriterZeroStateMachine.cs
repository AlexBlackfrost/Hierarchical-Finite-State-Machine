using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WriterZeroStateMachine : StateMachine {

    private StringBuilder stringBuilder;
    public WriterZeroStateMachine(StringBuilder stringBuilder, params StateObject[] states) : base(states) {
        this.stringBuilder = stringBuilder;
    }

    protected override void OnUpdate() {
        stringBuilder.Append(GetType()+ " Update");
    }

    protected override void OnEnter() {
        stringBuilder.Append(GetType() + " Enter");
    }

    protected override void OnExit() {
        stringBuilder.Append(GetType() + " Exit");
    }
}

