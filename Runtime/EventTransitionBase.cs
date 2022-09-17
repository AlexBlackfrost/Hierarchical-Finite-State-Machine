using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFSM {
    internal abstract class EventTransitionBase : Transition {

        private protected bool eventListened;
        public EventTransitionBase(StateObject originSateObject, StateObject targetStateObject, Action transitionAction = null) : 
            base(originSateObject, targetStateObject, transitionAction) {
        }

        public virtual void ConsumeEvent() {
            eventListened = false;
        }

        public override bool AllConditionsMet() {
            return eventListened && ConditionsMet();
        }

        private protected abstract bool ConditionsMet();
    }

}
