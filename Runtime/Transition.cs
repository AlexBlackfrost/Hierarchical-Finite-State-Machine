using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HFSM {
    internal class Transition {
        internal StateObject OriginStateObject { get; private set; }
        internal StateObject TargetStateObject { get; private set; }
        internal Action TransitionAction { get; private set; }

        private Func<bool>[] conditions;
        
    
        public Transition(StateObject originSateObject, StateObject targetStateObject, Action transitionAction = null,
             params Func<bool>[] conditions) {

            OriginStateObject = originSateObject;
            TargetStateObject = targetStateObject;
            this.conditions = conditions;
            TransitionAction = transitionAction;
        }

        public virtual bool AllConditionsMet() {
            foreach (Func<bool> condition in conditions) {
                if (!condition()) {
                    return false;
                }
            }
            return true;
        }

        public virtual void InvokeTransitionAction() {
            TransitionAction?.Invoke();
        }
    }

}