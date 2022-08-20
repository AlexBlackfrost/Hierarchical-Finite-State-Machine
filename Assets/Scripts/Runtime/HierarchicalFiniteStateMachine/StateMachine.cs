using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : StateObject  {
    public StateObject DefaultStateObject { get; set; } 
    public StateObject CurrentStateObject { get; private set; }
    internal LinkedList<StateMachine> PathFromRoot {
        get {
            if (pathFromRoot == null) {
                if (IsRoot()) {
                    PathFromRoot = new LinkedList<StateMachine>();
                } else {
                    PathFromRoot = new LinkedList<StateMachine>(StateMachine.PathFromRoot);
                }
                pathFromRoot.AddLast(this);
            }
            return pathFromRoot;
        }

        private set {
            pathFromRoot = value;
        } 
    }

    private LinkedList<StateMachine> pathFromRoot;
    private bool initialized;


    public StateMachine(params StateObject[] stateObjects) : base() {  
        DefaultStateObject = stateObjects[0];
        initialized = false; 
        foreach (StateObject stateObject in stateObjects) {
            stateObject.StateMachine = this;
        }
    }

    public void Init() {
        if (IsRoot()) {
            Enter();
            initialized = true;
        }
    }

    private void CheckInitialization() {
        if (IsRoot() && !initialized) {
            throw new RootStateMachineNotInitializedException("Root State Machine has not been initialized." +
                " Call rootStateMachine.Init() to initialize it");
        }
    }

    private bool TryChangeState() {
        bool changedState = false;
        Transition availableTransition = CurrentStateObject.GetAvailableTransition();
        CurrentStateObject.ConsumeTransitionsEvents();

        if (availableTransition != null) {
            ChangeState(availableTransition.TargetStateObject);
            changedState = true;
        }
        return changedState;
    }

    internal override sealed void ConsumeTransitionsEvents() {
        foreach (EventTransition eventTransition in eventTransitions.Values) {
            eventTransition.ConsumeEvent();
        }

        CurrentStateObject.ConsumeTransitionsEvents();
    }

    internal sealed override Transition GetAvailableTransition() {
        Transition availableTransition = null;
        foreach (Transition transition in transitions.Values) {
            if (transition.AllConditionsMet()) {
                availableTransition = transition;
                break;
            }
        }

        if(availableTransition == null) {
            availableTransition = CurrentStateObject.GetAvailableTransition();
        }
        return availableTransition;
    }

    private void ChangeState(StateObject stateObject) {
        StateMachine lowestCommonStateMachine = FindLowestCommonStateMachine(this, stateObject.StateMachine);

        lowestCommonStateMachine.CurrentStateObject.Exit();

        stateObject.StateMachine.CurrentStateObject = stateObject;
        StateMachine currentStateMachine = stateObject.StateMachine;
        while (currentStateMachine != null && !currentStateMachine.Equals(lowestCommonStateMachine)) {
            StateMachine parentStateMachine = currentStateMachine.StateMachine;
            parentStateMachine.CurrentStateObject = currentStateMachine;
            currentStateMachine = parentStateMachine;
        }

        lowestCommonStateMachine.CurrentStateObject.Enter();
    }

    public sealed override void Update() {
        CheckInitialization();
        bool changedState = TryChangeState();
        if (!changedState) {
            CurrentStateObject.UpdateInternal();
        }
    }

    internal sealed override void UpdateInternal() {
        OnUpdate();
        CurrentStateObject.UpdateInternal();
    }

    public sealed override void FixedUpdate() {
        CheckInitialization();
        CurrentStateObject.FixedUpdate();
        OnFixedUpdate();
    }

    public sealed override void LateUpdate() {
        CheckInitialization();
        CurrentStateObject.LateUpdate();
        OnLateUpdate();
    }

    internal sealed override void Enter() {
        if(CurrentStateObject == null) {
            CurrentStateObject = DefaultStateObject;
        } 

        OnEnter();
        CurrentStateObject.Enter();
    }

    internal sealed override void Exit() {
        CurrentStateObject.Exit();
        OnExit();
        CurrentStateObject = null;
    }

    public sealed override string GetCurrentStateName() {
        return GetType().ToString() + "." + CurrentStateObject.GetCurrentStateName();
    }
 
}