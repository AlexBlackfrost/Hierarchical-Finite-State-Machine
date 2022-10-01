using HFSM;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WriterStateMachineZero : StateMachine {

    private StringBuilder stringBuilder;
    public WriterStateMachineZero(StringBuilder stringBuilder, params StateObject[] states) : base(states) {
        this.stringBuilder = stringBuilder;
    }

    protected override void OnUpdate() {
        stringBuilder.Append(GetType() + HFSMTest.UpdateLogText);
    }
    protected override void OnFixedUpdate() {
        stringBuilder.Append(GetType() + HFSMTest.FixedUpdateLogText);
    }
    protected override void OnLateUpdate() {
        stringBuilder.Append(GetType() + HFSMTest.LateUpdateLogText);
    }
    protected override void OnEnter() {
        stringBuilder.Append(GetType() + HFSMTest.EnterLogText);
    }
    protected override void OnExit() {
        stringBuilder.Append(GetType() + HFSMTest.ExitLogText);
    }

}

