using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WriterStateMachineOne : StateMachine {

    private StringBuilder stringBuilder;
    public WriterStateMachineOne(StringBuilder stringBuilder, params StateObject[] states) : base(states) {
        this.stringBuilder = stringBuilder;
    }

    protected override void OnUpdate() {
        stringBuilder.Append(GetType() + HFSMTest.Update);
    }
    protected override void OnFixedUpdate() {
        stringBuilder.Append(GetType() + HFSMTest.FixedUpdate);
    }
    protected override void OnLateUpdate() {
        stringBuilder.Append(GetType() + HFSMTest.LateUpdate);
    }
    protected override void OnEnter() {
        stringBuilder.Append(GetType() + HFSMTest.Enter);
    }

    protected override void OnExit() {
        stringBuilder.Append(GetType() + HFSMTest.Exit);
    }
}

