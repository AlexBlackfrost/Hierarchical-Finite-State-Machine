using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class StateObject  {
    internal StateMachine StateMachine { get; set; }

    private protected Dictionary<Type, Transition> transitions;
    private protected Dictionary<Type, EventTransition> eventTransitions;

    public StateObject() {
        transitions = new Dictionary<Type, Transition>();
        eventTransitions = new Dictionary<Type, EventTransition>();
    }

    public bool Equals(StateObject otherStateObject) {
        return this.GetType() == otherStateObject.GetType();
    }

    public bool IsRoot() {
        return StateMachine == null;
    }

    public void AddTransition(StateObject destinationStateObject, params Func<bool>[] conditions) {

        Transition transition = new Transition(this, destinationStateObject, conditions);
        TryRegisterTransition(transition);
    }

    public void AddTransition(Action transitionEvent,StateObject destinationStateObject, 
                              params Func<bool>[] conditions) {

        EventTransition transition = new EventTransition(transitionEvent, this,
                                                    destinationStateObject, conditions);
        TryRegisterTransition(transition);
        eventTransitions[destinationStateObject.GetType()] = transition;
    }

    private void TryRegisterTransition(Transition transition) {
        if (transitions.ContainsKey(transition.TargetStateObject.GetType())) {
            throw new DuplicatedTransitionException(
                "State object " + transition.OriginStateObject.GetType() + 
                " already has a transition to state object "
                + transition.OriginStateObject.GetType()
            );
        }

        transitions[transition.TargetStateObject.GetType()] = transition;
    }


    public abstract string GetCurrentStateName();
    internal abstract void ConsumeTransitionsEvents();
    internal abstract Transition GetAvailableTransition();
    internal abstract void UpdateInternal();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void LateUpdate();
    internal abstract void Enter();
    internal abstract void Exit();

    
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnLateUpdate() { }
    protected virtual void OnEnter() { }
    protected virtual void OnExit() { }

}

