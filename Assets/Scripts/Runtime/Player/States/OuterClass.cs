using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShortName = OuterClass.InnerClassTwo;
public class OuterClass {
    
    private bool TheProperty {  get; set; }

    public OuterClass() {

    }

    protected void OuterClassMethod() {

    }

    public class InnerClassOne : OuterClass {
        public InnerClassOne() {
            OuterClass outer = new OuterClass();
            outer.TheProperty = true;
        }

        public void InnerClassOneMethod() {

        }
    }

    public class InnerClassTwo : OuterClass {
        public InnerClassTwo() {
            InnerClassOne one = new InnerClassOne();
            one.TheProperty = false;
            one.OuterClassMethod();
        }

        public void InnerClassTwoMethod() {

        }
    }

}

public class ConcreteInnerClassTwo : OuterClass.InnerClassTwo {
    public ConcreteInnerClassTwo() {
        
    }
}



public class AnotherClass {

    public AnotherClass() {
        OuterClass.InnerClassOne one = new OuterClass.InnerClassOne();
        
    }
}


