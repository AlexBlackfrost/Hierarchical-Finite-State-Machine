using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HFSM {
    /// <summary>
    /// Transition behaviour of a hierarchical finite state machine pattern.
    /// </summary>
    internal class Transition {
        internal StateObject OriginStateObject { get; private set; }
        internal StateObject TargetStateObject { get; private set; }
        internal Action TransitionAction { get; private set; }

        private Func<bool>[] conditions;

        /// <summary>
        /// Transition class constructor.
        /// </summary>
        /// <param name="originSateObject">
        /// Origin <see cref="StateObject"/> of the transition.
        /// </param>
        /// <param name="targetStateObject">
        /// Target <see cref="StateObject"/> of the transition.
        /// </param>
        /// <param name="transitionAction">
        /// Function executed when the transition occurs.
        /// </param>
        /// <param name="conditions">
        /// List of conditions that must be met (all of them) in order for the transition to occur.
        /// </param>
        public Transition(StateObject originSateObject, StateObject targetStateObject, Action transitionAction = null,
             params Func<bool>[] conditions) {

            OriginStateObject = originSateObject;
            TargetStateObject = targetStateObject;
            this.conditions = conditions;
            TransitionAction = transitionAction;
        }

        /// <summary>
        /// Checks whether all <see cref="Transition.conditions"/> are met or not.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if all <see cref="Transition.conditions"/> are met, <see langword="false"/> otherwise.
        /// </returns>
        public virtual bool AllConditionsMet() {
            foreach (Func<bool> condition in conditions) {
                if (!condition()) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Executes <see cref="Transition.TransitionAction"/>, if defined.
        /// </summary>
        public virtual void InvokeTransitionAction() {
            TransitionAction?.Invoke();
        }
    }
}