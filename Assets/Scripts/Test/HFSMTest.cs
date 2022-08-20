using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HFSMTest {
    // A Test behaves as an ordinary method
    [Test]
    public void ZeroOneA() {
        State AState = new AState();
        State BState = new BState();
        StateMachine oneStateMachine = new OneStateMachine(AState, BState);
        oneStateMachine.DefaultStateObject = AState;
        StateMachine zeroStateMachine = new ZeroStateMachine(oneStateMachine);
        zeroStateMachine.DefaultStateObject = oneStateMachine;

        zeroStateMachine.Init();

        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void ZeroOneABChangeBState() {
        State AState = new AState();
        State BState = new BState();
        StateMachine oneStateMachine = new OneStateMachine(AState, BState);
        oneStateMachine.DefaultStateObject = AState;
        StateMachine zeroStateMachine = new ZeroStateMachine(oneStateMachine);
        zeroStateMachine.DefaultStateObject = oneStateMachine;

        zeroStateMachine.Init();

        bool doTransitionOneB = false;
        oneStateMachine.AddTransition(BState, () => { return doTransitionOneB; });

        zeroStateMachine.Init();

        doTransitionOneB = true;
        zeroStateMachine.Update();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.BState", zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void ZeroOneABCChangeCCState() {
        State AState = new AState();
        State BState = new BState();
        State CState = new CState();
        StateMachine oneStateMachine = new OneStateMachine(AState, BState);
        oneStateMachine.DefaultStateObject = AState;
        StateMachine zeroStateMachine = new ZeroStateMachine(oneStateMachine, CState);
        zeroStateMachine.DefaultStateObject = oneStateMachine;

        bool doTransitionOneC = false;
        oneStateMachine.AddTransition(CState, () => { return doTransitionOneC; });

        zeroStateMachine.Init();

        doTransitionOneC = true;
        zeroStateMachine.Update();
        Assert.AreEqual("ZeroStateMachine.CState", zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void ZeroOneAUpdate() {
        State AState = new AState();
        State BState = new BState();
        StateMachine oneStateMachine = new OneStateMachine(AState, BState);
        oneStateMachine.DefaultStateObject = AState;
        StateMachine zeroStateMachine = new ZeroStateMachine(oneStateMachine);
        zeroStateMachine.DefaultStateObject = oneStateMachine;

        bool doTransitionAB = false;
        AState.AddTransition(BState, () => { return doTransitionAB; });

        zeroStateMachine.Init();

        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        doTransitionAB = true;
        zeroStateMachine.Update();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.BState", zeroStateMachine.GetCurrentStateName());

    }

    [Test]
    public void ZeroOneTwoABCDUpdate() {
        State AState = new AState();
        State BState = new BState();
        StateMachine oneStateMachine = new OneStateMachine(AState, BState);
        oneStateMachine.DefaultStateObject = AState;

        State CState = new CState();
        State DState = new DState();
        StateMachine twoStateMachine = new TwoStateMachine(CState, DState);
        twoStateMachine.DefaultStateObject = CState;

        StateMachine zeroStateMachine = new ZeroStateMachine(oneStateMachine, twoStateMachine);
        zeroStateMachine.DefaultStateObject = oneStateMachine;

        bool doTransitionOneC = false;
        oneStateMachine.AddTransition(CState, () => { return doTransitionOneC; });

        zeroStateMachine.Init();

        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        doTransitionOneC = true;
        zeroStateMachine.Update();
        Assert.AreEqual("ZeroStateMachine.TwoStateMachine.CState", zeroStateMachine.GetCurrentStateName());

    }

    [Test]
    public void ZeroOneTwoABUpdate() {
        State AState = new AState();
        StateMachine oneStateMachine = new OneStateMachine(AState);
        oneStateMachine.DefaultStateObject = AState;


        State BState = new BState();
        StateMachine twoStateMachine = new TwoStateMachine(BState);
        twoStateMachine.DefaultStateObject = BState;

        StateMachine zeroStateMachine = new ZeroStateMachine(oneStateMachine, twoStateMachine);
        zeroStateMachine.DefaultStateObject = oneStateMachine;

        bool doTransitionAB = false;
        AState.AddTransition(BState, () => { return doTransitionAB; });

        zeroStateMachine.Init();

        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        doTransitionAB = true;
        zeroStateMachine.Update();
        Assert.AreEqual("ZeroStateMachine.TwoStateMachine.BState", zeroStateMachine.GetCurrentStateName());

    }

    [Test]
    public void ThreeLevelTransition() {
        State AState = new AState();
        State BState = new BState();
        StateMachine twoStateMachine = new TwoStateMachine(AState, BState);
        twoStateMachine.DefaultStateObject = AState;

        State CState = new CState();
        StateMachine oneStateMachine = new OneStateMachine(twoStateMachine, CState);
        oneStateMachine.DefaultStateObject = twoStateMachine;

        StateMachine zeroStateMachine = new ZeroStateMachine(oneStateMachine, CState);
        zeroStateMachine.DefaultStateObject = oneStateMachine;

        bool doTransitionAB = false;
        AState.AddTransition(BState, () => { return doTransitionAB; });

        zeroStateMachine.Init();

        Assert.AreEqual("ZeroStateMachine.OneStateMachine.TwoStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        doTransitionAB = true;
        zeroStateMachine.Update();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.TwoStateMachine.BState", zeroStateMachine.GetCurrentStateName());

    }

    [Test]
    public void RootStateMachineNotInitialized() {
        StringBuilder stringBuilder = new StringBuilder();
        State StateA = new WriterStateA(stringBuilder);
        State StateB = new WriterStateB(stringBuilder);
        StateMachine zeroStateMachine = new WriterZeroStateMachine(stringBuilder, StateA, StateB);



        bool doTransitionAB = false;
        StateA.AddTransition(StateB, () => { return doTransitionAB; });

        Assert.Throws<RootStateMachineNotInitializedException>(zeroStateMachine.Update);
    }

    [Test]
    public void DuplicatedTransition() {
        StringBuilder stringBuilder = new StringBuilder();
        State StateA = new WriterStateA(stringBuilder);
        State StateB = new WriterStateB(stringBuilder);
        StateMachine zeroStateMachine = new WriterZeroStateMachine(stringBuilder, StateA, StateB);

        bool doTransitionAB = false;
        StateA.AddTransition(StateB, () => { return doTransitionAB; });

        Assert.Throws<DuplicatedTransitionException>(
            delegate {
                StateA.AddTransition(StateB,
                    () => {
                        return doTransitionAB;
                    }
                );
            }
        );
    }

    [Test]
    public void LCAStateWithoutStateMachine() {
        StringBuilder stringBuilder = new StringBuilder();
        State StateA = new WriterStateA(stringBuilder);
        State StateB = new WriterStateB(stringBuilder);
        StateMachine zeroStateMachine = new WriterZeroStateMachine(stringBuilder, StateA, StateB);
        zeroStateMachine.DefaultStateObject = StateA;

        State CState = new WriterStateC(stringBuilder);

        bool doTransitionAC = false;
        Assert.Throws<NoCommonParentStateMachineException>(
            delegate {
                StateA.AddTransition(CState,
                    () => {
                        return doTransitionAC;
                    }
                );
            }
        );
    }


    [Test]
    public void LCANoCommonParentStateMachineLevel0() {
        StringBuilder stringBuilder = new StringBuilder();
        State StateA = new AState();
        StateMachine oneStateMachine = new OneStateMachine(StateA);

        State StateB = new BState();
        StateMachine twoStateMachine = new TwoStateMachine(StateB);


        bool doTransitionOneTwo = false;
        Assert.Throws<NoCommonParentStateMachineException>(
            delegate {
                oneStateMachine.AddTransition(twoStateMachine,
                    () => {
                        return doTransitionOneTwo;
                    }
                );
            }
        );
    }

    [Test]
    public void LCANoCommonParentStateMachineLevel1() {
        State StateA = new AState();
        StateMachine oneStateMachine = new OneStateMachine(StateA);

        State StateB = new BState();
        StateMachine twoStateMachine = new TwoStateMachine(StateB);


        bool doTransitionAB = false;
        Assert.Throws<NoCommonParentStateMachineException>(
            delegate {
                StateA.AddTransition(StateB,
                    () => {
                        return doTransitionAB;
                    }
                );
            }
        );
    }

    [Test]
    public void ExecutionOrderEnter() {
        StringBuilder stringBuilder = new StringBuilder();
        
        State stateA = new WriterStateA(stringBuilder);
        StateMachine threeStateMachine = new WriterThreeStateMachine(stringBuilder, stateA);
        StateMachine twoStateMachine = new WriterTwoStateMachine(stringBuilder, threeStateMachine);
        StateMachine oneStateMachine = new WriterOneStateMachine(stringBuilder, twoStateMachine);
        StateMachine zeroStateMachine = new WriterZeroStateMachine(stringBuilder, oneStateMachine);

        zeroStateMachine.Init();
        string expected = "WriterZeroStateMachine Enter" +
                          "WriterOneStateMachine Enter" +
                          "WriterTwoStateMachine Enter" +
                          "WriterThreeStateMachine Enter" +
                          "WriterStateA Enter";  
        Assert.AreEqual(expected, stringBuilder.ToString());
    }

    [Test]
    public void ExecutionOrderExit() {
        StringBuilder stringBuilder = new StringBuilder();

        State stateB = new WriterStateB(stringBuilder);
        StateMachine threeStateMachine = new WriterThreeStateMachine(stringBuilder, stateB);

        State stateA = new WriterStateA(stringBuilder);
        StateMachine twoStateMachine = new WriterTwoStateMachine(stringBuilder, stateA);
        StateMachine oneStateMachine = new WriterOneStateMachine(stringBuilder, twoStateMachine);

        StateMachine zeroStateMachine = new WriterZeroStateMachine(stringBuilder, oneStateMachine, threeStateMachine);

        bool transitionAB = false;
        stateA.AddTransition(stateB, () => { return transitionAB; });

        zeroStateMachine.Init();
        transitionAB = true;
        zeroStateMachine.Update();
        
        string expected = "WriterZeroStateMachine Enter" +
                          "WriterOneStateMachine Enter" +
                          "WriterTwoStateMachine Enter" +
                          "WriterStateA Enter" +
                          "WriterStateA Exit" +
                          "WriterTwoStateMachine Exit" +
                          "WriterOneStateMachine Exit" +
                          "WriterThreeStateMachine Enter" +
                          "WriterStateB Enter";
        Assert.AreEqual(expected, stringBuilder.ToString());
    }

    [Test]
    public void EventWithConditionABChangeBState() {
        State AState = new AState();
        State BState = new BState();
        StateMachine oneStateMachine = new OneStateMachine(AState, BState);
        oneStateMachine.DefaultStateObject = AState;
        StateMachine zeroStateMachine = new ZeroStateMachine(oneStateMachine);
        zeroStateMachine.DefaultStateObject = oneStateMachine;

        Action transitionEvent = null;
        bool conditionAB = false;
        transitionEvent += AState.AddEventTransition(BState, () => { return conditionAB; });

        zeroStateMachine.Init();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        transitionEvent.Invoke();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        conditionAB = true;
        transitionEvent.Invoke();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        zeroStateMachine.Update();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.BState", zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void EventWithoutConditionABChangeBState() {
        State AState = new AState();
        State BState = new BState();
        StateMachine oneStateMachine = new OneStateMachine(AState, BState);
        oneStateMachine.DefaultStateObject = AState;
        StateMachine zeroStateMachine = new ZeroStateMachine(oneStateMachine);
        zeroStateMachine.DefaultStateObject = oneStateMachine;

        Action transitionEvent = null;
        transitionEvent += AState.AddEventTransition(BState);

        zeroStateMachine.Init();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        transitionEvent.Invoke();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        zeroStateMachine.Update();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.BState", zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void NoAvailableTransitionChangeState() {
        State AState = new AState();
        State BState = new BState();
        StateMachine oneStateMachine = new OneStateMachine(AState, BState);
        oneStateMachine.DefaultStateObject = AState;
        StateMachine zeroStateMachine = new ZeroStateMachine(oneStateMachine);
        zeroStateMachine.DefaultStateObject = oneStateMachine;

        AState.AddTransition(BState, ()=> { return false; });

        zeroStateMachine.Init();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        zeroStateMachine.Update();
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.AState", zeroStateMachine.GetCurrentStateName());
    }
}