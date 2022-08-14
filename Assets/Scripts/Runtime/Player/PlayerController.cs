using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    private void Start(){
        State run = new RunState();
        StateMachine grounded = new GroundedStateMachine(run);
        StateMachine root = new RootStateMachine(this, grounded, run);

        
    }

    private void Update(){
        
    }
}
