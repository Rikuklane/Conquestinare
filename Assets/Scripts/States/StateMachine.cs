using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine: MonoBehaviour
{
    protected State state;

    public State State
    {
        get => state;
        set {
            state = value;
            StartCoroutine(state.Start());
        } 
    }
}
