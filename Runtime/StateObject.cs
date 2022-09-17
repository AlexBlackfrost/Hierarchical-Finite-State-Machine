using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFSM {

    public abstract class StateObject {
        internal StateMachine StateMachine { get; set; }
        internal bool IsActive { get; private protected set; }
        private protected List<Transition> transitions;
        private protected List<EventTransitionBase> eventTransitions;

        public StateObject() {
            transitions = new List<Transition>();
            eventTransitions = new List<EventTransitionBase>();
            IsActive = false;
        }

        public bool Equals(StateObject otherStateObject) {
            return this.GetType() == otherStateObject.GetType();
        }

        public bool IsRoot() {
            return StateMachine == null;
        }

        public void AddTransition(StateObject destinationStateObject, params Func<bool>[] conditions) {
            Transition transition = new Transition(this, destinationStateObject, null, conditions);
            TryRegisterTransition(transition);
        }

        public void AddTransition(StateObject destinationStateObject, Action transitionAction, params Func<bool>[] conditions) {
            Transition transition = new Transition(this, destinationStateObject,transitionAction, conditions);
            TryRegisterTransition(transition);
        }

        #region Add Event Transition Methods
        public Action AddEventTransition(StateObject destinationStateObject, params Func<bool>[] conditions) {
            EventTransition transition = new EventTransition(this, destinationStateObject, null, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }
        public Action AddEventTransition(StateObject destinationStateObject, 
            Action transitionAction, params Func<bool>[] conditions) {

            EventTransition transition = new EventTransition(this, destinationStateObject, transitionAction, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        public Action<T> AddEventTransition<T>(StateObject destinationStateObject, params Func<T, bool>[] conditions){
            EventTransition<T> transition = new EventTransition<T>(this, destinationStateObject, null, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        public Action<T> AddEventTransition<T>(StateObject destinationStateObject, 
            Action<T> transitionAction, params Func<T, bool>[] conditions) {

            EventTransition<T> transition = new EventTransition<T>(this, destinationStateObject, transitionAction, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        public Action<T1, T2> AddEventTransition<T1, T2>(StateObject destinationStateObject,
            params Func<T1, T2, bool>[] conditions) {

            EventTransition<T1, T2> transition = new EventTransition<T1, T2>(this, destinationStateObject, null, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        public Action<T1, T2> AddEventTransition<T1, T2>(StateObject destinationStateObject, 
            Action<T1, T2> transitionAction, params Func<T1, T2, bool>[] conditions) {

            EventTransition<T1, T2> transition = new EventTransition<T1, T2>(this, destinationStateObject, transitionAction, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        public Action<T1, T2, T3> AddEventTransition<T1, T2, T3>(StateObject destinationStateObject, 
            params Func<T1, T2, T3, bool>[] conditions) {

            EventTransition<T1, T2, T3> transition = new EventTransition<T1, T2, T3>(this, destinationStateObject, null, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        public Action<T1, T2, T3> AddEventTransition<T1, T2, T3>(StateObject destinationStateObject, 
            Action<T1, T2, T3> transitionAction, params Func<T1, T2, T3, bool>[] conditions) {

            EventTransition<T1, T2, T3> transition = new EventTransition<T1, T2, T3>(this, destinationStateObject, transitionAction, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }
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

        private void TryRegisterEventTransition(EventTransitionBase eventTransition) {
            TryRegisterTransition(eventTransition);
            eventTransitions.Add(eventTransition);
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
}

