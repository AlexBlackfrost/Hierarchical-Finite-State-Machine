using HFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State {

    private ChaseParams chaseParams;
    [Serializable] public class ChaseParams {
        [field: SerializeField] public List<Species> PreySpecies { get; private set; }
        public GameObject Prey { get; set; }
        public PerceptionSystem PerceptionSystem { get; set; }
        public CharacterMovement CharacterMovement { get; set; }
        public Transform Transform { get; set; }
    }

    public ChaseState(ChaseParams chaseParams) {
        this.chaseParams = chaseParams;
    }

    protected override void OnUpdate() {
        chaseParams.CharacterMovement.MoveTowards(chaseParams.Prey.transform.position);
    }

    public bool HasReachedPrey(float acceptanceRadius) {
        return Vector3.Distance(chaseParams.Transform.position, chaseParams.Prey.transform.position) < acceptanceRadius;
    }
}

