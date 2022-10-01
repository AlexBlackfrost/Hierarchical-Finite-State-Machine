using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HFSM {
    /// <inheritdoc cref="EventTransitionBase"/>
    internal class EventTransition : EventTransitionBase {
        private Func<bool>[] conditions;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="originSateObject">
        /// Origin <see cref="StateObject"/> of the transition.
        /// </param>
        /// <param name="targetStateObject">
        /// Target <see cref="StateObject"/> of the transition.
        /// </param>
        /// <param name="transitionAction">
        /// Function executed (if defined) when the transition occurs.
        /// </param>
        /// <param name="processInstantly">
        /// Whether to process this transition as soon as the event that it is subscribed to is fired or not.
        /// </param>
        /// <param name="conditions">
        /// List of conditions that must be met (all of them) in order for the transition to occur.
        /// </param>
        public EventTransition(StateObject originSateObject,
            StateObject targetStateObject, Action transitionAction, bool processInstantly, params Func<bool>[] conditions) :
            base(originSateObject, targetStateObject, processInstantly, transitionAction) {

            this.conditions = conditions;
        }

        /// <summary>
        /// Listens to the event this transition is subscribed to. It only listens to the event if
        /// <see cref="Transition.OriginStateObject"/> is active or if <see cref="Transition.OriginStateObject"/> is
        /// "Any" and <see cref="Transition.OriginStateObject"/>'s <see cref="StateMachine"/> is active.
        /// </summary>
        public void ListenEvent() {
            if (OriginStateObject.IsActive ||
               (OriginStateObject.GetType() == typeof(State.Any) && OriginStateObject.StateMachine.IsActive)) {

                eventListened = true;
            }
        }

        /// <inheritdoc cref="EventTransitionBase.ConditionsMet"/>
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

    /// <inheritdoc cref="EventTransitionBase"/>
    internal class EventTransition<T> : EventTransitionBase {
        private List<T> args; // An event can be fired more than once per frame. Cache the args from every fired event.
        private Action<T> transitionAction;
        private Func<T, bool>[] conditions;
        private T currentArg;

        /// <inheritdoc cref="EventTransition.EventTransition(StateObject, StateObject, Action, bool, Func{bool}[])"/>
        public EventTransition(StateObject originSateObject,
            StateObject targetStateObject, Action<T> transitionAction, bool processInstantly, params Func<T, bool>[] conditions) :
            base(originSateObject, targetStateObject, processInstantly) {

            this.conditions = conditions;
            this.transitionAction = transitionAction;
            args = new List<T>();
            currentArg = default(T);
        }

        /// <inheritdoc cref="EventTransitionBase.ConsumeEvent"/>
        public override void ConsumeEvent() {
            args.Clear();
            currentArg = default(T);
            base.ConsumeEvent();
        }

        /// <inheritdoc cref="EventTransition.ListenEvent"/>
        public void ListenEvent(T arg) {
            if (OriginStateObject.IsActive ||
               (OriginStateObject.GetType() == typeof(State.Any) && OriginStateObject.StateMachine.IsActive)) {

                eventListened = true;
                args.Add(arg);
            }
        }

        /// <inheritdoc cref="EventTransitionBase.ConditionsMet"/>
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

        /// <inheritdoc cref="Transition.InvokeTransitionAction"/>
        public override void InvokeTransitionAction() {
            transitionAction?.Invoke(currentArg); // Execute the transition action using the first event argument that met all the conditions
        } 
    }

    /// <inheritdoc cref="EventTransitionBase"/>
    internal class EventTransition<T1, T2> : EventTransitionBase {
        private List<(T1, T2)> args; // An event can be fired more than once per frame. Cache the args from every fired event.
        private Func<T1, T2, bool>[] conditions;
        private Action<T1, T2> transitionAction;
        private (T1, T2) currentArgs;

        /// <inheritdoc cref="EventTransition.EventTransition(StateObject, StateObject, Action, bool, Func{bool}[])"/>
        public EventTransition(StateObject originSateObject,
            StateObject targetStateObject, Action<T1, T2> transitionAction, bool processInstantly, params Func<T1, T2, bool>[] conditions) :
            base(originSateObject, targetStateObject, processInstantly) {

            this.conditions = conditions;
            this.transitionAction = transitionAction;
            args = new List<(T1, T2)>();
            currentArgs = default((T1, T2));
        }

        /// <inheritdoc cref="EventTransitionBase.ConsumeEvent"/>
        public override void ConsumeEvent() {
            args.Clear();
            currentArgs = default((T1, T2));
            base.ConsumeEvent();
        }

        /// <inheritdoc cref="EventTransition.ListenEvent"/>
        public void ListenEvent(T1 arg1, T2 arg2) {
            if (OriginStateObject.IsActive ||
               (OriginStateObject.GetType() == typeof(State.Any) && OriginStateObject.StateMachine.IsActive)) {

                eventListened = true;
                this.args.Add((arg1, arg2));
            }
        }

        /// <inheritdoc cref="EventTransitionBase.ConditionsMet"/>
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

        /// <inheritdoc cref="Transition.InvokeTransitionAction"/>
        public override void InvokeTransitionAction() {
            transitionAction?.Invoke(currentArgs.Item1, currentArgs.Item2);
        }
    }

    /// <inheritdoc cref="EventTransitionBase"/>
    internal class EventTransition<T1, T2, T3> : EventTransitionBase {
        private List<(T1, T2, T3)> args;
        private (T1, T2, T3) currentArgs;
        private Func<T1, T2, T3, bool>[] conditions;
        private Action<T1, T2, T3> transitionAction;

        /// <inheritdoc cref="EventTransition.EventTransition(StateObject, StateObject, Action, bool, Func{bool}[])"/>
        public EventTransition(StateObject originSateObject,
            StateObject targetStateObject, Action<T1, T2, T3> transitionAction, bool processInstantly = false, params Func<T1, T2, T3, bool>[] conditions) :
            base(originSateObject, targetStateObject, processInstantly) {

            this.conditions = conditions;
            this.transitionAction = transitionAction;
            args = new List<(T1, T2, T3)>();
            currentArgs = default((T1, T2, T3));
        }

        /// <inheritdoc cref="EventTransitionBase.ConsumeEvent"/>
        public override void ConsumeEvent() {
            args.Clear();
            currentArgs = default((T1, T2, T3));
            base.ConsumeEvent();
        }

        /// <inheritdoc cref="EventTransition.ListenEvent"/>
        public void ListenEvent(T1 arg1, T2 arg2, T3 arg3) {
            if (processInstantly) {
                OriginStateObject.StateMachine.ProcessInstantEvent(this);

            } else if (OriginStateObject.IsActive ||
               (OriginStateObject.GetType() == typeof(State.Any) && OriginStateObject.StateMachine.IsActive)) {

                eventListened = true;
                args.Add( (arg1, arg2, arg3) );
                
            }
        }

        /// <inheritdoc cref="EventTransitionBase.ConditionsMet"/>
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

        /// <inheritdoc cref="Transition.InvokeTransitionAction"/>
        public override void InvokeTransitionAction() {
            transitionAction?.Invoke(currentArgs.Item1, currentArgs.Item2, currentArgs.Item3);
        }
    }
}

