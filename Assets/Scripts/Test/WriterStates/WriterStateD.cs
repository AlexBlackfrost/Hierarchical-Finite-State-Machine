using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WriterStateD : State {
    private StringBuilder stringBuilder;

    public WriterStateD(StringBuilder stringBuilder) {
        this.stringBuilder = stringBuilder;
    }

    protected override void OnUpdate() {
        stringBuilder.Append(GetType() + " Update");
    }
    protected override void OnFixedUpdate() {
        stringBuilder.Append(GetType() + " FixedUpdate");
    }
    protected override void OnLateUpdate() {
        stringBuilder.Append(GetType() + " LateUpdate");
    }
    protected override void OnEnter() {
        stringBuilder.Append(GetType() + " Enter");
    }

    protected override void OnExit() {
        stringBuilder.Append(GetType() + " Exit");
    }
}

