using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    internal class Transition {
        public StateObject OriginStateObject { get; private set; }
        public StateObject TargetStateObject { get; private set; }

        private Func<bool>[] conditions;

        public Transition(StateObject originSateObject, StateObject targetStateObject, params Func<bool>[] conditions) {
            OriginStateObject = originSateObject;
            TargetStateObject = targetStateObject;
            this.conditions = conditions;
        }

        public virtual bool AllConditionsMet() {
            foreach (Func<bool> condition in conditions) {
                if (!condition()) {
                    return false;
                }
            }
            return true;
        }
    }

