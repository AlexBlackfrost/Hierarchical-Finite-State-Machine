using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    internal class EventTransition : Transition {

        private bool eventListened;
        public EventTransition(Action transitionEvent, StateObject originSateObject,
            StateObject targetStateObject, params Func<bool>[] conditions) :
            base(originSateObject, targetStateObject, conditions) {

            transitionEvent += ListenEvent;
        }

        private void ListenEvent() {
            if (OriginStateObject.StateMachine?.CurrentStateObject == OriginStateObject) {
                eventListened = true;
            }
        }

        public void ConsumeEvent() {
            eventListened = false;
        }

        public override bool AllConditionsMet() {
            return eventListened && base.AllConditionsMet();
        }
    }



