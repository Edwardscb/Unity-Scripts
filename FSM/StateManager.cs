using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2
{
    // base template for our state manager
    public abstract class StateManager : MonoBehaviour
    // since abstract class, we need to create another class that inherits from this class
    {
        State currentState;
        // the current state..
        Dictionary<string, State> allStates = new Dictionary<string, State>();
        // dictionaries are optimized ways for handling multiple data
        // have string as the key because states are going to be unique
        // the = new .... is initializing the dictionary "allStates"

        [HideInInspector]
        public Transform mTransform;
        // we don't really care if this is in the inspector or not so we hide it

        private void Start()
        {
            // instead of using: this.transform here.  We use mTransform = this.transform; which is casting and makes the process faster for accessing your transform
            // this has something to do with Unity - he didn't fully explain why though
            mTransform = this.transform;

            Init();
        }

        public abstract void Init();
        // since our other states will be inheriting from this class we want to make sure that everything is properly initialized
        // going to have to use Init method every time you derive from this class

        public void FixedTick()
        {
            // first we need to know if null
            if (currentState == null)
                return;
            
            currentState.FixedTick();
        }

        public void Tick()
        {
            // first we need to know if null
            if (currentState == null)
                return;
            
            currentState.Tick();
        }

        public void LateTick()
        {
            // first we need to know if null
            if (currentState == null)
                return;
            
            currentState.LateTick();
        }

        public void ChangeState(string targetId)
        {

            if (currentState != null)
            {
                // Run on exit actions of currentState
            }

            State targetState = GetState(targetId);
            // run on enter actions

            currentState = targetState;
            // this way we've changed our state

            currentState.onEnter?.Invoke();
            // the above code is basically the same as: if (currentState.onEnter != null) { currentState.onEnter.Invoke(); }
            // this will invoke our method
        }

        State GetState(string targetId)
        // also need a way to get back our state
        {
            allStates.TryGetValue(targetId, out State retVal);
            return retVal;
        }


        protected void RegisterState(string stateId, State state)
        // can override it from StateManager but cannot from any where else, because we don't need to
        {
            allStates.Add(stateId, state);
        }

    }
}
