using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HFSM {
    internal class EventTransition : EventTransitionBase {
        private Func<bool>[] conditions;
        public EventTransition(StateObject originSateObject,
            StateObject targetStateObject, Action transitionAction, params Func<bool>[] conditions) :
            base(originSateObject, targetStateObject, transitionAction) {

            this.conditions = conditions;
        }

        public void ListenEvent() {
            if (OriginStateObject.IsActive ||
               (OriginStateObject.GetType() == typeof(State.Any) && OriginStateObject.StateMachine.IsActive)) {

                eventListened = true;
            }
        }

        private protected override bool ConditionsMet() {
            bool conditionsMet = true;
            foreach (Func<bool> condition in conditions) {
                if (!condition()) {
                    conditionsMet = false;
                    break;
                }
            }
            return conditionsMet;
        }
    }

    internal class EventTransition<T> : EventTransitionBase {
        private List<T> args; // An event can be fired more than once per frame. Cache the args from every fired event.
        private Action<T> transitionAction;
        private Func<T, bool>[] conditions;
        private T currentArg;

        public EventTransition(StateObject originSateObject,
            StateObject targetStateObject, Action<T> transitionAction, params Func<T, bool>[] conditions) :
            base(originSateObject, targetStateObject) {

            this.conditions = conditions;
            this.transitionAction = transitionAction;
            args = new List<T>();
            currentArg = default(T);
        }

        public override void ConsumeEvent() {
            args.Clear();
            currentArg = default(T);
            base.ConsumeEvent();
        }

        public void ListenEvent(T arg) {
            if (OriginStateObject.IsActive ||
               (OriginStateObject.GetType() == typeof(State.Any) && OriginStateObject.StateMachine.IsActive)) {

                eventListened = true;
                args.Add(arg);
            }
        }

        private protected override bool ConditionsMet() {
            bool conditionsMet = true;
            foreach(T arg in args) {
                currentArg = arg;
                foreach (Func<T, bool> condition in conditions) {
                    if (!condition(arg)) {
                        conditionsMet = false;
                        break;
                    }
                }
                if (conditionsMet) {
                    break;
                }
            }
            return conditionsMet;
        }

        public override void InvokeTransitionAction() {
            transitionAction?.Invoke(currentArg); // Execute the transition action using the first event argument that met all the conditions
        } 
    }

    internal class EventTransition<T1, T2> : EventTransitionBase {
        private List<(T1, T2)> args; // An event can be fired more than once per frame. Cache the args from every fired event.
        private Func<T1, T2, bool>[] conditions;
        private Action<T1, T2> transitionAction;
        private (T1, T2) currentArgs;

        public EventTransition(StateObject originSateObject,
            StateObject targetStateObject, Action<T1, T2> transitionAction, params Func<T1, T2, bool>[] conditions) :
            base(originSateObject, targetStateObject) {

            this.conditions = conditions;
            this.transitionAction = transitionAction;
            args = new List<(T1, T2)>();
            currentArgs = default((T1, T2));
        }

        public override void ConsumeEvent() {
            args.Clear();
            currentArgs = default((T1, T2));
            base.ConsumeEvent();
        }

        public void ListenEvent(T1 arg1, T2 arg2) {
            if (OriginStateObject.IsActive ||
               (OriginStateObject.GetType() == typeof(State.Any) && OriginStateObject.StateMachine.IsActive)) {

                eventListened = true;
                this.args.Add((arg1, arg2));
            }
        }
        private protected override bool ConditionsMet() {
            bool conditionsMet = true;
            foreach((T1, T2) argTuple in args) {
                currentArgs = argTuple;
                foreach (Func<T1, T2, bool> condition in conditions) {
                    if (!condition(currentArgs.Item1, currentArgs.Item2)) {
                        conditionsMet = false;
                        break;
                    }
                }
                if (conditionsMet) {
                    break;
                }
            }
            return conditionsMet;
        }

        public override void InvokeTransitionAction() {
            transitionAction?.Invoke(currentArgs.Item1, currentArgs.Item2);
        }
    }

    internal class EventTransition<T1, T2, T3> : EventTransitionBase {
        private List<(T1, T2, T3)> args;
        private (T1, T2, T3) currentArgs;
        private Func<T1, T2, T3, bool>[] conditions;
        private Action<T1, T2, T3> transitionAction;


        public EventTransition(StateObject originSateObject,
            StateObject targetStateObject, Action<T1, T2, T3> transitionAction, params Func<T1, T2, T3, bool>[] conditions) :
            base(originSateObject, targetStateObject) {

            this.conditions = conditions;
            this.transitionAction = transitionAction;
            args = new List<(T1, T2, T3)>();
            currentArgs = default((T1, T2, T3));
        }

        public override void ConsumeEvent() {
            args.Clear();
            currentArgs = default((T1, T2, T3));
            base.ConsumeEvent();
        }

        public void ListenEvent(T1 arg1, T2 arg2, T3 arg3) {
            if (OriginStateObject.IsActive ||
               (OriginStateObject.GetType() == typeof(State.Any) && OriginStateObject.StateMachine.IsActive)) {

                eventListened = true;
                args.Add( (arg1, arg2, arg3) );
            }
        }

        private protected override bool ConditionsMet() {
            bool conditionsMet = true;
            foreach((T1, T2, T3) argTuple in args) {
                currentArgs = argTuple;
                foreach (Func<T1, T2, T3, bool> condition in conditions) {
                    if (!condition(argTuple.Item1, argTuple.Item2, argTuple.Item3)) {
                        conditionsMet = false;
                        break;
                    }
                }
                if (conditionsMet) {
                    break;
                }
            }
            return conditionsMet;
        }
        public override void InvokeTransitionAction() {
            transitionAction?.Invoke(currentArgs.Item1, currentArgs.Item2, currentArgs.Item3);
        }
    }

}

