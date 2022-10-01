using System;
using System.Collections.Generic;
using UnityEngine;


namespace HFSM {
    /// <summary>
	/// Hierarchical finite state machine.
	/// </summary>
    public abstract class StateMachine : StateObject {
        public StateObject DefaultStateObject { get; set; }
        public StateObject CurrentStateObject { get; private set; }
        /// <summary>
        /// A linked list of <see cref="StateMachine"/> starting from the root <see cref="StateMachine"/>. 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
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
        private List<EventTransitionBase> anyEventTransitions;
        private bool initialized;
        private State anyState;

        /// <summary>
        /// Class constructor. Creates a <see cref="StateMachine"/> and initializes it. Throws
        /// <see cref="StatelessStateMachineException"/> if no <see cref="StateObject"/> is passed as argument.
        /// </summary>
        /// <param name="stateObjects">
        /// List of state objects inside this <see cref="HFSM.StateMachine"/>. <see cref="StateObject"/>s inherit from <see cref="StateMachine"/>StateMachine
        /// or <see cref="State"/> classes.
        /// </param>
        /// <exception cref="StatelessStateMachineException">
        /// Thrown when no <see cref="StateObject"/> is passed as argument.
        /// </exception>
        public StateMachine(params StateObject[] stateObjects) : base() {
            if (stateObjects.Length == 0) {
                throw new StatelessStateMachineException(
                    "A State Machine must have at least one state object." +
                    " State machine of type '" + GetType() + "' does not have any state objects."
                );
            }
            anyTransitions = new List<Transition>();
            anyEventTransitions = new List<EventTransitionBase>();
            anyState = new State.Any();
            anyState.StateMachine = this;
            initialized = false;

            DefaultStateObject = stateObjects[0];
            foreach (StateObject stateObject in stateObjects) {
                stateObject.StateMachine = this;
            }
        }

        /// <summary>
        /// Initializes the root <see cref="StateMachine"/> by calling <see cref="Enter"/> method.
        /// </summary>
        /// <seealso cref="Enter"/>
        public void Init() {
            if (IsRoot()) {
                Enter();
                initialized = true;
            }
        }

        /// <summary>
        /// Checks whether <see cref="Init"/> has not been on called on the root <see cref="StateMachine"/> object
        /// </summary>
        /// <exception cref="RootStateMachineNotInitializedException">
        /// Thrown if <see cref="Init"/> has not been on called on the root <see cref="StateMachine"/> object.
        /// </exception>
        private void CheckInitialization() {
            if (IsRoot() && !initialized) {
                throw new RootStateMachineNotInitializedException("Root State Machine has not been initialized." +
                    " Call rootStateMachine.Init() to initialize it");
            }
        }

        #region AddAnyTransition methods
        /// <summary>
        /// Adds a <see cref="Transition"/> from any <see cref="StateObject"/> inside this <see cref="StateMachine"/> 
        /// to <paramref name="targetStateObject"/>. In order to change to <paramref name="targetStateObject"/>, 
        /// all <paramref name="conditions"/> (if any) must return <see langword="true"/>. Transitions added first 
        /// have higher priority.
        /// </summary>
        /// <param name = "targetStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name = "conditions">
        /// The list of conditions that must be met in order to change to <paramref name="targetStateObject"/>.
        /// </param>
        public void AddAnyTransition(StateObject targetStateObject, params Func<bool>[] conditions) {
            Transition transition = new Transition(anyState, targetStateObject, null, conditions);
            TryRegisterAnyTransition(transition);

        }

        /// <summary>
        /// Adds a <see cref="Transition"/> from any <see cref="StateObject"/> inside this <see cref="StateMachine"/> 
        /// to <paramref name="targetStateObject"/>. In order to change to <paramref name="targetStateObject"/>, 
        /// all <paramref name="conditions"/> (if any) must return <see langword="true"/>. Transitions added first 
        /// have higher priority.
        /// </summary>
        /// <param name = "targetStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name="transitionAction">
        /// Function to be executed if the transition occurs. It is executed after current <see cref="StateObject"/> 
        /// is exited and before the new one is entered.
        /// </param>
        /// <param name = "conditions">
        /// The list of conditions that must be met in order to change to <paramref name="targetStateObject"/>.
        /// </param>
        public void AddAnyTransition(StateObject targetStateObject, Action transitionAction,
            params Func<bool>[] conditions) {
            Transition transition = new Transition(anyState, targetStateObject, transitionAction, conditions);
            TryRegisterAnyTransition(transition);
        }

        /// <summary>
        /// Adds an <see cref="EventTransition"/> to <paramref name="targetStateObject"/>.
        /// In order to change to <paramref name="targetStateObject"/>, the event must have been fired and
        /// all <paramref name="conditions"/> (if any) must return <see langword="true"/>. Transitions added first have higher priority.
        /// All <see cref="EventTransition"/>s are processed together with <see cref="Transition"/>s (polling transitions)
        /// at the same time in the execution flow.
        /// Set <paramref name="processInstantly"/> parameter to <see langword="true"/> if you want to process transition events as soon as
        /// the event is listened. Events are only listened if the origin <see cref="StateObject"/> is active.
        /// </summary>
        /// <param name="targetStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name="conditions">
        /// The list of conditions that must be met in order to change to a new <see cref="StateObject"/>.
        /// </param>
        /// <returns>
        /// An event listener function that must be subscribed to an event.
        /// </returns>
        public Action AddAnyEventTransition(StateObject targetStateObject, params Func<bool>[] conditions) {
            EventTransition transition = new EventTransition(anyState, targetStateObject, null, false, conditions);

            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        /// <summary>
        /// Adds an <see cref="EventTransition"/> to <paramref name="targetStateObject"/>.
        /// In order to change to <paramref name="targetStateObject"/>, the event must have been fired and
        /// all <paramref name="conditions"/> (if any) must return <see langword="true"/>. Transitions added first have higher priority.
        /// All <see cref="EventTransition"/>s are processed together with <see cref="Transition"/>s (polling transitions)
        /// at the same time in the execution flow.
        /// Set <paramref name="processInstantly"/> parameter to <see langword="true"/> if you want to process transition events as soon as
        /// the event is listened. Events are only listened if the origin <see cref="StateObject"/> is active.
        /// </summary>
        /// <param name="targetStateObject">
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
        public Action AddAnyEventTransition(StateObject targetStateObject, bool processInstantly, params Func<bool>[] conditions) {
            EventTransition transition = new EventTransition(anyState, targetStateObject, null, processInstantly, conditions);

            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        /// <summary>
        /// Adds an <see cref="EventTransition"/> to <paramref name="targetStateObject"/>.
        /// In order to change to <paramref name="targetStateObject"/>, the event must have been fired and
        /// all <paramref name="conditions"/> (if any) must return <see langword="true"/>. Transitions added first have higher priority.
        /// All <see cref="EventTransition"/>s are processed together with <see cref="Transition"/>s (polling transitions)
        /// at the same time in the execution flow.
        /// Set <paramref name="processInstantly"/> parameter to <see langword="true"/> if you want to process transition events as soon as
        /// the event is listened. Events are only listened if the origin <see cref="StateObject"/> is active.
        /// </summary>
        /// <param name="targetStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name="transitionAction">
        /// Function to be executed if the transition occurs. It is executed after current <see cref="StateObject"/> 
        /// is exited and before the new one is entered.
        /// </param>
        /// <param name="conditions">
        /// The list of conditions that must be met in order to change to a new <see cref="StateObject"/>.
        /// </param>
        /// <returns>
        /// An event listener function that must be subscribed to an event.
        /// </returns>
        public Action AddAnyEventTransition(StateObject targetStateObject, Action transitionAction, params Func<bool>[] conditions) {
            EventTransition transition = new EventTransition(anyState, targetStateObject, transitionAction, false, conditions);

            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        /// <summary>
        /// Adds an <see cref="EventTransition"/> to <paramref name="targetStateObject"/>.
        /// In order to change to <paramref name="targetStateObject"/>, the event must have been fired and
        /// all <paramref name="conditions"/> (if any) must return <see langword="true"/>. Transitions added first have higher priority.
        /// All <see cref="EventTransition"/>s are processed together with <see cref="Transition"/>s (polling transitions)
        /// at the same time in the execution flow.
        /// Set <paramref name="processInstantly"/> parameter to <see langword="true"/> if you want to process transition events as soon as
        /// the event is listened. Events are only listened if the origin <see cref="StateObject"/> is active.
        /// </summary>
        /// <param name="targetStateObject">
        /// The next <see cref="StateObject"/> to be executed after the transition is completed.
        /// </param>
        /// <param name="transitionAction">
        /// Function to be executed if the transition occurs. It is executed after current <see cref="StateObject"/> 
        /// is exited and before the new one is entered.
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
        public Action AddAnyEventTransition(StateObject targetStateObject, Action transitionAction, bool processInstantly, params Func<bool>[] conditions) {
            EventTransition transition = new EventTransition(anyState, targetStateObject, transitionAction, processInstantly, conditions);

            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddAnyEventTransition(StateObject, Func{bool}[])"/>
        public Action<T> AddAnyEventTransition<T>(StateObject targetStateObject, params Func<T, bool>[] conditions) {
            EventTransition<T> transition = new EventTransition<T>(anyState, targetStateObject, null, false, conditions);

            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        ///<inheritdoc cref="AddAnyEventTransition(StateObject, bool, Func{bool}[])"/>
        public Action<T> AddAnyEventTransition<T>(StateObject targetStateObject, bool processInstantly, params Func<T, bool>[] conditions) {
            EventTransition<T> transition = new EventTransition<T>(anyState, targetStateObject, null, processInstantly, conditions);

            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        /// <inheritdoc cref="AddAnyEventTransition(StateObject, Action, Func{bool}[])"/>
        public Action<T> AddAnyEventTransition<T>(StateObject targetStateObject, Action<T> transitionAction, params Func<T, bool>[] conditions) {

            EventTransition<T> transition = new EventTransition<T>(anyState, targetStateObject, transitionAction, false, conditions);
            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        ///<inheritdoc cref="AddAnyEventTransition(StateObject, Action, bool, Func{bool}[])"/>
        public Action<T> AddAnyEventTransition<T>(StateObject targetStateObject, Action<T> transitionAction,
            bool processInstantly, params Func<T, bool>[] conditions) {

            EventTransition<T> transition = new EventTransition<T>(anyState, targetStateObject, transitionAction, processInstantly, conditions);
            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        ///<inheritdoc cref="AddAnyEventTransition(StateObject, Func{bool}[])"/>
        public Action<T1, T2> AddAnyEventTransition<T1, T2>(StateObject targetStateObject, params Func<T1, T2, bool>[] conditions) {

            EventTransition<T1, T2> transition = new EventTransition<T1, T2>(anyState, targetStateObject, null, false, conditions);
            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        ///<inheritdoc cref="AddAnyEventTransition(StateObject, bool, Func{bool}[])"/>
        public Action<T1, T2> AddAnyEventTransition<T1, T2>(StateObject targetStateObject, bool processInstantly, params Func<T1, T2, bool>[] conditions) {

            EventTransition<T1, T2> transition = new EventTransition<T1, T2>(anyState, targetStateObject, null, processInstantly, conditions);
            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        ///<inheritdoc cref="AddAnyEventTransition(StateObject, Action, Func{bool}[])"/>
        public Action<T1, T2> AddAnyEventTransition<T1, T2>(StateObject targetStateObject,
            Action<T1, T2> transitionAction, params Func<T1, T2, bool>[] conditions) {

            EventTransition<T1, T2> transition = new EventTransition<T1, T2>(anyState, targetStateObject, transitionAction, false, conditions);
            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        ///<inheritdoc cref="AddAnyEventTransition(StateObject, Action, bool, Func{bool}[])"/>
        public Action<T1, T2> AddAnyEventTransition<T1, T2>(StateObject targetStateObject,
            Action<T1, T2> transitionAction, bool processInstantly, params Func<T1, T2, bool>[] conditions) {

            EventTransition<T1, T2> transition = new EventTransition<T1, T2>(anyState, targetStateObject, transitionAction, processInstantly, conditions);
            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        ///<inheritdoc cref="AddAnyEventTransition(StateObject, Func{bool}[])"/>
        public Action<T1, T2, T3> AddAnyEventTransition<T1, T2, T3>(StateObject targetStateObject, params Func<T1, T2, T3, bool>[] conditions) {

            EventTransition<T1, T2, T3> transition = new EventTransition<T1, T2, T3>(anyState, targetStateObject, null, false, conditions);
            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        ///<inheritdoc cref="AddAnyEventTransition(StateObject, bool, Func{bool}[])"/>
        public Action<T1, T2, T3> AddAnyEventTransition<T1, T2, T3>(StateObject targetStateObject, bool processInstantly, params Func<T1, T2, T3, bool>[] conditions) {

            EventTransition<T1, T2, T3> transition = new EventTransition<T1, T2, T3>(anyState, targetStateObject, null, processInstantly, conditions);
            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        ///<inheritdoc cref="AddAnyEventTransition(StateObject, Action, bool, Func{bool}[])"/>
        public Action<T1, T2, T3> AddAnyEventTransition<T1, T2, T3>(StateObject targetStateObject,
            Action<T1, T2, T3> transitionAction, params Func<T1, T2, T3, bool>[] conditions) {

            EventTransition<T1, T2, T3> transition = new EventTransition<T1, T2, T3>(anyState, targetStateObject, transitionAction, false, conditions);
            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        ///<inheritdoc cref="AddAnyEventTransition(StateObject, Action, bool, Func{bool}[])"/>
        public Action<T1, T2, T3> AddAnyEventTransition<T1, T2, T3>(StateObject targetStateObject,
            Action<T1, T2, T3> transitionAction, bool processInstantly, params Func<T1, T2, T3, bool>[] conditions) {

            EventTransition<T1, T2, T3> transition = new EventTransition<T1, T2, T3>(anyState, targetStateObject, transitionAction, processInstantly, conditions);
            TryRegisterAnyTransition(transition);
            anyEventTransitions.Add(transition);
            return transition.ListenEvent;
        }

        /// <summary>
        /// Tries to store a <see cref="Transition"/> from any <see cref="StateObject"/>. It will throw
        /// <see cref="NoCommonParentStateMachineException"/> if the <see cref="StateObject"/>s inside
        /// this <see cref="StateMachine"/> and <see cref="Transition.TargetStateObject"/> don't have
        /// a common <see cref="StateMachine"/> ancestor.
        /// </summary>
        /// <param name="anyTransition">
        /// The <see cref="Transition"/> to be registered.
        /// </param>
        private void TryRegisterAnyTransition(Transition anyTransition) {
            if (!HaveCommonStateMachineAncestor(anyTransition.OriginStateObject,
                anyTransition.TargetStateObject)) {

                throw new NoCommonParentStateMachineException(
                    "States inside " + typeof(StateMachine).ToString() + " of type " + this.GetType() + " and state object of " +
                    "type " + anyTransition.TargetStateObject.GetType() + " don't have a common " +
                    "parent state machine."
                );
            } else {
                anyTransitions.Add(anyTransition);
            }
        }
        #endregion

        /// <summary>
        /// Tries to find an available <see cref="Transition"/> and then use it to change to a new
        /// <see cref="StateObject"/>.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the current <see cref="StateObject"/> was changed, <see langword="false"/> otherwise. 
        /// </returns>
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
            if (availableTransition == null) {
                availableTransition = CurrentStateObject.GetAvailableTransition();
            }

            foreach (EventTransition anyEventTransition in anyEventTransitions) {
                anyEventTransition.ConsumeEvent();
            }

            if (availableTransition != null) {
                ChangeState(availableTransition);
                changedState = true;
            }

            CurrentStateObject.ConsumeTransitionsEvents();
            return changedState;
        }

        /// <summary>
        /// Consumes all the events listened by <see cref="EventTransition"/>s that have
        /// this <see cref="StateMachine"/> as their <see cref="Transition.OriginStateObject"/> and
        /// all the events listened by <see cref="EventTransition"/>s whose <see cref="Transition.OriginStateObject"/> is any
        /// of the <see cref="StateObject"/>s nested in this <see cref="StateMachine"/>.
        /// </summary>
        /// <seealso cref="AddAnyEventTransition(StateObject, Func{bool}[])"/>
        internal override sealed void ConsumeTransitionsEvents() {
            foreach (EventTransitionBase anyEventTransition in anyEventTransitions) {
                anyEventTransition.ConsumeEvent();
            }

            foreach (EventTransitionBase eventTransition in eventTransitions) {
                eventTransition.ConsumeEvent();
            }

            CurrentStateObject.ConsumeTransitionsEvents();
        }

        /// <summary>
        /// Finds the highest priority available <see cref="Transition"/> in this <see cref="StateMachine"/>.
        /// <see cref="Transition"/>s added first have higher priority. If no available <see cref="Transition"/> is found
        /// it returns the available <see cref="Transition"/>s from its current <see cref="StateObject"/>.
        /// </summary>
        /// <returns>
        /// The highest priority available <see cref="Transition"/> if any, <see langword="null"/> otherwise.
        /// </returns>
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

        /// <summary>
        /// Sets <paramref name="availableTransition"/>'s <see cref="Transition.TargetStateObject"/> as the current
        /// <see cref="StateObject"/> of this <see cref="StateMachine"/>. <see cref="StateObject.Exit"/>, 
        /// <see cref="Transition.TransitionAction"/> (if specified) and <see cref="StateObject.Enter"/> are executed
        /// in that order.
        /// </summary>
        /// <param name="availableTransition"></param>
        private void ChangeState(Transition availableTransition) {
            StateObject originStateObject = availableTransition.OriginStateObject;
            StateObject targetStateObject = availableTransition.TargetStateObject;

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

            availableTransition.InvokeTransitionAction();
            lowestCommonStateMachine.CurrentStateObject.Enter();
        }

        /// <summary>
        /// Evaluates the conditions of <paramref name="eventTransition"/> instantly without waiting
        /// for the next update cycle and performs the transition if all the conditions are met. 
        /// </summary>
        /// <param name="eventTransition">
        /// The transtion to be performed.
        /// </param>
        internal void ProcessInstantEvent(EventTransitionBase eventTransition) {
            StateObject originStateObject = eventTransition.OriginStateObject;
            if (originStateObject.IsActive ||
                (originStateObject.GetType() == typeof(State.Any) && originStateObject.StateMachine.IsActive) &&
                eventTransition.AllConditionsMet()) {

                ChangeState(eventTransition);
            }
        }

        /// <summary>
        /// Executes the code needed to implement the state machine beahviour of a 
        /// hierarchical finite state machine pattern as well as the update cycle code defined
        /// in the extended classes.
        /// </summary>
        public sealed override void Update() {
            CheckInitialization();
            bool changedState = TryChangeState();
            if (!changedState) {
                OnUpdate();
                CurrentStateObject.UpdateInternal();
            }
        }

        /// <summary>
        /// Executes the code needed to implement the state machine behaviour of a
        /// hiearchical finite state machine pattern.
        /// </summary>
        internal sealed override void UpdateInternal() {
            OnUpdate();
            CurrentStateObject.UpdateInternal();
        }

        /// <summary>
        /// Executes the code needed to implement the state machine beahviour of a 
        /// hierarchical finite state machine pattern as well as the fixed update cycle code defined
        /// in the extended classes.
        /// </summary>
        public sealed override void FixedUpdate() {
            CheckInitialization();
            OnFixedUpdate();
            CurrentStateObject.FixedUpdate();
        }

        /// <summary>
        /// Executes the code needed to implement the state machine beahviour of a 
        /// hierarchical finite state machine pattern as well as the late update cycle code defined
        /// in the extended classes.
        /// </summary>
        public sealed override void LateUpdate() {
            CheckInitialization();
            OnLateUpdate();
            CurrentStateObject.LateUpdate();
        }

        /// <summary>
        /// Executes the code needed to implement the state machine beahviour of a 
        /// hierarchical finite state machine pattern as well as the logic defined in the extended classes.
        /// This function is called the first update cycle after this <see cref="StateMachine"/> has become active.
        /// The hierarchical execution of <see cref="Enter"/> is performed in a top-down fashion.
        /// </summary>
        internal sealed override void Enter() {
            IsActive = true;
            if (CurrentStateObject == null) {
                CurrentStateObject = DefaultStateObject;
            }

            OnEnter();
            CurrentStateObject.Enter();
        }

        /// <summary>
        /// Executes the code needed to implement the state machine beahviour of a 
        /// hierarchical finite state machine pattern as well as the logic defined in the extended classes.
        /// This function is called the last update cycle before this <see cref="StateMachine"/> becomes inactive.
        /// The hierarchical execution of <see cref="Exit"/> is performed in a bottom-up fashion.
        /// </summary>
        internal sealed override void Exit() {
            CurrentStateObject.Exit();
            OnExit();
            IsActive = false;
            CurrentStateObject = null;
        }

        /// <summary>
        /// Returns the hierarchy of active <see cref="StateObject"/>s starting from the root state machine.
        /// </summary>
        /// <returns>
        /// The hierarchy of active <see cref="StateObject"/>s converted to string.
        /// </returns>
        public sealed override string GetCurrentStateName() {
            string name = GetType().ToString() + ".";
            if (CurrentStateObject == null) {
                name += "None";
            } else {
                name += CurrentStateObject.GetCurrentStateName();
            }
            return name;
        }
    }
}