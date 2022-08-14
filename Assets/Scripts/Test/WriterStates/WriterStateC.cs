using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WriterStateC : State {
    private StringBuilder stringBuilder;

    public WriterStateC(StringBuilder stringBuilder) {
        this.stringBuilder = stringBuilder;
    }

    protected override void OnUpdate() {
        stringBuilder.Append(GetType() + " Update");
    }

    protected override void OnEnter() {
        stringBuilder.Append(GetType() + " Enter");
    }

    protected override void OnExit() {
        stringBuilder.Append(GetType() + " Exit");
    }
}

