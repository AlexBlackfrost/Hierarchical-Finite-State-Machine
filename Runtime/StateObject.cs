using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFSM {
    /// <summary>
	/// Base class for <see cref="State"/> and <see cref="HFSM.StateMachine"/> objects.
	/// </summary>
    public abstract class StateObject {
        internal StateMachine StateMachine { get; set; }
        internal bool IsActive { get; private protected set; }
        private protected List<Transition> transitions;
        private protected List<EventTransitionBase> eventTransitions;

        /// <summary>
		/// <see cref="StateObject"/> class constructor.
		/// </summary>
        public StateObject() {
            transitions = new List<Transition>();
            eventTransitions = new List<EventTransitionBase>();
            IsActive = false;
        }

        /// <summary>
        /// Indicates whether some other <see cref="StateObject"/> is "equal to" this one.
        /// </summary>
        /// <param name="otherStateObject">
        /// The <see cref="StateObject"/> with which to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the the <see cref="StateObject"/>s are of the same subclass, <see langword="false"/> otherwise.
        /// </returns>
        public bool Equals(StateObject otherStateObject) {
            return this.GetType() == otherStateObject.GetType();
        }

        /// <summary>
        /// Indicates whether this <see cref="StateObject"/> is at the top of the <see cref="StateObject"/>s hierarchy.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="StateObject"/> is not inside a state machine, <see langword="false"/> otherwise.
        /// </returns>
        public bool IsRoot() {
            return StateMachine == null;
        }

        /// <summary>
        /// Adds a <see cref="Transition"/> to <paramref name="destinationStateObject"/>.
        /// In order to change to <paramref name="destinationStateObject"/>, all <paramref name="conditions"/> 
        /// (if any) must return <see langword="true"/>. 
        /// </summary>
        /// <param name="destinationStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name="conditions">
        /// The list of conditions that must be met in order to change to a new <see cref="StateObject"/>.
        /// </param>
        public void AddTransition(StateObject destinationStateObject, params Func<bool>[] conditions) {
            Transition transition = new Transition(this, destinationStateObject, null, conditions);
            TryRegisterTransition(transition);
        }

        /// <summary>
        /// Adds a <see cref="Transition"/> to <paramref name="destinationStateObject"/>.
        /// In order to change to <paramref name="destinationStateObject"/>, all <paramref name="conditions"/> 
        /// (if any) must return true. Transitions added first have higher priority. <paramref name="transitionAction"/> 
        /// is executed after the transition occurs.
        /// </summary>
        /// <param name="destinationStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name="transitionAction">
        /// Function to be executed if the transition occurs. It is executed after current <see cref="StateObject"/> 
        /// is exited and before the new one is entered.
        /// </param>
        /// <param name="conditions">
        /// The list of conditions that must be met in order to change to a new <see ref="StateObject"/>.
        /// </param>
        public void AddTransition(StateObject destinationStateObject, Action transitionAction, params Func<bool>[] conditions) {
            Transition transition = new Transition(this, destinationStateObject, transitionAction, conditions);
            TryRegisterTransition(transition);
        }

        #region Add Event Transition Methods
        /// <summary>
        /// Adds an <see cref="EventTransition"/> to the <see cref="StateObject"/> <paramref name="destinationStateObject"/>.
        /// In order to change to <paramref name="destinationStateObject"/>, the event must have been fired and
        /// all <paramref name="conditions"/> (if any) must return <see langword="true"/>. Transitions added first have higher priority.
        /// All <see cref="EventTransition"/>s are processed together with polling transitions at the same time in the execution flow.
        /// Set <paramref name="processInstantly"/> parameter to <see langword="true"/> if you want to process transition events as soon as
        /// the event is listened. Events are only listened if the origin <see cref="StateObject"/> is active.
        /// </summary>
        /// <param name="destinationStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name="conditions">
        /// The list of conditions that must be met in order to change to a new <see cref="StateObject"/>.
        /// </param>
        /// <returns>
        /// An event listener function that must be subscribed to an event.
        /// </returns>
        public Action AddEventTransition(StateObject destinationStateObject, params Func<bool>[] conditions) {
            EventTransition transition = new EventTransition(this, destinationStateObject, null, false, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <summary>
        /// Adds an <see cref="EventTransition"/> to <paramref name="destinationStateObject"/>.
        /// In order to change to <paramref name="destinationStateObject"/>, the event must have been fired and
        /// all <paramref name="conditions"/> (if any) must return <see langword="true"/>. Transitions added first have higher priority.
        /// All <see cref="EventTransition"/>s are processed together with polling transitions at the same time in the execution flow.
        /// Set <paramref name="processInstantly"/> parameter to <see langword="true"/> if you want to process transition events as soon as
        /// the event is listened. Events are only listened if the origin <see cref="StateObject"/> is active.
        /// </summary>
        /// <param name="destinationStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name="processInstantly">
        /// Indicates whether <see cref="Transition"/>s should be evaluated as soon as the event is listened or not.
        /// </param>
        /// <param name="conditions">
        /// The list of conditions that must be met in order to change to a new <see cref="StateObject"/>.
        /// </param>
        /// <returns>
        /// An event listener function that must be subscribed to an event.
        /// </returns>
        public Action AddEventTransition(StateObject destinationStateObject, bool processInstantly, params Func<bool>[] conditions) {
            EventTransition transition = new EventTransition(this, destinationStateObject, null, processInstantly, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <summary>
        /// Adds an <see cref="EventTransition"/> to <paramref name="destinationStateObject"/>.
        /// In order to change to <paramref name="destinationStateObject"/>, the event must have been fired and
        /// all <paramref name="conditions"/> (if any) must return <see langword="true"/>. Transitions added first have higher priority.
        /// All <see cref="EventTransition"/>s are processed together with polling transitions at the same time in the execution flow.
        /// Set <paramref name="processInstantly"/> parameter to <see langword="true"/> if you want to process transition events as soon as
        /// the event is listened. Events are only listened if the origin <see cref="StateObject"/> is active. <paramref name="transitionAction"/> 
        /// is executed after the transition occurs.
        /// </summary>
        /// <param name = "destinationStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name = "transitionAction">
        /// Function to be executed if the transition occurs. It is executed after current <see cref="StateObject"/> 
        /// is exited and before the new one is entered.
        /// </param>
        /// <param name = "conditions">
        /// The list of conditions that must be met in order to change to a new <see cref="StateObject"/>.
        /// </param>
        /// <returns>
        /// An event listener function that must be subscribed to an event.
        /// </returns>
        public Action AddEventTransition(StateObject destinationStateObject,
            Action transitionAction, params Func<bool>[] conditions) {

            EventTransition transition = new EventTransition(this, destinationStateObject, transitionAction, false, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <summary>
        /// Adds an <see cref="EventTransition"/> to <paramref name="destinationStateObject"/>.
        /// In order to change to <paramref name="destinationStateObject"/>, the event must have been fired and
        /// all <paramref name="conditions"/> (if any) must return <see langword="true"/>. Transitions added first have higher priority.
        /// All <see cref="EventTransition"/>s are processed together with polling transitions at the same time in the execution flow.
        /// Set <paramref name="processInstantly"/> parameter to <see langword="true"/> if you want to process transition events as soon as
        /// the event is listened. Events are only listened if the origin <see cref="StateObject"/> is active. <paramref name="transitionAction"/> 
        /// is executed after the transition occurs.
        /// </summary>
        /// <param name = "destinationStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name = "transitionAction">
        /// Function to be executed if the transition occurs. It is executed after current <see cref="StateObject"/> 
        /// is exited and before the new one is entered.
        /// </param>
        /// <param name="processInstantly">
        /// Indicates whether <see cref="Transition"/>s should be evaluated as soon as the event is listened or not.
        /// </param>
        /// <param name = "conditions">
        /// The list of conditions that must be met in order to change to a new <see cref="StateObject"/>.
        /// </param>
        /// <returns>
        /// An event listener function that must be subscribed to an event.
        /// </returns>
        public Action AddEventTransition(StateObject destinationStateObject,
            Action transitionAction, bool processInstantly, params Func<bool>[] conditions) {

            EventTransition transition = new EventTransition(this, destinationStateObject, transitionAction, processInstantly, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, Func{bool}[])"/>
        public Action<T> AddEventTransition<T>(StateObject destinationStateObject, params Func<T, bool>[] conditions) {
            EventTransition<T> transition = new EventTransition<T>(this, destinationStateObject, null, false, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, bool, Func{bool}[])"/>
        public Action<T> AddEventTransition<T>(StateObject destinationStateObject, bool processInstantly, params Func<T, bool>[] conditions) {
            EventTransition<T> transition = new EventTransition<T>(this, destinationStateObject, null, processInstantly, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, Action, Func{bool}[])"/>
        public Action<T> AddEventTransition<T>(StateObject destinationStateObject,
            Action<T> transitionAction, params Func<T, bool>[] conditions) {

            EventTransition<T> transition = new EventTransition<T>(this, destinationStateObject, transitionAction, false, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, Action, bool, Func{bool}[])"/>
        public Action<T> AddEventTransition<T>(StateObject destinationStateObject,
            Action<T> transitionAction, bool processInstantly, params Func<T, bool>[] conditions) {

            EventTransition<T> transition = new EventTransition<T>(this, destinationStateObject, transitionAction, processInstantly, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, Func{bool}[])"/>
        public Action<T1, T2> AddEventTransition<T1, T2>(StateObject destinationStateObject, params Func<T1, T2, bool>[] conditions) {

            EventTransition<T1, T2> transition = new EventTransition<T1, T2>(this, destinationStateObject, null, false, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, bool, Func{bool}[])"/>
        public Action<T1, T2> AddEventTransition<T1, T2>(StateObject destinationStateObject,
            bool processInstantly, params Func<T1, T2, bool>[] conditions) {

            EventTransition<T1, T2> transition = new EventTransition<T1, T2>(this, destinationStateObject, null, processInstantly, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, Action, Func{bool}[])"/>
        public Action<T1, T2> AddEventTransition<T1, T2>(StateObject destinationStateObject,
            Action<T1, T2> transitionAction, params Func<T1, T2, bool>[] conditions) {

            EventTransition<T1, T2> transition = new EventTransition<T1, T2>(this, destinationStateObject, transitionAction, false, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, Action, bool, Func{bool}[])"/>
        public Action<T1, T2> AddEventTransition<T1, T2>(StateObject destinationStateObject,
            Action<T1, T2> transitionAction, bool processInstantly = false, params Func<T1, T2, bool>[] conditions) {

            EventTransition<T1, T2> transition = new EventTransition<T1, T2>(this, destinationStateObject, transitionAction, processInstantly, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, Func{bool}[])"/>
        public Action<T1, T2, T3> AddEventTransition<T1, T2, T3>(StateObject destinationStateObject, params Func<T1, T2, T3, bool>[] conditions) {

            EventTransition<T1, T2, T3> transition = new EventTransition<T1, T2, T3>(this, destinationStateObject, null, false, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, bool, Func{bool}[])"/>
        public Action<T1, T2, T3> AddEventTransition<T1, T2, T3>(StateObject destinationStateObject,
            bool processInstantly, params Func<T1, T2, T3, bool>[] conditions) {

            EventTransition<T1, T2, T3> transition = new EventTransition<T1, T2, T3>(this, destinationStateObject, null, processInstantly, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, Action, Func{bool}[])"/>
        public Action<T1, T2, T3> AddEventTransition<T1, T2, T3>(StateObject destinationStateObject,
            Action<T1, T2, T3> transitionAction, params Func<T1, T2, T3, bool>[] conditions) {

            EventTransition<T1, T2, T3> transition = new EventTransition<T1, T2, T3>(this, destinationStateObject, transitionAction, false, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddEventTransition(StateObject, Action, bool, Func{bool}[])"/>
        public Action<T1, T2, T3> AddEventTransition<T1, T2, T3>(StateObject destinationStateObject,
            Action<T1, T2, T3> transitionAction, bool processInstantly = false, params Func<T1, T2, T3, bool>[] conditions) {

            EventTransition<T1, T2, T3> transition = new EventTransition<T1, T2, T3>(this, destinationStateObject, transitionAction, processInstantly, conditions);
            TryRegisterEventTransition(transition);
            return transition.ListenEvent;
        }
        #endregion

        /// <summary>
        /// Tries to store a transition. Throws a <paramref name="NoCommonParentStateMachineException"/>
        /// exception if the <see cref="StateObject"/>s of the transition are not inside a common state machine 
        /// of the state machine hierarchy tree.
        /// </summary>
        /// <param name="transition">
        /// The transition.
        /// </param>
        /// <exception cref="NoCommonParentStateMachineException">
        /// </exception>
        private protected void TryRegisterTransition(Transition transition) {
            if (!HaveCommonStateMachineAncestor(transition.OriginStateObject, transition.TargetStateObject)) {
                throw new NoCommonParentStateMachineException(
                    typeof(StateObject).ToString() + "s " + transition.OriginStateObject.GetType() +
                    " and " + transition.TargetStateObject.GetType() + " don't have " +
                    " a common parent " + typeof(StateMachine).ToString() + " state machine."
                );
            }

            transitions.Add(transition);
        }

        /// <summary>
        /// Tries to store an event transition. Throws a <see cref="NoCommonParentStateMachineException"/>
        /// exception if the <see cref="StateObject"/>s of the transition are not inside a common state machine 
        /// of the state machine hierarchy tree.
        /// </summary>
        /// <param name="transition">
        /// The event transition.
        /// </param>
        /// <exception cref="NoCommonParentStateMachineException">
        /// </exception>
        private void TryRegisterEventTransition(EventTransitionBase eventTransition) {
            TryRegisterTransition(eventTransition);
            eventTransitions.Add(eventTransition);
        }

        /// <summary>
        /// Checks whether <paramref name="stateObject1"/> and <paramref name="stateObject2"/> have
        /// a common state machine ancestor in the state machine hierarchy tree.
        /// </summary>
        /// <param name="stateObject1"></param>
        /// <param name="stateObject2"></param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="stateObject1"/> and <paramref name="stateObject2"/> have
        /// a common state machine ancestor in the state machine hierarchy tree, <see langword="false"/> otherwise.
        /// </returns>
        private protected bool HaveCommonStateMachineAncestor(StateObject stateObject1, StateObject stateObject2) {
            bool haveCommonStateMachineAncestor = false;
            StateMachine stateMachine1 = stateObject1.StateMachine;
            StateMachine stateMachine2 = stateObject2.StateMachine;

            if (stateMachine1 != null && stateMachine2 != null) {
                haveCommonStateMachineAncestor =
                    FindLowestCommonStateMachine(stateMachine1, stateMachine2) != null;
            }

            return haveCommonStateMachineAncestor;
        }



        /// <summary>
        /// Lowest Common Ancestor tree algorithm. Finds the lowest common <see cref="HFSM.StateMachine"/> ancestor
        /// of <paramref name="sm1"/> and <paramref name="sm2"/> in the <see cref="HFSM.StateMachine"/> hierarchy tree.
        /// </summary>
        /// <param name="sm1">
        /// A <see cref="HFSM.StateMachine"/> object.
        /// </param>
        /// <param name="sm2">
        /// A <see cref="HFSM.StateMachine"/> object.
        /// </param>
        /// <returns>
        /// The lowest common <see cref="HFSM.StateMachine"/> ancestor or <see langword="null"/> if <paramref name="sm1"/> and <paramref name="sm2"/>
        /// don't have a common <see cref="HFSM.StateMachine"/> ancestor.
        /// </returns>
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

        /// <summary>
        /// Returns the name of the current <see cref="StateObject"/> including the names of all the
        /// parent <see cref="StateObject"/>s starting from the root of the hierarchy.
        /// </summary>
        /// <returns>
        /// The full name of the <see cref="StateObject"/>.
        /// </returns>
        public abstract string GetCurrentStateName();

        /// <summary>
        /// Consumes the events listened by the <see cref="EventTransition"/>s.
        /// </summary>
        internal abstract void ConsumeTransitionsEvents();

        /// <summary>
        /// Tries to find a transition that can be performed in this update call.
        /// </summary>
        /// <returns>
        /// An available <see cref="Transition"/>, if there is any, <see langword="null"/> otherwise.
        /// </returns>
        internal abstract Transition GetAvailableTransition();

        /// <summary>
        /// Executes the logic needed to implement the hierarchical finite state machine pattern.
        /// It should be called every frame.
        /// </summary>
        internal abstract void UpdateInternal();

        /// <summary>
        /// Executes the logic needed to implement the hierarchical finite state machine pattern 
        /// as well as the custom logic defined in concrete <see cref="StateObject"/>s. It should be called from
        /// MonoBehaviour.Update function.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Executes the custom logic defined in concrete <see cref="StateObject"/>s that should be executed with
        /// frame-rate independece. It should be called from MonoBehaviour.FixedUpdate function.
        /// </summary>
        public abstract void FixedUpdate();

        /// <summary>
        /// Executes the custom logic defined in concrete <see cref="StateObject"/>s that needs be executed after
        /// the regular Update cycle. It should be called from MonoBehaviour.LateUpdate function.
        /// </summary>
        public abstract void LateUpdate();

        /// <summary>
        /// Executes the logic needed to implement the hierarchical finite state machine pattern 
        /// as well as the custom logic defined in concrete <see cref="StateObject"/> classes. It is executed when a 
        /// transition is performed; it is executed when entering a new <see cref="StateObject"/>.
        /// </summary>
        internal abstract void Enter();

        /// <summary>
        /// Executes the logic needed to implement the hierarchical finite state machine pattern 
        /// as well as the custom logic defined in concrete <see cref="StateObject"/>s. It is executed when a 
        /// transition is performed; it is executed when leaving the current <see cref="StateObject"/>.
        /// </summary>
        internal abstract void Exit();

        /// <summary>
        /// Custom logic defined in concrete <see cref="StateObject"/> classes that gets executed every update cycle.
        /// </summary>
        protected virtual void OnUpdate() { }

        /// <summary>
        /// Custom logic defined in concrete <see cref="StateObject"/> <see cref="StateObject"/>s that gets executed every fixed update cycle.
        /// </summary>
        protected virtual void OnFixedUpdate() { }

        /// <summary>
        /// Custom logic defined in concrete <see cref="StateObject"/> classes that gets executed every late update cycle.
        /// </summary>
        protected virtual void OnLateUpdate() { }

        /// <summary>
        /// Custom logic defined in concrete <see cref="StateObject"/> classes that gets executed when a new <see cref="StateObject"/> is entered.
        /// </summary>
        protected virtual void OnEnter() { }

        /// <summary>
        /// Custom logic defined in concrete <see cref="StateObject"/>s that gets executed when the current <see cref="StateObject"/> is exited.
        /// </summary>
        protected virtual void OnExit() { }

    }
}

