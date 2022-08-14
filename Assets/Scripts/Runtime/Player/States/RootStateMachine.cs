using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootStateMachine : StateMachine {
    private PlayerController PlayerController {get; set;}
    public RootStateMachine(PlayerController playerController, params StateObject[] states):base(states) {
        PlayerController = playerController;

    }

    public RootStateMachine(params StateObject[] states) : base(states) {
        PlayerController = null;

    }

    protected override void OnUpdate() {

    }
}

