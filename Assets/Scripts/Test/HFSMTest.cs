using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HFSMTest {
    // A Test behaves as an ordinary method
    internal static string Enter = " Enter";
    internal static string Exit = " Exit";
    internal static string Update = " Update";
    internal static string FixedUpdate = " FixedUpdate";
    internal static string LateUpdate = " LateUpdate";
    internal static string transitionText = "TransitionAction";

    [Test]
    public void ZeroOneA() {
        State stateA = new StateA();
        State stateB = new StateB();
        StateMachine stateMachineOne = new StateMachineOne(stateA, stateB);
        stateMachineOne.DefaultStateObject = stateA;
        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne);
        zeroStateMachine.DefaultStateObject = stateMachineOne;

        zeroStateMachine.Init();

        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(), 
                        
                        zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void ZeroOneABChangeBState() {
        State stateA = new StateA();
        State stateB = new StateB();
        StateMachine stateMachineOne = new StateMachineOne(stateA, stateB);
        stateMachineOne.DefaultStateObject = stateA;
        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne);
        zeroStateMachine.DefaultStateObject = stateMachineOne;

        zeroStateMachine.Init();

        bool doTransitionOneB = false;
        stateMachineOne.AddTransition(stateB, () => { return doTransitionOneB; });

        zeroStateMachine.Init();

        doTransitionOneB = true;
        zeroStateMachine.Update();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateB.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void ZeroOneABCChangeCCState() {
        State stateA = new StateA();
        State stateB = new StateB();
        State stateC = new StateC();
        StateMachine stateMachineOne = new StateMachineOne(stateA, stateB);
        stateMachineOne.DefaultStateObject = stateA;
        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne, stateC);
        zeroStateMachine.DefaultStateObject = stateMachineOne;

        bool doTransitionOneC = false;
        stateMachineOne.AddTransition(stateC, () => { return doTransitionOneC; });

        zeroStateMachine.Init();

        doTransitionOneC = true;
        zeroStateMachine.Update();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateC.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void ZeroOneAUpdate() {
        State stateA = new StateA();
        State stateB = new StateB();
        StateMachine stateMachineOne = new StateMachineOne(stateA, stateB);
        stateMachineOne.DefaultStateObject = stateA;
        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne);
        zeroStateMachine.DefaultStateObject = stateMachineOne;

        bool doTransitionAB = false;
        stateA.AddTransition(stateB, () => { return doTransitionAB; });

        zeroStateMachine.Init();

        Assert.AreEqual(zeroStateMachine.GetType() + "." + 
                        stateMachineOne.GetType() + "." + 
                        stateA.GetType(), 
                        
                        zeroStateMachine.GetCurrentStateName());
        doTransitionAB = true;
        zeroStateMachine.Update();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateB.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void ZeroOneTwoABCDUpdate() {
        State stateA = new StateA();
        State stateB = new StateB();
        StateMachine stateMachineOne = new StateMachineOne(stateA, stateB);
        stateMachineOne.DefaultStateObject = stateA;

        State CState = new StateC();
        State DState = new StateD();
        StateMachine stateMachineTwo = new StateMachineTwo(CState, DState);
        stateMachineTwo.DefaultStateObject = CState;

        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne, stateMachineTwo);
        zeroStateMachine.DefaultStateObject = stateMachineOne;

        bool doTransitionOneC = false;
        stateMachineOne.AddTransition(CState, () => { return doTransitionOneC; });

        zeroStateMachine.Init();

        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
        doTransitionOneC = true;
        zeroStateMachine.Update();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineTwo.GetType() + "." +
                        CState.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());

    }

    [Test]
    public void ZeroOneTwoABUpdate() {
        State stateA = new StateA();
        StateMachine stateMachineOne = new StateMachineOne(stateA);
        stateMachineOne.DefaultStateObject = stateA;


        State stateB = new StateB();
        StateMachine stateMachineTwo = new StateMachineTwo(stateB);
        stateMachineTwo.DefaultStateObject = stateB;

        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne, stateMachineTwo);
        zeroStateMachine.DefaultStateObject = stateMachineOne;

        bool doTransitionAB = false;
        stateA.AddTransition(stateB, () => { return doTransitionAB; });

        zeroStateMachine.Init();

        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
        doTransitionAB = true;
        zeroStateMachine.Update();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineTwo.GetType() + "." +
                        stateB.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());

    }

    [Test]
    public void ThreeLevelTransition() {
        State stateA = new StateA();
        State stateB = new StateB();
        StateMachine stateMachineTwo = new StateMachineTwo(stateA, stateB);
        stateMachineTwo.DefaultStateObject = stateA;

        State CState = new StateC();
        StateMachine stateMachineOne = new StateMachineOne(stateMachineTwo, CState);
        stateMachineOne.DefaultStateObject = stateMachineTwo;

        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne, CState);
        zeroStateMachine.DefaultStateObject = stateMachineOne;

        bool doTransitionAB = false;
        stateA.AddTransition(stateB, () => { return doTransitionAB; });

        zeroStateMachine.Init();

        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateMachineTwo.GetType() + "." +
                        stateA.GetType(), 
                        
                        zeroStateMachine.GetCurrentStateName());
        doTransitionAB = true;
        zeroStateMachine.Update();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateMachineTwo.GetType() + "." +
                        stateB.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());

    }

    [Test]
    public void ThreeLevelTransitionExecutionOrder() {
        StringBuilder sb = new StringBuilder();
        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb,stateA, stateB);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool doTransitionAB = false;
        stateA.AddTransition(stateB, () => { return doTransitionAB; });

        stateMachineZero.Init();
        doTransitionAB = true;
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                          stateMachineOne.GetType() + Enter +
                          stateA.GetType() + Enter +

                          stateA.GetType() + Exit +
                          stateB.GetType() + Enter +

                          stateMachineZero.GetType() + Update +
                          stateMachineOne.GetType() + Update +
                          stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void RootStateMachineNotInitialized() {
        StringBuilder stringBuilder = new StringBuilder();
        State StateA = new WriterStateA(stringBuilder);
        State StateB = new WriterStateB(stringBuilder);
        StateMachine zeroStateMachine = new WriterStateMachineZero(stringBuilder, StateA, StateB);



        bool doTransitionAB = false;
        StateA.AddTransition(StateB, () => { return doTransitionAB; });

        Assert.Throws<RootStateMachineNotInitializedException>(zeroStateMachine.Update);
    }

    [Test]
    public void LCAStateWithoutStateMachine() {
        StringBuilder stringBuilder = new StringBuilder();
        State StateA = new WriterStateA(stringBuilder);
        State StateB = new WriterStateB(stringBuilder);
        StateMachine zeroStateMachine = new WriterStateMachineZero(stringBuilder, StateA, StateB);
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
        State StateA = new StateA();
        StateMachine stateMachineOne = new StateMachineOne(StateA);

        State StateB = new StateB();
        StateMachine stateMachineTwo = new StateMachineTwo(StateB);


        bool doTransitionOneTwo = false;
        Assert.Throws<NoCommonParentStateMachineException>(
            delegate {
                stateMachineOne.AddTransition(stateMachineTwo,
                    () => {
                        return doTransitionOneTwo;
                    }
                );
            }
        );
    }

    [Test]
    public void LCANoCommonParentStateMachineLevel1() {
        State StateA = new StateA();
        StateMachine stateMachineOne = new StateMachineOne(StateA);

        State StateB = new StateB();
        StateMachine stateMachineTwo = new StateMachineTwo(StateB);


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
        StateMachine stateMachineThree = new WriterStateMachineThree(stringBuilder, stateA);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(stringBuilder, stateMachineThree);
        StateMachine stateMachineOne = new WriterStateMachineOne(stringBuilder, stateMachineTwo);
        StateMachine zeroStateMachine = new WriterStateMachineZero(stringBuilder, stateMachineOne);

        zeroStateMachine.Init();
        string expected = zeroStateMachine.GetType() + Enter +
                          stateMachineOne.GetType() + Enter +
                          stateMachineTwo.GetType() + Enter +
                          stateMachineThree.GetType() + Enter +
                          stateA.GetType() + Enter;
        Assert.AreEqual(expected, stringBuilder.ToString());
    }

    [Test]
    public void ExecutionOrderExit() {
        StringBuilder stringBuilder = new StringBuilder();

        State stateB = new WriterStateB(stringBuilder);
        StateMachine stateMachineThree = new WriterStateMachineThree(stringBuilder, stateB);

        State stateA = new WriterStateA(stringBuilder);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(stringBuilder, stateA);
        StateMachine stateMachineOne = new WriterStateMachineOne(stringBuilder, stateMachineTwo);

        StateMachine zeroStateMachine = new WriterStateMachineZero(stringBuilder, stateMachineOne, stateMachineThree);

        bool transitionAB = false;
        stateA.AddTransition(stateB, () => { return transitionAB; });

        zeroStateMachine.Init();
        transitionAB = true;
        zeroStateMachine.Update();

        string expected = zeroStateMachine.GetType() + Enter +
                          stateMachineOne.GetType() + Enter +
                          stateMachineTwo.GetType() + Enter +
                          stateA.GetType() + Enter +
                          stateA.GetType() + Exit +
                          stateMachineTwo.GetType() + Exit +
                          stateMachineOne.GetType() + Exit +
                          stateMachineThree.GetType() + Enter +
                          stateB.GetType() + Enter;
        Assert.AreEqual(expected, stringBuilder.ToString());
    }

    [Test]
    public void EventWithConditionABChangeBState() {
        State stateA = new StateA();
        State stateB = new StateB();
        StateMachine stateMachineOne = new StateMachineOne(stateA, stateB);
        stateMachineOne.DefaultStateObject = stateA;
        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne);
        zeroStateMachine.DefaultStateObject = stateMachineOne;

        Action transitionEvent = null;
        bool conditionAB = false;
        transitionEvent += stateA.AddEventTransition(stateB, () => { return conditionAB; });

        zeroStateMachine.Init();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
        transitionEvent.Invoke();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
        conditionAB = true;
        transitionEvent.Invoke();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
        zeroStateMachine.Update();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateB.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void EventWithoutConditionABChangeBState() {
        State stateA = new StateA();
        State stateB = new StateB();
        StateMachine stateMachineOne = new StateMachineOne(stateA, stateB);
        stateMachineOne.DefaultStateObject = stateA;
        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne);
        zeroStateMachine.DefaultStateObject = stateMachineOne;

        Action transitionEvent = null;
        transitionEvent += stateA.AddEventTransition(stateB);

        zeroStateMachine.Init();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
        transitionEvent.Invoke();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
        zeroStateMachine.Update();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateB.GetType(), zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void NoAvailableTransitionChangeState() {
        State stateA = new StateA();
        State stateB = new StateB();
        StateMachine stateMachineOne = new StateMachineOne(stateA, stateB);
        stateMachineOne.DefaultStateObject = stateA;
        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne);
        zeroStateMachine.DefaultStateObject = stateMachineOne;

        stateA.AddTransition(stateB, () => { return false; });

        zeroStateMachine.Init();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
        zeroStateMachine.Update();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void NoAvailableEventTransitionForStateMachine() {
        State stateA = new StateA();
        StateMachine stateMachineOne = new StateMachineOne(stateA);

        State stateB = new StateB();
        StateMachine stateMachineTwo = new StateMachineTwo(stateB);

        StateMachine zeroStateMachine = new StateMachineZero(stateMachineOne, stateMachineTwo);

        stateMachineOne.AddEventTransition(stateB);

        zeroStateMachine.Init();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
        zeroStateMachine.Update();
        Assert.AreEqual(zeroStateMachine.GetType() + "." +
                        stateMachineOne.GetType() + "." +
                        stateA.GetType(),
                        
                        zeroStateMachine.GetCurrentStateName());
    }

    [Test]
    public void FixedUpdateCallOrder() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine zeroStateMachine = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        Action transitionEvent = null;
        transitionEvent += stateMachineOne.AddEventTransition(stateB);

        zeroStateMachine.Init();

        zeroStateMachine.FixedUpdate();
        zeroStateMachine.Update();

        transitionEvent.Invoke();

        zeroStateMachine.FixedUpdate();
        zeroStateMachine.Update();

        zeroStateMachine.FixedUpdate();
        zeroStateMachine.Update();

        string expected = zeroStateMachine.GetType() + Enter +
                          stateMachineOne.GetType() + Enter +
                          stateA.GetType() + Enter +
                          zeroStateMachine.GetType() + FixedUpdate +
                          stateMachineOne.GetType() + FixedUpdate +
                          stateA.GetType() + FixedUpdate +
                          zeroStateMachine.GetType() + Update +
                          stateMachineOne.GetType() + Update +
                          stateA.GetType() + Update +

                          zeroStateMachine.GetType() + FixedUpdate +
                          stateMachineOne.GetType() + FixedUpdate +
                          stateA.GetType() + FixedUpdate +
                          stateA.GetType() + Exit +
                          stateMachineOne.GetType() + Exit +
                          stateMachineTwo.GetType() + Enter +
                          stateB.GetType() + Enter +

                          zeroStateMachine.GetType() + FixedUpdate +
                          stateMachineTwo.GetType() + FixedUpdate +
                          stateB.GetType() + FixedUpdate +
                          zeroStateMachine.GetType() + Update +
                          stateMachineTwo.GetType() + Update +
                          stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void LateUpdateCallOrder() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine zeroStateMachine = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        Action transitionEvent = null;
        transitionEvent += stateMachineOne.AddEventTransition(stateB);

        zeroStateMachine.Init();

        zeroStateMachine.Update();
        zeroStateMachine.LateUpdate();

        transitionEvent.Invoke();

        zeroStateMachine.Update();
        zeroStateMachine.LateUpdate();

        zeroStateMachine.Update();
        zeroStateMachine.LateUpdate();

        string expected = zeroStateMachine.GetType() + Enter +
                          stateMachineOne.GetType() + Enter +
                          stateA.GetType() + Enter +
                          zeroStateMachine.GetType() + Update +
                          stateMachineOne.GetType() + Update +
                          stateA.GetType() + Update +
                          zeroStateMachine.GetType() + LateUpdate +
                          stateMachineOne.GetType() + LateUpdate +
                          stateA.GetType() + LateUpdate +

                          stateA.GetType() + Exit +
                          stateMachineOne.GetType() + Exit +
                          stateMachineTwo.GetType() + Enter +
                          stateB.GetType() + Enter +
                          zeroStateMachine.GetType() + LateUpdate +
                          stateMachineTwo.GetType() + LateUpdate +
                          stateB.GetType() + LateUpdate +

                          zeroStateMachine.GetType() + Update +
                          stateMachineTwo.GetType() + Update +
                          stateB.GetType() + Update +
                          zeroStateMachine.GetType() + LateUpdate +
                          stateMachineTwo.GetType() + LateUpdate +
                          stateB.GetType() + LateUpdate;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void StateMachineWithoutStates() {
        Assert.Throws<StatelessStateMachineException>(() => new StateMachineZero());
    }

    [Test]
    public void SingleState() {
        StringBuilder sb = new StringBuilder();
        State stateA = new WriterStateA(sb);
        stateA.Update();
        Assert.AreEqual(stateA.GetType() + Update, sb.ToString());
    }

    [Test]
    public void TransitionAction() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine zeroStateMachine = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        
        Action transitionAction = () => { sb.Append(transitionText); };
        stateA.AddTransition(stateB, transitionAction);

        zeroStateMachine.Init();
        zeroStateMachine.Update();
        zeroStateMachine.Update();

        string expected = zeroStateMachine.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +
                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         transitionText +
                         stateMachineTwo.GetType() +  Enter +
                         stateB.GetType() + Enter +
                         zeroStateMachine.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionEventAction() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);


        Action transitionAction = () => { sb.Append(transitionText); };
        Action transitionEvent = null;
        transitionEvent += stateA.AddEventTransition(stateB, transitionAction);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke();
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +
                         stateMachineZero.GetType() + Update + 
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +


                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         transitionText +
                         stateMachineTwo.GetType() + Enter +
                         stateB.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }
    [Test]
    public void TransitionEventWithGenericType() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);


        Action<int> transitionEvent = null;
        transitionEvent += stateA.AddEventTransition<int>(stateB);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke(163453);
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +


                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         stateMachineTwo.GetType() + Enter +
                         stateB.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionEventActionWithGenericType() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);


        Action transitionAction = () => { sb.Append(transitionText); };
        Action<int> transitionEvent = null;
        transitionEvent += stateA.AddEventTransition<int>(stateB, transitionAction);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke(163453);
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +


                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         transitionText +
                         stateMachineTwo.GetType() + Enter +
                         stateB.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionEventWithTwoGenericTypes() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        Action<int, string> transitionEvent = null;
        transitionEvent += stateA.AddEventTransition<int, string>(stateB);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke(163453, "yes");
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +


                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         stateMachineTwo.GetType() + Enter +
                         stateB.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionEventActionWithTwoGenericTypes() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);


        Action transitionAction = () => { sb.Append(transitionText); };
        Action<int, string> transitionEvent = null;
        transitionEvent += stateA.AddEventTransition<int, string>(stateB, transitionAction);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke(163453, "yes");
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +


                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         transitionText +
                         stateMachineTwo.GetType() + Enter +
                         stateB.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionEventWithThreeGenericTypes() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        Action<int, string, int> transitionEvent = null;
        transitionEvent += stateA.AddEventTransition<int, string, int>(stateB);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke(163453, "yes", 9);
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +


                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         stateMachineTwo.GetType() + Enter +
                         stateB.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionEventActionWithThreeGenericTypes() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);


        Action transitionAction = () => { sb.Append(transitionText); };
        Action<int, string, int> transitionEvent = null;
        transitionEvent += stateA.AddEventTransition<int, string, int>(stateB, transitionAction);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke(163453, "yes", 9);
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +


                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         transitionText +
                         stateMachineTwo.GetType() + Enter +
                         stateB.GetType() + Enter +
                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void SelfTransitionStateMachine() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionCondition = false;
        Action transitionAction = () => { sb.Append(transitionText); };
        stateMachineOne.AddTransition(stateMachineOne, transitionAction, ()=> { return transitionCondition; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        transitionCondition = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         transitionText +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString(), expected + "\n" + sb.ToString());
    }

    [Test]
    public void TransitionFromStateMachineToItsCurrentState() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionCondition = false;
        Action transitionAction = () => { sb.Append(transitionText); };
        stateMachineOne.AddTransition(stateA, transitionAction, () => { return transitionCondition; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        transitionCondition = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         transitionText +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString(), expected + "\n" + sb.ToString());
    }

    [Test]
    public void SelfTransitionState() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionCondition = false;
        Action transitionAction = () => { sb.Append(transitionText); };
        stateA.AddTransition(stateA, transitionAction, () => { return transitionCondition; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        transitionCondition = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         transitionText +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionFromStateToItsParentStateMachine() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionCondition = false;
        Action transitionAction = () => { sb.Append(transitionText); };
        stateA.AddTransition(stateMachineOne, transitionAction, () => { return transitionCondition; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        transitionCondition = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         transitionText +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionFromStateToItsGrandParentStateMachine() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateA);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateMachineTwo);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionCondition = false;
        Action transitionAction = () => { sb.Append(transitionText); };
        stateA.AddTransition(stateMachineOne, transitionAction, () => { return transitionCondition; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        transitionCondition = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateMachineTwo.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateMachineTwo.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         transitionText +
                         stateMachineOne.GetType() + Enter +
                         stateMachineTwo.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateA.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionFromStateToItsGrandParentStateMachinev2() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateA);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateB, stateMachineTwo);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionConditionAOne = false;
        stateA.AddTransition(stateMachineOne, () => { return transitionConditionAOne; });

        bool transitionConditionBA = false;
        stateB.AddTransition(stateA, () => { return transitionConditionBA; });

        stateMachineZero.Init();
        transitionConditionBA = true;
        stateMachineZero.Update();
        stateMachineZero.Update();
        transitionConditionAOne = true;
        transitionConditionBA = false;
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateB.GetType() + Enter +

                         stateB.GetType() + Exit +
                         stateMachineTwo.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateMachineTwo.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         stateMachineOne.GetType() + Enter +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionFromStateMachineToGrandChildState() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateA);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateB, stateMachineTwo);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionConditionOneA = false;
        stateMachineOne.AddTransition(stateA, () => { return transitionConditionOneA; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionOneA = true;
        stateMachineZero.Update();
        transitionConditionOneA = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateB.GetType() + Update +

                         stateB.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         stateMachineOne.GetType() + Enter +
                         stateMachineTwo.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateA.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyTransition() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        bool transitionCondition = false;
        Action transitionAction = () => { sb.Append(transitionText); };
        stateMachineOne.AddAnyTransition(stateB, transitionAction, () => { return transitionCondition; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         transitionText +
                         stateMachineTwo.GetType() + Enter +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyTransitionPriority() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);
        State stateD = new WriterStateD(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateC, stateD);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        bool transitionCondition = false;
        stateMachineOne.AddAnyTransition(stateC, () => { return transitionCondition; });

        bool transitionConditionBD = false;
        stateB.AddTransition(stateD, () => { return transitionConditionBD; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         stateMachineTwo.GetType() + Enter +
                         stateC.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateC.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyTransitionPriorityThreeLevels() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);
        State stateD = new WriterStateD(sb);

        StateMachine stateMachineThree = new WriterStateMachineTwo(sb, stateC, stateD);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateMachineThree);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        bool transitionConditionOneTwo = false;
        stateMachineOne.AddTransition(stateMachineTwo, () => { return transitionConditionOneTwo; });

        bool transitionConditionCD = false;
        stateMachineTwo.AddAnyTransition(stateD, () => { return transitionConditionCD; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionCD = true;
        transitionConditionOneTwo = true;
        stateMachineZero.Update();
        transitionConditionCD = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         stateMachineTwo.GetType() + Enter +
                         stateMachineThree.GetType() + Enter +
                         stateC.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateMachineThree.GetType() + Update +
                         stateC.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void EventTransitionConsumed() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);
        State stateD = new WriterStateD(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateC, stateD);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        bool transitionConditionOneC = false;
        stateMachineOne.AddAnyTransition(stateC, () => { return transitionConditionOneC; });

        bool transitionConditionCD = false;
        Action transitionEvent = null;
        transitionEvent += stateC.AddEventTransition(stateD, () => { return transitionConditionCD; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionOneC = true;
        transitionConditionCD = true;
        transitionEvent.Invoke();
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         stateMachineTwo.GetType() + Enter +
                         stateC.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateC.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void NoCommonParentStateMachineAnyTransition() {
        StringBuilder stringBuilder = new StringBuilder();
        State StateA = new WriterStateA(stringBuilder);
        State StateB = new WriterStateB(stringBuilder);
        StateMachine zeroStateMachine = new WriterStateMachineZero(stringBuilder, StateA, StateB);
        zeroStateMachine.DefaultStateObject = StateA;

        State CState = new WriterStateC(stringBuilder);

        bool doTransitionAC = false;
        Assert.Throws<NoCommonParentStateMachineException>(
            delegate {
                zeroStateMachine.AddAnyTransition(CState,
                    () => {
                        return doTransitionAC;
                    }
                );
            }
        );
    }

    [Test]
    public void AnyEventTransitionZeroParam() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionConditionAB = false;
        Action transitionEvent = null;
        transitionEvent += stateMachineOne.AddAnyEventTransition(stateB, () => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke();
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyEventTransitionZeroParamWithTransitionAction() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionConditionAB = false;
        Action transitionEvent = null;
        Action transitionAction = () => { sb.Append(transitionText); };
        transitionEvent += stateMachineOne.AddAnyEventTransition(stateB, transitionAction, () => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke();
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         transitionText +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }


    [Test]
    public void AnyEventTransitionOneParam() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionConditionAB = false;
        Action<int> transitionEvent = null;
        transitionEvent += stateMachineOne.AddAnyEventTransition<int>(stateB, () => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1);
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyEventTransitionOneParamWithTransitionAction() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionConditionAB = false;
        Action<int> transitionEvent = null;
        Action transitionAction = () => { sb.Append(transitionText); };
        transitionEvent += stateMachineOne.AddAnyEventTransition<int>(stateB, transitionAction, () => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1);
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         transitionText +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyEventTransitionTwoParams() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionConditionAB = false;
        Action<int, string> transitionEvent = null;
        transitionEvent += stateMachineOne.AddAnyEventTransition<int, string>(stateB, () => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1, "yay");
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyEventTransitionTwoParamsWithTransitionAction() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionConditionAB = false;
        Action<int, string> transitionEvent = null;
        Action transitionAction = () => { sb.Append(transitionText); };
        transitionEvent += stateMachineOne.AddAnyEventTransition<int, string>(stateB, transitionAction, () => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1, "yay");
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         transitionText +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyEventTransitionThreeParams() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionConditionAB = false;
        Action<int, string, bool> transitionEvent = null;
        transitionEvent += stateMachineOne.AddAnyEventTransition<int, string, bool>(stateB, () => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1, "yay", false);
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyEventTransitionThreeParamsWithTransitionAction() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionConditionAB = false;
        Action<int, string, bool> transitionEvent = null;
        Action transitionAction = () => { sb.Append(transitionText); };
        transitionEvent += stateMachineOne.AddAnyEventTransition<int, string, bool>(stateB, transitionAction, () => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1, "yay", false);
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         transitionText +
                         stateB.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateB.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyEventTransitionRootStateMachine() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineTwo = new WriterStateMachineOne(sb, stateC);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        bool transitionConditionC = false;
        Action transitionEvent = null;
        transitionEvent += stateMachineZero.AddAnyEventTransition(stateC, () => { return transitionConditionC; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionC = true;
        transitionEvent.Invoke();
        stateMachineZero.Update();
        transitionConditionC = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + Enter +
                         stateMachineOne.GetType() + Enter +
                         stateA.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineOne.GetType() + Update +
                         stateA.GetType() + Update +

                         stateA.GetType() + Exit +
                         stateMachineOne.GetType() + Exit +
                         stateMachineTwo.GetType() + Enter +
                         stateC.GetType() + Enter +

                         stateMachineZero.GetType() + Update +
                         stateMachineTwo.GetType() + Update +
                         stateC.GetType() + Update;

        Assert.AreEqual(expected, sb.ToString());
    }
}