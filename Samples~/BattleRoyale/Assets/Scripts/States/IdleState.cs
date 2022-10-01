using HFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleState : State {
    private float currentIdleTime;
    private float currentMaxIdleTime;
    private Quaternion initialHeadRotation;
    private Quaternion targetHeadRotation;
    private float lerp;
    private IdleParams idleParams;

    [Serializable] public class IdleParams {
        public float minIdleTime = 0.5f;
        public float maxIdleTime = 2f;
        public float headRotationSpeed = 1;
        public float headRotationAngle = 30;
        [field: NonSerialized] public Transform Transform { get; set; }
    }

    public IdleState(IdleParams idleParams) {
        this.idleParams = idleParams;
    }

    protected override void OnEnter() {
        currentIdleTime = 0;
        currentMaxIdleTime = Random.Range(idleParams.minIdleTime, idleParams.maxIdleTime);
        lerp = 0;
        initialHeadRotation = idleParams.Transform.rotation;
        targetHeadRotation = Quaternion.Euler(0f, idleParams.headRotationAngle, 0f) * idleParams.Transform.rotation;
    }

    protected override void OnUpdate() {
        RotateHead();
        currentIdleTime += Time.deltaTime;
    }

    private void RotateHead() {
        lerp += idleParams.headRotationSpeed * Time.deltaTime;
        idleParams.Transform.rotation = Quaternion.Lerp(initialHeadRotation, targetHeadRotation, lerp);

        if(lerp >= 1) { // swap them
            Quaternion aux = targetHeadRotation;
            targetHeadRotation = initialHeadRotation;
            initialHeadRotation = aux;
            lerp = 0;
        }
    }

    public bool IdleTimeFinished() {
        return currentIdleTime > currentMaxIdleTime;
    }
}

