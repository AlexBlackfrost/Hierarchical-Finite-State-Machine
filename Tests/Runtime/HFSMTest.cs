using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HFSM;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HFSMTest {
    // A Test behaves as an ordinary method
    internal static string EnterLogText = " Enter. ";
    internal static string ExitLogText = " Exit. ";
    internal static string UpdateLogText = " Update. ";
    internal static string FixedUpdateLogText = " FixedUpdate. ";
    internal static string LateUpdateLogText = " LateUpdate. ";
    internal static string TransitionLogText = "TransitionAction";

    [Test]
    public void StateName() {
        State stateA = new StateA();
        State stateB = new StateB();
        StateMachine stateMachineOne = new StateMachineOne(stateA);
        StateMachine stateMachineTwo = new StateMachineTwo(stateB);
        StateMachine stateMachineZero = new StateMachineZero(stateMachineOne, stateMachineTwo);

        stateMachineZero.Init();
        Assert.AreEqual("StateMachineZero.StateMachineOne.StateA", stateMachineZero.GetCurrentStateName());
    }

    [Test]
    public void StateNameNone() {
        State stateA = new StateA();
        State stateB = new StateB();
        StateMachine stateMachineOne = new StateMachineOne(stateA);
        StateMachine stateMachineTwo = new StateMachineTwo(stateB);
        StateMachine stateMachineZero = new StateMachineZero(stateMachineOne, stateMachineTwo);

        stateMachineZero.Init();
        Assert.AreEqual("StateMachineTwo.None", stateMachineTwo.GetCurrentStateName());
    }

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

        string expected = stateMachineZero.GetType() + EnterLogText +
                          stateMachineOne.GetType() + EnterLogText +
                          stateA.GetType() + EnterLogText +

                          stateA.GetType() + ExitLogText +
                          stateB.GetType() + EnterLogText +

                          stateMachineZero.GetType() + UpdateLogText +
                          stateMachineOne.GetType() + UpdateLogText +
                          stateB.GetType() + UpdateLogText;

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
        string expected = zeroStateMachine.GetType() + EnterLogText +
                          stateMachineOne.GetType() + EnterLogText +
                          stateMachineTwo.GetType() + EnterLogText +
                          stateMachineThree.GetType() + EnterLogText +
                          stateA.GetType() + EnterLogText;
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

        string expected = zeroStateMachine.GetType() + EnterLogText +
                          stateMachineOne.GetType() + EnterLogText +
                          stateMachineTwo.GetType() + EnterLogText +
                          stateA.GetType() + EnterLogText +
                          stateA.GetType() + ExitLogText +
                          stateMachineTwo.GetType() + ExitLogText +
                          stateMachineOne.GetType() + ExitLogText +
                          stateMachineThree.GetType() + EnterLogText +
                          stateB.GetType() + EnterLogText;
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

        string expected = zeroStateMachine.GetType() + EnterLogText +
                          stateMachineOne.GetType() + EnterLogText +
                          stateA.GetType() + EnterLogText +
                          zeroStateMachine.GetType() + FixedUpdateLogText +
                          stateMachineOne.GetType() + FixedUpdateLogText +
                          stateA.GetType() + FixedUpdateLogText +
                          zeroStateMachine.GetType() + UpdateLogText +
                          stateMachineOne.GetType() + UpdateLogText +
                          stateA.GetType() + UpdateLogText +

                          zeroStateMachine.GetType() + FixedUpdateLogText +
                          stateMachineOne.GetType() + FixedUpdateLogText +
                          stateA.GetType() + FixedUpdateLogText +
                          stateA.GetType() + ExitLogText +
                          stateMachineOne.GetType() + ExitLogText +
                          stateMachineTwo.GetType() + EnterLogText +
                          stateB.GetType() + EnterLogText +

                          zeroStateMachine.GetType() + FixedUpdateLogText +
                          stateMachineTwo.GetType() + FixedUpdateLogText +
                          stateB.GetType() + FixedUpdateLogText +
                          zeroStateMachine.GetType() + UpdateLogText +
                          stateMachineTwo.GetType() + UpdateLogText +
                          stateB.GetType() + UpdateLogText;

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

        string expected = zeroStateMachine.GetType() + EnterLogText +
                          stateMachineOne.GetType() + EnterLogText +
                          stateA.GetType() + EnterLogText +
                          zeroStateMachine.GetType() + UpdateLogText +
                          stateMachineOne.GetType() + UpdateLogText +
                          stateA.GetType() + UpdateLogText +
                          zeroStateMachine.GetType() + LateUpdateLogText +
                          stateMachineOne.GetType() + LateUpdateLogText +
                          stateA.GetType() + LateUpdateLogText +

                          stateA.GetType() + ExitLogText +
                          stateMachineOne.GetType() + ExitLogText +
                          stateMachineTwo.GetType() + EnterLogText +
                          stateB.GetType() + EnterLogText +
                          zeroStateMachine.GetType() + LateUpdateLogText +
                          stateMachineTwo.GetType() + LateUpdateLogText +
                          stateB.GetType() + LateUpdateLogText +

                          zeroStateMachine.GetType() + UpdateLogText +
                          stateMachineTwo.GetType() + UpdateLogText +
                          stateB.GetType() + UpdateLogText +
                          zeroStateMachine.GetType() + LateUpdateLogText +
                          stateMachineTwo.GetType() + LateUpdateLogText +
                          stateB.GetType() + LateUpdateLogText;

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
        Assert.AreEqual(stateA.GetType() + UpdateLogText, sb.ToString());
    }

    [Test]
    public void TransitionAction() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        State stateB = new WriterStateB(sb);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateB);

        StateMachine zeroStateMachine = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);

        
        Action transitionAction = () => { sb.Append(TransitionLogText); };
        stateA.AddTransition(stateB, transitionAction);

        zeroStateMachine.Init();
        zeroStateMachine.Update();
        zeroStateMachine.Update();

        string expected = zeroStateMachine.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +
                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         TransitionLogText +
                         stateMachineTwo.GetType() +  EnterLogText +
                         stateB.GetType() + EnterLogText +
                         zeroStateMachine.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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


        Action transitionAction = () => { sb.Append(TransitionLogText); };
        Action transitionEvent = null;
        transitionEvent += stateA.AddEventTransition(stateB, transitionAction);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke();
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText + 
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +


                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         TransitionLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +


                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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


        Action<int> transitionAction = (int arg) => { sb.Append(TransitionLogText); };
        Action<int> transitionEvent = null;
        transitionEvent += stateA.AddEventTransition<int>(stateB, transitionAction);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke(163453);
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +


                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         TransitionLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +


                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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


        Action<int, string> transitionAction = (int arg1, string arg2) => { sb.Append(TransitionLogText); };
        Action<int, string> transitionEvent = null;
        transitionEvent += stateA.AddEventTransition<int, string>(stateB, transitionAction);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke(163453, "yes");
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +


                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         TransitionLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +


                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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


        Action<int, string, int> transitionAction = (int arg1, string arg2, int arg3) => { sb.Append(TransitionLogText); };
        Action<int, string, int> transitionEvent = null;
        transitionEvent += stateA.AddEventTransition<int, string, int>(stateB, transitionAction);

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionEvent.Invoke(163453, "yes", 9);
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +


                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         TransitionLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +
                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void SelfTransitionStateMachine() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionCondition = false;
        Action transitionAction = () => { sb.Append(TransitionLogText); };
        stateMachineOne.AddTransition(stateMachineOne, transitionAction, ()=> { return transitionCondition; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        transitionCondition = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         TransitionLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText;

        Assert.AreEqual(expected, sb.ToString(), expected + "\n" + sb.ToString());
    }

    [Test]
    public void TransitionFromStateMachineToItsCurrentState() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionCondition = false;
        Action transitionAction = () => { sb.Append(TransitionLogText); };
        stateMachineOne.AddTransition(stateA, transitionAction, () => { return transitionCondition; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        transitionCondition = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         TransitionLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText;

        Assert.AreEqual(expected, sb.ToString(), expected + "\n" + sb.ToString());
    }

    [Test]
    public void SelfTransitionState() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionCondition = false;
        Action transitionAction = () => { sb.Append(TransitionLogText); };
        stateA.AddTransition(stateA, transitionAction, () => { return transitionCondition; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        transitionCondition = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         TransitionLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionFromStateToItsParentStateMachine() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA);

        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne);

        bool transitionCondition = false;
        Action transitionAction = () => { sb.Append(TransitionLogText); };
        stateA.AddTransition(stateMachineOne, transitionAction, () => { return transitionCondition; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        transitionCondition = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         TransitionLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText;

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
        Action transitionAction = () => { sb.Append(TransitionLogText); };
        stateA.AddTransition(stateMachineOne, transitionAction, () => { return transitionCondition; });

        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        transitionCondition = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         TransitionLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText;

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

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +

                         stateB.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText +

                         stateB.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText;

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
        Action transitionAction = () => { sb.Append(TransitionLogText); };
        stateMachineOne.AddAnyTransition(stateB, transitionAction, () => { return transitionCondition; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionCondition = true;
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         TransitionLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateC.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateC.GetType() + UpdateLogText;

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

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateMachineThree.GetType() + EnterLogText +
                         stateC.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateMachineThree.GetType() + UpdateLogText +
                         stateC.GetType() + UpdateLogText;

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

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateC.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateC.GetType() + UpdateLogText;

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

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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
        Action transitionAction = () => { sb.Append(TransitionLogText); };
        transitionEvent += stateMachineOne.AddAnyEventTransition(stateB, transitionAction, () => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke();
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         TransitionLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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
        transitionEvent += stateMachineOne.AddAnyEventTransition<int>(stateB, (int arg1) => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1);
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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
        Action<int> transitionAction = (int arg) => { sb.Append(TransitionLogText); };
        transitionEvent += stateMachineOne.AddAnyEventTransition<int>(stateB, transitionAction, (int arg1) => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1);
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         TransitionLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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
        transitionEvent += stateMachineOne.AddAnyEventTransition<int, string>(stateB, (int arg1, string arg2) => { return transitionConditionAB; });


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1, "yay");
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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
        Action<int, string> transitionAction = (int arg1, string arg2) => { sb.Append(TransitionLogText); };
        Func<int, string, bool> transitionCondition = (int arg1, string arg2) => { return transitionConditionAB; };
        transitionEvent += stateMachineOne.AddAnyEventTransition<int, string>(stateB, transitionAction, transitionCondition);


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1, "yay");
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         TransitionLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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
        Func<int, string, bool, bool> transitionCondition = (int arg1, string arg2, bool arg3) => { return transitionConditionAB; };
        transitionEvent += stateMachineOne.AddAnyEventTransition<int, string, bool>(stateB, transitionCondition);


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1, "yay", false);
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

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
        Action<int, string, bool> transitionAction = (int arg1, string arg2, bool arg3) => { sb.Append(TransitionLogText); };
        Func<int, string, bool, bool> transitionCondition = (int arg1, string arg2, bool arg3) => { return transitionConditionAB; };
        transitionEvent += stateMachineOne.AddAnyEventTransition<int, string, bool>(stateB, transitionAction, transitionCondition );


        stateMachineZero.Init();
        stateMachineZero.Update();
        transitionConditionAB = true;
        transitionEvent.Invoke(1, "yay", false);
        stateMachineZero.Update();
        transitionConditionAB = false;
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         TransitionLogText +
                         stateB.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateB.GetType() + UpdateLogText;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void AnyEventTransitionRootStateMachine() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateC);
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

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateC.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateC.GetType() + UpdateLogText;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void TransitionEventActionWithEventParam() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateC);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);


        Action<int[]> transitionEvent = null;
        int[] number = { 48 };
        Action<int[]> transitionAction = (int[] number) => { 
            number[0]++; 
        };
        Func<int[], bool> transitionCondition = (int[] numberArg) => { return numberArg[0] == 49; };
        transitionEvent += stateA.AddEventTransition<int[]>(stateC, transitionAction, transitionCondition);


        stateMachineZero.Init();
        stateMachineZero.Update();
        number[0] = 49;
        transitionEvent.Invoke(number);
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateC.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateC.GetType() + UpdateLogText;

        Assert.AreEqual(expected, sb.ToString());
        Assert.AreEqual(50, number[0]); 
    }

    [Test]
    public void MultipleEventsFiredBeforeProcessing() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateC);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);


        Action<int[]> transitionEvent = null;
        int[] number = { 48 };
        Action<int[]> transitionAction = (int[] number) => {
            number[0]++;
        };
        Func<int[], bool> transitionConditionAC = (int[] numberArg) => { return numberArg[0] == 49; };
        Func<int[], bool> transitionConditionCA = (int[] numberArg) => { return numberArg[0] == 50; };
        transitionEvent += stateA.AddEventTransition<int[]>(stateC, transitionAction, transitionConditionAC);
        transitionEvent += stateC.AddEventTransition<int[]>(stateA, transitionAction, transitionConditionCA);


        stateMachineZero.Init();
        stateMachineZero.Update();
        number[0] = 49;
        transitionEvent.Invoke(number);
        transitionEvent.Invoke(number);
        transitionEvent.Invoke(number);
        stateMachineZero.Update();
        number[0] = 50;
        transitionEvent.Invoke(number);
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateC.GetType() + EnterLogText +

                         stateC.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + ExitLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText;

        Assert.AreEqual(expected, sb.ToString());
    }

    [Test]
    public void MultipleEventsWithDifferentArgsFiredBeforeProcessing() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateC);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);


        int[] number = { 48 };
        Action<int[]> transitionEvent = null;
        Action<int[]> transitionAction = (int[] number) => {
            number[0]++;
        };
        Func<int[], bool> transitionConditionAC = (int[] numberArg) => { return numberArg[0] == 49; };
        Func<int[], bool> transitionConditionCA = (int[] numberArg) => { return numberArg[0] == 50; };
        transitionEvent += stateA.AddEventTransition<int[]>(stateC, transitionAction, transitionConditionAC);
        transitionEvent += stateC.AddEventTransition<int[]>(stateA, transitionAction, transitionConditionCA);


        stateMachineZero.Init();
        stateMachineZero.Update();
        number[0] = 47;
        transitionEvent.Invoke(number);
        number[0] = 46;
        transitionEvent.Invoke(number);
        number[0] = 49;
        transitionEvent.Invoke(number);
        stateMachineZero.Update();
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateC.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateC.GetType() + UpdateLogText;

        Assert.AreEqual(expected, sb.ToString()); 
    }

    [Test]
    public void ProcessIntantlyTransitionEvent() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);

        StateMachine stateMachineOne = new WriterStateMachineOne(sb, stateA, stateB);
        StateMachine stateMachineTwo = new WriterStateMachineTwo(sb, stateC);
        StateMachine stateMachineZero = new WriterStateMachineZero(sb, stateMachineOne, stateMachineTwo);


        int[] number = { 48 };
        Action<int[]> transitionEvent = null;
        Func<int[], bool> transitionConditionAC = (int[] numberArg) => { return numberArg[0] == 49; };
        transitionEvent += stateA.AddEventTransition<int[]>(stateC, true, transitionConditionAC);


        stateMachineZero.Init();
        stateMachineZero.Update();
        number[0] = 49;
        transitionEvent.Invoke(number);
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateMachineOne.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineOne.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateMachineOne.GetType() + ExitLogText +
                         stateMachineTwo.GetType() + EnterLogText +
                         stateC.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateMachineTwo.GetType() + UpdateLogText +
                         stateC.GetType() + UpdateLogText;
    }
    [Test]
    public void AnyTransitionEventCast() {
        StringBuilder sb = new StringBuilder();

        State stateA = new WriterStateA(sb);
        State stateB = new WriterStateB(sb);
        State stateC = new WriterStateC(sb);
        StateMachineZero stateMachineZero = new StateMachineZero(stateA, stateB, stateC);

        int[] number = { 48 };
        Action<int[]> transitionEvent = null;
        transitionEvent += stateMachineZero.AddAnyEventTransition<int[]>(stateC);


        stateMachineZero.Init();
        stateMachineZero.Update();
        number[0] = 49;
        transitionEvent.Invoke(number);
        stateMachineZero.Update();

        string expected = stateMachineZero.GetType() + EnterLogText +
                         stateA.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateA.GetType() + UpdateLogText +

                         stateA.GetType() + ExitLogText +
                         stateC.GetType() + EnterLogText +

                         stateMachineZero.GetType() + UpdateLogText +
                         stateC.GetType() + UpdateLogText;
    }
}