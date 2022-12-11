using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2 {
    // this will have specific things that only the player will need
    public class PlayerStateManager : CharacterStateManager
    // derives from the CharacterStateManager
    {

        //variables that only the player would have to use
        [Header("Inputs")]
        public float mouseX;
        public float mouseY;
        public float moveAmount;
        public Vector3 rotateDirection;


        [Header("References")]
        public new Transform camera;
        public Transform target;

        [Header("Movement Stats")]
        public float frontRayOffset = 0.5f;
        public float movementSpeed = 4;
        public float adaptSpeed = 10;
        public float rotationSpeed = 10;

        [HideInInspector]
        public LayerMask ignoreForGroundCheck;

        // these will not or should not be changed at runtime, so make them constant
        [HideInInspector]
        public string locomotionId = "locomotion";
        [HideInInspector]
        public string attackStateId = "attackState";
        
        public override void Init()
        // bc the character state manager already instantiates the init, we don't necessarily need it here, so we will call base.Init()
        {
            base.Init();

            State locomotion = new State(
                // since CharacterStateManager constructor requires the list arguments, we have to pass them here
                new List<StateAction>() // Fixed Update
                {
                    // not using a semi-colon above because it is not the end of the statement, the ending parenthesis is
                    new MovePlayerCharacter(this)
                },
                new List<StateAction> () // Update
                {
                    // we only get inputs on update
                    new InputManager(this),
                },
                new List<StateAction> () // Late Update
                {
                }
            );

            locomotion.onEnter = DisableRootMotion;
            // since this is the first event, we have to assign it

            State attackState = new State(
                // attackState is another state that we know will be happening at some point
                // since CharacterStateManager constructor requires the list arguments, we have to pass them here
                new List<StateAction>() // Fixed Update
                {
                },
                new List<StateAction> () // Update
                {
                    new MonitorInteractingAnimation(this, "isInteracting", locomotionId),
                    // now we have some logic that will allow us to go into attack state and when it is finished we can go back to locomotion
                },
                new List<StateAction> () // Late Update
                {
                }
            );

            attackState.onEnter = EnableRootMotion;

            RegisterState(locomotionId, locomotion);
            // we want to register the name of the state, which in this case = locomotion
            RegisterState(attackStateId, attackState);
            // looks like we will be doing this for all of our states

            ChangeState(locomotionId);
            // starting state

            ignoreForGroundCheck = ~(1 << 9 | 1 << 10);
            // initializes our values for the ignoreForGroundCheck

        }

        private void FixedUpdate()
        // runs Fixed Tick from StateManager(?)
        {
            delta = Time.fixedDeltaTime;

            base.FixedTick();
        }

        private void Update()
        {
            delta = Time.deltaTime;

            base.Tick();
        }

        private void LateUpdate()
        {
            // since we also have forced exit on late update, we need to implement the late update here as well
            base.LateTick();
        }

        #region State Events
        // 

        void DisableRootMotion()
        {
            useRootMotion = false;
        }

        void EnableRootMotion()
        {
            useRootMotion = true;
        }

        #endregion


    }
}
