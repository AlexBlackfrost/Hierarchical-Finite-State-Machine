using HFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State {
    private Vector3 initialScale;
    private DeadParams deadParams;
    private float animationElapsedTime;
    private float animationLength;
    private float destroyDelay = 0;
    public event Action Dead;

    [Serializable] public class DeadParams {
        public AnimationCurve animationCurve;
        [field:NonSerialized]public Transform Transform { get; set; }
        [field: NonSerialized] public Action<float> Destroy { get; set; }
    }

    public DeadState(DeadParams deadParams) {
        this.deadParams = deadParams;
        animationLength = deadParams.animationCurve.keys[deadParams.animationCurve.length - 1].time;
    }

    protected override void OnEnter() {
        initialScale = deadParams.Transform.localScale;
        animationElapsedTime = 0;
    }

    protected override void OnUpdate() {
        deadParams.Transform.localScale = deadParams.animationCurve.Evaluate(animationElapsedTime) * initialScale;
        animationElapsedTime += Time.deltaTime;

        if(animationElapsedTime > animationLength) {
            deadParams.Destroy(destroyDelay);
            Dead.Invoke();
        }
    }

}

