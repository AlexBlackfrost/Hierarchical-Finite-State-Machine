using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


internal class EventTransition : Transition {

    private bool eventListened;

    public EventTransition(StateObject originSateObject,
        StateObject targetStateObject, params Func<bool>[] conditions) :
        base(originSateObject, targetStateObject, conditions) {

    }

    private void EventLogic() {
        if (OriginStateObject.StateMachine?.CurrentStateObject == OriginStateObject) {
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

    public void ListenEvent<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
        EventLogic();
    }

    public void ListenEvent<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
        EventLogic();
    }

    public void ListenEvent<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
        EventLogic();
    }
    public void ListenEvent<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
        EventLogic();
    }

    public void ListenEvent<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
        EventLogic();
    }
    public void ListenEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
        EventLogic();
    }
    public void ListenEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
        EventLogic();
    }

    public void ListenEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
        EventLogic();
    }
    public void ListenEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
        EventLogic();
    }
    public void ListenEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
        EventLogic();
    }
    public void ListenEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) {
        EventLogic();
    }
    public void ListenEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) {
        EventLogic();
    }
    public void ListenEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) {
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



