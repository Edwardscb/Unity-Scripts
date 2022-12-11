using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2
{

    public class AnimatorHook : MonoBehaviour
    {
        CharacterStateManager states;

        public virtual void Init(CharacterStateManager stateManager)
        // he said we're using a virtual void because we will probably also be using this on enemies as well
        // what is a virtual void??
        // Using StateManager (instead of CharacterStateManager - which would also be viable), if it's on the base class then we also know we are passing the player state (? what?)
        {
            states = (CharacterStateManager)stateManager;
        }
        // this is not entirely safe, so we have to make sure that we derive from this (AnimatorHook?) if we want to pass something different, like when we add enemy states

        public void OnAnimatorMove()
        // internal function that runs every time there is a new value on the animator
        {
            OnAnimatorMoveOverride();
        }

        protected virtual void OnAnimatorMoveOverride()
        {
            if (states.useRootMotion == false)
                return;

            if (states.isGrounded && states.delta > 0)
            // just for those cases where delta will be 0, which is very rare, but could happen
            {
                Vector3 v = (states.anim.deltaPosition) / states.delta;
                // this gives us some better values to work with
                v.y = states.rigidbody.velocity.y;
                states.rigidbody.velocity = v;
            }

        }

    }
}
