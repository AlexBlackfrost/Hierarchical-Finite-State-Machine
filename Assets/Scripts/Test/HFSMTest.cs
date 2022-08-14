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
        AState.AddTransition( BState, () => { return doTransitionAB; });

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
        oneStateMachine.AddTransition( CState, () => { return doTransitionOneC; });

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
        AState.AddTransition( BState, () => { return doTransitionAB; });

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
        AState.AddTransition( BState, () => { return doTransitionAB; });

        zeroStateMachine.Init();

        Assert.AreEqual("ZeroStateMachine.OneStateMachine.TwoStateMachine.AState", zeroStateMachine.GetCurrentStateName());
        doTransitionAB = true;
        zeroStateMachine.Update(); 
        Assert.AreEqual("ZeroStateMachine.OneStateMachine.TwoStateMachine.BState", zeroStateMachine.GetCurrentStateName());

    }

    [Test]
    public void LCANoRootStateMachine() {
        StringBuilder stringBuilder = new StringBuilder();
        State StateA = new WriterStateA(stringBuilder);
        State StateB = new WriterStateB(stringBuilder);
        StateMachine zeroStateMachine = new WriterZeroStateMachine(stringBuilder, StateA, StateB);
        zeroStateMachine.DefaultStateObject = StateA;

        State CState = new WriterStateC(stringBuilder);

        bool doTransitionAC = false;
        StateA.AddTransition(CState, () => { return doTransitionAC; });

        zeroStateMachine.Init();
        doTransitionAC = true;
        zeroStateMachine.Update();

        string expected = zeroStateMachine.GetType() + " Enter" +
                          StateA.GetType() + " Enter" +
                          StateA.GetType() + " Exit" +
                          zeroStateMachine.GetType() + " Exit" +
                          StateB.GetType() + " Enter";
        Assert.AreEqual(expected,stringBuilder.ToString());
    }
}