using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class StateObject {
    internal StateMachine StateMachine { get; set; }
    internal bool IsActive { get; private protected set; }
    private protected List<Transition> transitions;
    private protected List<EventTransition> eventTransitions;

    public StateObject() {
        transitions = new List<Transition>();
        eventTransitions = new List< EventTransition>();
        IsActive = false;
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

    public void AddTransition(StateObject destinationStateObject, Action transitionAction, params Func<bool>[] conditions) {
        Transition transition = new Transition(this, destinationStateObject,transitionAction, conditions);
        TryRegisterTransition(transition);
    }

    #region Add Event Transition Methods
    public Action AddEventTransition(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryRegisterEventTransition(destinationStateObject, null,conditions);
        return transition.ListenEvent;
    }
    public Action AddEventTransition(StateObject destinationStateObject, 
        Action transitionAction,params Func<bool>[] conditions) {

        EventTransition transition = TryRegisterEventTransition(destinationStateObject, transitionAction, conditions);
        return transition.ListenEvent;
    }

    public Action<T> AddEventTransition<T>(StateObject destinationStateObject, params Func<bool>[] conditions){
        EventTransition transition = TryRegisterEventTransition(destinationStateObject, null, conditions);
        return transition.ListenEvent;
    }

    public Action<T> AddEventTransition<T>(StateObject destinationStateObject, 
        Action transitionAction, params Func<bool>[] conditions) {

        EventTransition transition = TryRegisterEventTransition(destinationStateObject, transitionAction, conditions);
        return transition.ListenEvent;
    }
    public Action<T1, T2> AddEventTransition<T1, T2>(StateObject destinationStateObject,
        params Func<bool>[] conditions) {

        EventTransition transition = TryRegisterEventTransition(destinationStateObject, null, conditions);
        return transition.ListenEvent;
    }

    public Action<T1, T2> AddEventTransition<T1, T2>(StateObject destinationStateObject, 
        Action transitionAction, params Func<bool>[] conditions) {

        EventTransition transition = TryRegisterEventTransition(destinationStateObject, transitionAction, conditions);
        return transition.ListenEvent;
    }

    public Action<T1, T2, T3> AddEventTransition<T1, T2, T3>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryRegisterEventTransition(destinationStateObject, null, conditions);
        return transition.ListenEvent;
    }

    public Action<T1, T2, T3> AddEventTransition<T1, T2, T3>(StateObject destinationStateObject, 
        Action transitionAction, params Func<bool>[] conditions) {
        EventTransition transition = TryRegisterEventTransition(destinationStateObject, transitionAction, conditions);
        return transition.ListenEvent;
    }

    /*public Action<T1, T2, T3, T4> AddTransition<T1, T2, T3, T4>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }
    
    public Action<T1, T2, T3, T4, T5> AddTransition<T1, T2, T3, T4, T5>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }

    public Action<T1, T2, T3, T4, T5, T6> AddTransition<T1, T2, T3, T4, T5, T6>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }
    public Action<T1, T2, T3, T4, T5, T6, T7> AddTransition<T1, T2, T3, T4, T5, T6, T7>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }
    public Action<T1, T2, T3, T4, T5, T6, T7, T8> AddTransition<T1, T2, T3, T4, T5, T6, T7, T8>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }
    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> AddTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }
    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AddTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }
    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AddTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }
    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AddTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }
    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> AddTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }
    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> AddTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }

    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> AddTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }
    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> AddTransition<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(StateObject destinationStateObject, params Func<bool>[] conditions) {
        EventTransition transition = TryCreateEventTransition(destinationStateObject, conditions);
        return transition.ListenEvent;
    }*/
    #endregion

    private protected void TryRegisterTransition(Transition transition) {
        if(!HaveCommonStateMachineAncestor(transition.OriginStateObject, transition.TargetStateObject)) {
            throw new NoCommonParentStateMachineException(
                "State objects " + transition.OriginStateObject.GetType() +
                " and " + transition.TargetStateObject.GetType() + " don't have " + 
                " a common parent state machine."
            );
        }

        transitions.Add(transition);
    }

    private EventTransition TryRegisterEventTransition(StateObject destinationStateObject,
        Action transitionAction, params Func<bool>[] conditions) {

        EventTransition transition = new EventTransition(this, destinationStateObject, transitionAction, conditions);
        TryRegisterTransition(transition);
        eventTransitions.Add(transition);
        return transition;
    }

    private protected bool HaveCommonStateMachineAncestor(StateObject stateObject1, StateObject stateObject2) {
        bool haveCommonStateMachineAncestor = false;
        StateMachine stateMachine1 = stateObject1.StateMachine;
        StateMachine stateMachine2 = stateObject2.StateMachine;

        if(stateMachine1 != null && stateMachine2 != null) {
            haveCommonStateMachineAncestor = 
                FindLowestCommonStateMachine(stateMachine1, stateMachine2) != null;
        }

        return haveCommonStateMachineAncestor;
    }


    /* <summary>
     * Lowest Common Ancestor tree algorithm
     * </summary>
     */
    private protected StateMachine FindLowestCommonStateMachine(StateMachine sm1, StateMachine sm2) {
        LinkedListNode<StateMachine> currentAncestor1 = sm1.PathFromRoot.First;
        LinkedListNode<StateMachine> currentAncestor2 = sm2.PathFromRoot.First;
        StateMachine lowestCommonStateMachine = null;

        while (currentAncestor1 != null && currentAncestor2 != null &&
            currentAncestor1.Value.Equals(currentAncestor2.Value)) {

            lowestCommonStateMachine = currentAncestor1.Value;
            currentAncestor1 = currentAncestor1.Next;
            currentAncestor2 = currentAncestor2.Next;
        }
        return lowestCommonStateMachine;
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

