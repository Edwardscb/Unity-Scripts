using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2
{
    public class State
    {
        bool forceExit;
        List<StateAction> fixedUpdateActions;
        // List of state actions and also need it to be fixed update because we have/had less problems when they were fixed update instead of just update actions
        List<StateAction> updateActions;
        List<StateAction> lateUpdateActions;
        // the above don't require = new List<StateAction>(); because we are doing it in PlayerStateManager

        public delegate void OnEnter();
        public OnEnter onEnter;
        // using on enter because we want to initialize when we start the state (instead of when we exit using on exit)


        // instead of making each of the List items public, we can make a public constructor:
        public State(List<StateAction> fixedUpdateActions, List<StateAction> updateActions, List<StateAction> lateUpdateActions)
        {
            this.fixedUpdateActions = fixedUpdateActions;
            this.updateActions = updateActions;
            this.lateUpdateActions = lateUpdateActions;
        }

        // he said that they may end up not using these, but we'll add it here any way
        //need to add some methods for ticks that we will be using:
        public void FixedTick()
        {
            ExecuteListOfActions(fixedUpdateActions);
        }

        public void Tick()
        {
            ExecuteListOfActions(updateActions);
        }

        public void LateTick()
        {
            ExecuteListOfActions(lateUpdateActions);
            forceExit = false;
            // used to cancel the bool forceExit
        }

        void ExecuteListOfActions(List<StateAction> l)
        {
            // this is a way to run all our actions
            // since the Lists above are initialized, we don't need a way to check if the list does or doesn't have any actions
            for (int i = 0; i < l.Count; i++)
            {
                if (forceExit)
                    return;

                forceExit = l[i].Execute();
            }
        }
    }
}