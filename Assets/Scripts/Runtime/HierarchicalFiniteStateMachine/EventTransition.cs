using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HFSM {
    internal class EventTransition : Transition {

        private bool eventListened;

        public EventTransition(StateObject originSateObject,
            StateObject targetStateObject, Action transitionAction, params Func<bool>[] conditions) :
            base(originSateObject, targetStateObject, transitionAction, conditions) {
        }



        private void EventLogic() {
            if (OriginStateObject.IsActive ||
               (OriginStateObject.GetType() == typeof(State.Any) && OriginStateObject.StateMachine.IsActive)) {

                eventListened = true;
            }
        }

        #region Generic parameter callback Methods
        public void ListenEvent() {
            EventLogic();
        }

        public void ListenEvent<T>(T arg1) {
            EventLogic();
        }

        public void ListenEvent<T1, T2>(T1 arg1, T2 arg2) {
            EventLogic();
        }

        public void ListenEvent<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) {
            EventLogic();
        }

        #endregion
        public void ConsumeEvent() {
            eventListened = false;
        }

        public override bool AllConditionsMet() {
            return eventListened && base.AllConditionsMet();
        }
    }

}

