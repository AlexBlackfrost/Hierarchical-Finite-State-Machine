using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFSM {
    /// <summary>
    /// Base class for <see cref="EventTransition"/> classes. Implements the same behaviour of a
    /// <see cref="Transition"/> and listens to events.
    /// </summary>
    internal abstract class EventTransitionBase : Transition {

        private protected bool eventListened;
        private protected bool processInstantly;
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
        public EventTransitionBase(StateObject originSateObject, StateObject targetStateObject, bool processInstantly = false, Action transitionAction = null) : 
            base(originSateObject, targetStateObject, transitionAction) {

            this.processInstantly = processInstantly;
        }

        public virtual void ConsumeEvent() {
            eventListened = false;
        }

        /// <summary>
        /// Checks whether all <see cref="Transition.conditions"/> (if any) are met and whether the event this transition
        /// is subscribed to has been fired.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if all <see cref="Transition.conditions"/> (if any) are met and the event was fired, <see langword="false"/> otherwise.
        /// </returns>
        public override bool AllConditionsMet() {
            return eventListened && ConditionsMet();
        }

        /// <summary>
        /// Checks whether all <see cref="Transition.conditions"/> (if any) are met.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if all <see cref="Transition.conditions"/> (if any) are met, <see langword="false"/> otherwise.
        /// </returns>
        private protected abstract bool ConditionsMet();
    }

}
