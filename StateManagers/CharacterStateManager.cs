using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2
{
    public abstract class CharacterStateManager : StateManager // adding StateManager here after the colon makes it derive (inherit?) from StateManager
    // making it abstract means we can't simply drop the CharacterStateManager onto a game object (in this case that's why we did it, any way)
    {
        [Header("References")]
        public Animator anim;
        // every character will have animations
        public new Rigidbody rigidbody;
        public AnimatorHook animHook;

        [Header("States")] // moved this from player state manager to here because enemies can use these too
        public bool isGrounded;
        public bool useRootMotion;

        [Header("Controller Values")]
        public float vertical;
        public float horizontal;
        public bool lockOn;
        public float delta;
        public Vector3 rootMovement;



        public override void Init()
        // implement the abstract class
        {
            anim = GetComponentInChildren<Animator>();
            animHook = GetComponentInChildren<AnimatorHook>();
            rigidbody = GetComponentInChildren<Rigidbody>();
            anim.applyRootMotion = false;

            animHook.Init(this);
            // initializes the animHook and passes "this" state manager

        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

    }
}
