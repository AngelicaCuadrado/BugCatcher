using System.Collections.Generic;
using UnityEngine;

public class SwarmFSM
{
    public class State
    {
        public string name;
        public System.Action onEnter;
        public System.Action onExit;
        public System.Action onStay;
        public List<Transition> transitions = new List<Transition>();

        public override string ToString()
        {
            return name;
        }

    }

    public class Transition
    {
        public State to;
        public System.Func<bool> condition;

        public Transition() { }
        public Transition(State to, System.Func<bool> condition)
        {
            this.to = to;
            this.condition = condition;
        }
    }

    Dictionary<string, State> states = new Dictionary<string, State>();

    public State currentState;
    public State initialState;

    public State CreateState(string name)
    {
        var newState = new State();
        newState.name = name;
        if (states.Count == 0)
        {
            initialState = newState;
        }
        states[name] = newState;
        return newState;
    }

    public void AddTransition(string from, string to, System.Func<bool> condition)
    {
        var transition = new Transition(states[to], condition);
        states[from].transitions.Add(transition);
    }

    public void Update()
    {
        if (states.Count == 0 || initialState == null)
        {
            Debug.Log("No states");
        }
        if (currentState == null) TransitionTo(initialState);
        if (currentState.onStay != null)
        {
            currentState.onStay();
        }

        foreach (var transition in currentState.transitions)
        {
            if (transition.condition())
            {
                TransitionTo(transition.to);
            }
        }
    }

    public void TransitionTo(State newState)
    {
        if (newState == null)
        {
            Debug.Log("New state is null");
            return;
        }
        if (currentState != null && currentState.onExit != null)
        {
            currentState.onExit();
        }
        Debug.LogFormat("Transition from state '{0}' to state '{1}'", currentState, newState);
        currentState = newState;
        if (newState.onEnter != null)
        {
            newState.onEnter();
        }
    }

    public void TransitionTo(string newStateName)
    {
        if (!states.ContainsKey(newStateName))
        {
            Debug.Log("StateMachine doesn't contain the state " + newStateName);
            return;
        }
        var state = states[newStateName];
        TransitionTo(state);
    }
}
