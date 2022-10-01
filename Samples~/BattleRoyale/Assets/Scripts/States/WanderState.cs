using HFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class WanderState : State {
    private WanderParams wanderParams;
    private Bounds groundBounds;
    private Vector3 targetLocation;

    [Serializable] public class WanderParams {
        public GameObject ground;
        [field:NonSerialized] public Transform Transform { get; set; }
        [field: NonSerialized] public CharacterMovement CharacterMovement { get; set; }
    }

    public WanderState(WanderParams wanderParams) {
        this.wanderParams = wanderParams;
        groundBounds = wanderParams.ground.GetComponent<BoxCollider>().bounds;
    }

    protected override void OnEnter() {
       targetLocation = GetRandomLocationInsideBounds();
    }

    protected override void OnUpdate() {
        wanderParams.CharacterMovement.MoveTowards(targetLocation);
    }

    private Vector3 GetRandomLocationInsideBounds() {
        return new Vector3(
            UnityEngine.Random.Range(groundBounds.min.x, groundBounds.max.x),
            wanderParams.Transform.position.y,
            UnityEngine.Random.Range(groundBounds.min.z, groundBounds.max.z)
        );
    }

    public bool HasReachedTargetLocation(float acceptanceRadius) {
        return Vector3.Distance(wanderParams.Transform.position, targetLocation) < acceptanceRadius;
    }

}

