using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HFSM {
    /// <summary>
    /// State behaviour of a hierarchical finite state machine pattern.
    /// </summary>
    public abstract class State : StateObject {

        public State() : base() { }
        /// <summary>
        /// Consumes all the events listened by <see cref="EventTransition"/>s that have
        /// this <see cref="State"/> as their <see cref="Transition.OriginStateObject"/>.
        /// </summary>
        internal sealed override void ConsumeTransitionsEvents() {
            foreach (EventTransitionBase eventTransition in eventTransitions) {
                eventTransition.ConsumeEvent();
            }
        }

        /// <summary>
        /// Finds the highest priority available <see cref="Transition"/> in this <see cref="State"/>.
        /// <see cref="Transition"/>s added first have higher priority. If no available <see cref="Transition"/> is found
        /// it returns the available <see langword="null"/>.
        /// </summary>
        /// <returns>
        /// The highest priority available <see cref="Transition"/> if any, <see langword="null"/> otherwise.
        /// </returns>
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

        /// <summary>
        /// Executes the code needed to implement the state behaviour of a
        /// hiearchical finite state machine pattern.
        /// </summary>
        internal sealed override void UpdateInternal() {
            OnUpdate();
        }

        /// <summary>
        /// Executes the code needed to implement the state beahviour of a 
        /// hierarchical finite state machine pattern as well as the update cycle code defined
        /// in the extended classes.
        /// </summary>
        public sealed override void Update() {
            UpdateInternal();
        }

        /// <summary>
        /// Executes the code needed to implement the state beahviour of a 
        /// hierarchical finite state machine pattern as well as the fixed update cycle code defined
        /// in the extended classes.
        /// </summary>
        public sealed override void FixedUpdate() {
            OnFixedUpdate();
        }

        /// <summary>
        /// Executes the code needed to implement the state beahviour of a 
        /// hierarchical finite state machine pattern as well as the late update cycle code defined
        /// in the extended classes.
        /// </summary>
        public sealed override void LateUpdate() {
            OnLateUpdate();
        }

        /// <summary>
        /// Executes the code needed to implement the state beahviour of a 
        /// hierarchical finite state machine pattern as well as the logic defined in the extended classes.
        /// This function is called the first update cycle after this <see cref="State"/> has become active.
        /// </summary>
        internal sealed override void Enter() {
            IsActive = true;
            OnEnter();
        }

        /// <summary>
        /// Executes the code needed to implement the state beahviour of a 
        /// hierarchical finite state machine pattern as well as the logic defined in the extended classes.
        /// This function is called the last update cycle before this <see cref="State"/> becomes inactive.
        /// </summary>
        internal sealed override void Exit() {
            IsActive = false;
            OnExit();
        }

        /// <summary>
        /// Returns the type of this <see cref="State"/> converted to string.
        /// </summary>
        /// <returns>
        /// The type of this <see cref="State"/> converted to string.
        /// </returns>
        public sealed override string GetCurrentStateName() { 
            return GetType().ToString(); 
        }

        /// <summary>
        /// Definition of "Any State" used in <see cref="Transition"/>s from whose <see cref="Transition.OriginStateObject"/>
        /// can be any. 
        /// </summary>
        internal class Any : State { }
    }
}