using HFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Species { None, Red, Blue, Green}

public class AnimalController : MonoBehaviour {
    public CharacterMovement characterMovement;
    public GameObject ground;
    public Species species;

    [Header("States settings")]
    public IdleState.IdleParams idleStateSettings;
    public WanderState.WanderParams wanderStateSettings;
    public AliveStateMachine.AliveParams aliveStateMachineSettings;
    public DeadState.DeadParams deadStateSettings;
    public ChaseState.ChaseParams chaseStateSettings;
    public AttackState.AttackParams attackStateSettings;

    private StateMachine rootStateMachine;
    private PerceptionSystem perceptionSystem;
    private CharacterInfoDebugger infoDebugger;
    private event Action<float> Damaged;

    private void Awake() {
        Init();
        InitStatesParameters();
        BuildHFSM();
    }

    private void Init() {
        infoDebugger = GetComponent<CharacterInfoDebugger>();
        perceptionSystem = GetComponent<PerceptionSystem>();
        CharacterController characterController = GetComponent<CharacterController>();
        characterMovement.Init(characterController);
    }

    private void InitStatesParameters() {
        aliveStateMachineSettings.MonoBehaviour = this;
        aliveStateMachineSettings.MeshRenderers = GetComponentsInChildren<MeshRenderer>();
        aliveStateMachineSettings.Transform = transform;

        wanderStateSettings.Transform = transform;
        wanderStateSettings.CharacterMovement = characterMovement;

        idleStateSettings.Transform = transform;

        deadStateSettings.Transform = transform;
        deadStateSettings.Destroy = DestroyCharacter;

        chaseStateSettings.CharacterMovement = characterMovement;
        chaseStateSettings.Transform = transform;

        attackStateSettings.Transform = transform;
        attackStateSettings.CharacterMovement = characterMovement;
        attackStateSettings.MonoBehaviour = this;
    }

    private void BuildHFSM() { 
        // Create states and state machines
        WanderState wanderState = new WanderState(wanderStateSettings);
        IdleState idleState = new IdleState(idleStateSettings);
        DeadState deadState = new DeadState(deadStateSettings);
        ChaseState chaseState = new ChaseState(chaseStateSettings);
        AttackState attackState = new AttackState(attackStateSettings);
        AliveStateMachine aliveStateMachine = new AliveStateMachine(aliveStateMachineSettings, wanderState, idleState, chaseState, attackState);
        RootStateMachine rootStateMachine = new RootStateMachine(aliveStateMachine, deadState);

        // Create transitions 
        wanderState.AddTransition(idleState, () => { return wanderState.HasReachedTargetLocation(1f); });
        idleState.AddTransition(wanderState, idleState.IdleTimeFinished);
        aliveStateMachine.AddTransition(deadState, () => { return !IsAlive(); });
        chaseState.AddTransition(attackState, SetAttackPrey, () => { return chaseState.HasReachedPrey(attackStateSettings.AttackRange); },
                                                             () => { return !attackState.IsOnCooldown; });
        attackState.AddTransition(idleState, attackState.HasFinishedAnimation);

        // Create event transitions
        perceptionSystem.AnimalDetected += idleState.AddEventTransition<GameObject>(chaseState, 
                                                                                    SetChasePrey, 
                                                                                    (GameObject animal) => { return !attackState.IsOnCooldown; },
                                                                                    DetectedAnimalIsPrey
                                                                                    );
        perceptionSystem.AnimalDetected += wanderState.AddEventTransition<GameObject>(chaseState, 
                                                                                      SetChasePrey, 
                                                                                      (GameObject animal) => { return !attackState.IsOnCooldown; },
                                                                                      DetectedAnimalIsPrey
                                                                                     );
        deadState.Dead += attackState.AddEventTransition(idleState);
        deadState.Dead += chaseState.AddEventTransition(idleState);

        Damaged += aliveStateMachine.OnDamaged;
        this.rootStateMachine = rootStateMachine;
    }

    public void DealDamage(float damage) {
        Damaged.Invoke(damage);
    }

    public bool IsAlive() {
        return aliveStateMachineSettings.CurrentHealth > 0;
    }

    private void DestroyCharacter(float delay) {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, delay);
    }

    #region Conditions
    private bool DetectedAnimalIsPrey(GameObject detectedAnimal) {
        bool animalIsPrey = false;
        Species detectedAnimalSpecies = detectedAnimal.GetComponent<AnimalController>().species;
        if (chaseStateSettings.PreySpecies.Contains(detectedAnimalSpecies)) {
            animalIsPrey = true;
        }
        return animalIsPrey;
    }

    private bool DetectedAnimalIsAlive(GameObject detectedAnimal) {
        return detectedAnimal.GetComponent<AnimalController>().IsAlive();
    }
    #endregion

    #region Transition actions
    private void SetChasePrey(GameObject prey){
        chaseStateSettings.Prey = prey;
    }

    private void SetAttackPrey() {
        attackStateSettings.Prey = chaseStateSettings.Prey;
    }
    #endregion

    private void Start() {
        this.rootStateMachine.Init();
    }

    private void Update() {
        rootStateMachine.Update();
        UpdateDebugInfo();
    }

    private void UpdateDebugInfo() { 
        infoDebugger.UpdateCurrentState(rootStateMachine.GetCurrentStateName());
        infoDebugger.UpdatePosition();
    }
}

