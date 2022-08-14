using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateObjectFactory:MonoBehaviour {
    public class TestState {
        public GameObject go;

        public TestState(GameObject gameObject) {
            go = gameObject;
            Debug.Log("Test state " + gameObject);
        }

    }

    public class TestStateMachine {

        public GameObject go;
        public TestStateMachine(GameObject gameObject) {
            go = gameObject;
            Debug.Log("Test state machine "+gameObject);
        }
    }

    public static T CreateState<T>() {
        GameObject go = new GameObject();
        T state = (T)Activator.CreateInstance(typeof(T), new object[] { go });

        return state;
    }

    private void Awake() {
        TestState testState = CreateState<TestState>();
        TestStateMachine testStateMachine = CreateState<TestStateMachine>();
    }

}

