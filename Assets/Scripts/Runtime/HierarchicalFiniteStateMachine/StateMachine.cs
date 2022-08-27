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
    private List<Transition> anyTransitions;
    private List<EventTransition> anyEventTransitions;
    private bool initialized;
    private State anyState;

    public StateMachine(params StateObject[] stateObjects) : base() { 
        if(stateObjects.Length == 0) {
            throw new StatelessStateMachineException(
                "A State Machine must have at least one state object." +
                " State machine of type '" + GetType() + "' does not have any state objects."
            );
        }
        anyTransitions = new List<Transition>();
        anyEventTransitions = new List<EventTransition>();
        anyState = new State.Any();
        anyState.StateMachine = this;
        initialized = false;

        DefaultStateObject = stateObjects[0];
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

    #region AddAnyTransition methods
    public void AddAnyTransition(StateObject targetStateObject, params Func<bool>[] conditions) {
        Transition transition = new Transition(anyState, targetStateObject, null, conditions);
        TryRegisterAnyTransition(transition);
        
    }

    public void AddAnyTransition(StateObject targetStateObject, Action transitionAction,
        params Func<bool>[] conditions) {
        Transition transition = new Transition(anyState, targetStateObject, transitionAction, conditions);
        TryRegisterAnyTransition(transition);
    }

    public Action AddAnyEventTransition(StateObject targetStateObject, params Func<bool>[] conditions) {
        EventTransition transition = new EventTransition(anyState, targetStateObject, null, conditions);

        TryRegisterAnyTransition(transition);
        anyEventTransitions.Add(transition);
        return transition.ListenEvent;
    }

    public Action AddAnyEventTransition(StateObject targetStateObject, Action transitionAction, params Func<bool>[] conditions) {
        EventTransition transition = new EventTransition(anyState, targetStateObject, transitionAction, conditions);

        TryRegisterAnyTransition(transition);
        anyEventTransitions.Add(transition);
        return transition.ListenEvent;
    }

    public Action<T> AddAnyEventTransition<T>(StateObject targetStateObject, params Func<bool>[] conditions) {
        EventTransition transition = new EventTransition(anyState, targetStateObject, null, conditions);

        TryRegisterAnyTransition(transition);
        anyEventTransitions.Add(transition);
        return transition.ListenEvent;
    }

    public Action<T> AddAnyEventTransition<T>(StateObject targetStateObject, Action transitionAction, 
        params Func<bool>[] conditions) {

        EventTransition transition = new EventTransition(anyState, targetStateObject, transitionAction, conditions);
        TryRegisterAnyTransition(transition);
        anyEventTransitions.Add(transition);
        return transition.ListenEvent;
    }

    public Action<T1, T2> AddAnyEventTransition<T1, T2>(StateObject targetStateObject, params Func<bool>[] conditions) {

        EventTransition transition = new EventTransition(anyState, targetStateObject, null, conditions);
        TryRegisterAnyTransition(transition);
        anyEventTransitions.Add(transition);
        return transition.ListenEvent;
    }

    public Action<T1, T2> AddAnyEventTransition<T1, T2>(StateObject targetStateObject, 
        Action transitionAction, params Func<bool>[] conditions) {

        EventTransition transition = new EventTransition(anyState, targetStateObject, transitionAction, conditions);
        TryRegisterAnyTransition(transition);
        anyEventTransitions.Add(transition);
        return transition.ListenEvent;
    }

    public Action<T1, T2, T3> AddAnyEventTransition<T1, T2, T3>(StateObject targetStateObject, params Func<bool>[] conditions) {
        
        EventTransition transition = new EventTransition(anyState, targetStateObject, null, conditions);
        TryRegisterAnyTransition(transition);
        anyEventTransitions.Add(transition);
        return transition.ListenEvent;
    }

    public Action<T1, T2, T3> AddAnyEventTransition<T1, T2, T3>(StateObject targetStateObject, 
        Action transitionAction, params Func<bool>[] conditions) {

        EventTransition transition = new EventTransition(anyState, targetStateObject, transitionAction, conditions);
        TryRegisterAnyTransition(transition);
        anyEventTransitions.Add(transition);
        return transition.ListenEvent;
    }

    private void TryRegisterAnyTransition(Transition anyTransition) {
        if (!HaveCommonStateMachineAncestor(anyTransition.OriginStateObject, 
            anyTransition.TargetStateObject)) {

            throw new NoCommonParentStateMachineException(
                "States inside state machine of type " + this.GetType() + " and state object of " +
                "type " + anyTransition.TargetStateObject.GetType() + " don't have a common " +
                "parent state machine."
            );
        } else {
            anyTransitions.Add(anyTransition);
        }
    }
#endregion

    private bool TryChangeState() {
        bool changedState = false;
        Transition availableTransition = null;

        // Check any state object's transitions
        foreach (Transition anyTransition in anyTransitions) {
            if (anyTransition.AllConditionsMet()) {
                availableTransition = anyTransition;
                break;
            }
        }

        // Check current state object's transitions
        if(availableTransition == null) {
            availableTransition = CurrentStateObject.GetAvailableTransition();
        }

        foreach(EventTransition anyEventTransition in anyEventTransitions) {
            anyEventTransition.ConsumeEvent();
        }
        CurrentStateObject.ConsumeTransitionsEvents();

        if (availableTransition != null) {
            ChangeState(availableTransition.OriginStateObject,
                        availableTransition.TargetStateObject, 
                        availableTransition.TransitionAction);
            changedState = true;
        }
        return changedState;
    }

    internal override sealed void ConsumeTransitionsEvents() {
        foreach (EventTransition anyEventTransition in anyEventTransitions) {
            anyEventTransition.ConsumeEvent();
        }

        foreach (EventTransition eventTransition in eventTransitions) {
            eventTransition.ConsumeEvent();
        }

        CurrentStateObject.ConsumeTransitionsEvents();
    }

    internal sealed override Transition GetAvailableTransition() {
        Transition availableTransition = null;

        // Check this state machine's normal and event transitions
        foreach (Transition transition in transitions) {
            if (transition.AllConditionsMet()) {
                availableTransition = transition;
                break;
            }
        }
        
        // Check any state object's transitions
        foreach (Transition anyTransition in anyTransitions) {
            if (anyTransition.AllConditionsMet()) {
                availableTransition = anyTransition;
                break;
            }
        }

        // Check current state object's transitions
        if (availableTransition == null) {
            availableTransition = CurrentStateObject.GetAvailableTransition();
        }

        return availableTransition;
    }

    private void ChangeState(StateObject originStateObject, StateObject targetStateObject, 
        Action transitionAction) {

        StateMachine stateMachine1 = originStateObject.StateMachine;
        StateMachine stateMachine2 = targetStateObject.StateMachine;

        StateMachine lowestCommonStateMachine = FindLowestCommonStateMachine(
            stateMachine1, stateMachine2
        );

        lowestCommonStateMachine.CurrentStateObject.Exit();

        targetStateObject.StateMachine.CurrentStateObject = targetStateObject;
        StateMachine currentStateMachine = targetStateObject.StateMachine;
        while (currentStateMachine != null && !currentStateMachine.Equals(lowestCommonStateMachine)) {
            StateMachine parentStateMachine = currentStateMachine.StateMachine;
            parentStateMachine.CurrentStateObject = currentStateMachine;
            currentStateMachine = parentStateMachine;
        }

        transitionAction?.Invoke();
        lowestCommonStateMachine.CurrentStateObject.Enter();
    }

    public sealed override void Update() {
        CheckInitialization();
        bool changedState = TryChangeState();
        if (!changedState) {
            OnUpdate();
            CurrentStateObject.UpdateInternal();
        }
    }

    internal sealed override void UpdateInternal() {
        OnUpdate();
        CurrentStateObject.UpdateInternal();
    }

    public sealed override void FixedUpdate() {
        CheckInitialization();
        OnFixedUpdate();
        CurrentStateObject.FixedUpdate();
    }

    public sealed override void LateUpdate() {
        CheckInitialization();
        OnLateUpdate();
        CurrentStateObject.LateUpdate();
    }

    internal sealed override void Enter() {
        IsActive = true;
        if(CurrentStateObject == null) {
            CurrentStateObject = DefaultStateObject;
        } 

        OnEnter();
        CurrentStateObject.Enter();
    }

    internal sealed override void Exit() {
        CurrentStateObject.Exit();
        OnExit();
        IsActive = false;
        CurrentStateObject = null;
    }

    public sealed override string GetCurrentStateName() {
        return GetType().ToString() + "." + CurrentStateObject.GetCurrentStateName();
    }
}