using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HFSM {
    public abstract class State : StateObject {

    public State() : base() { }

    internal sealed override void ConsumeTransitionsEvents() {
        foreach (EventTransition eventTransition in eventTransitions) {
            eventTransition.ConsumeEvent();
        }
    }

    internal sealed override Transition GetAvailableTransition() {
        Transition availableTransition = null;
        foreach (Transition transition in transitions) {
            if (transition.AllConditionsMet()) {
                availableTransition = transition;
                break;
            }
        }
        return availableTransition;
    }


    internal sealed override void UpdateInternal() {
        OnUpdate();
    }

    public sealed override void Update() {
        UpdateInternal();
    }

    public sealed override void FixedUpdate() {
        OnFixedUpdate();
    }

    public sealed override void LateUpdate() {
        OnLateUpdate();
    }

    internal sealed override void Enter() {
        IsActive = true;
        OnEnter();
    }

    internal sealed override void Exit() {
        IsActive = false;
        OnExit();
    }

    public sealed override string GetCurrentStateName() { 
        return GetType().ToString(); 
    }

    internal class Any : State { }
}
}