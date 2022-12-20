using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2
{
    // this will only be used for the player to move around
    public class MovePlayerCharacter : StateAction
    {
        // we want a reference to the PlayerStateManager
        PlayerStateManager states;


        // create a constructor
        public MovePlayerCharacter(PlayerStateManager playerStateManager)
        {
            states = playerStateManager;
        }

        public override bool Execute()
        {

            float frontY = 0;
            RaycastHit hit;
            Vector3 targetVelocity = Vector3.zero;  // do this and then the if statement to do as few operations as possible:

            if (states.lockOn)
            {
                targetVelocity = states.mTransform.forward * states.vertical * states.movementSpeed;
                targetVelocity += states.mTransform.right * states.horizontal * states.movementSpeed;
            }
            else
            {

                targetVelocity = states.mTransform.forward * states.moveAmount * states.movementSpeed;
            }

            Vector3 origin = states.mTransform.position + (targetVelocity.normalized * states.frontRayOffset); // (states.mTransform.forward * states.frontRayOffset) is the direction we want to go

            origin.y += .5f;
            Debug.DrawRay(origin, -Vector3.up, Color.red, .01f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, 1, states.ignoreForGroundCheck))
            {
                float y = hit.point.y;
                frontY = y - states.mTransform.position.y;
                // this checks for angle

            }

            Vector3 currentVelocity = states.rigidbody.velocity;

            // if (states.isLockingOn)
            // {
            //     targetVelocity = states.rotateDirection * states.moveAmount * movementSpeed;
            // }

            if (states.isGrounded)
            {
                float moveAmount = states.moveAmount;

                if (moveAmount > 0.1f)
                {
                    states.rigidbody.isKinematic = false;
                    states.rigidbody.drag = 0;
                    if (Mathf.Abs(frontY) > 0.02f)
                    {
                        targetVelocity.y = ((frontY > 0) ? frontY + 0.2f : frontY - 0.2f) * states.movementSpeed;
                    }
                }
                else
                {
                    float abs = Mathf.Abs(frontY);

                    if (abs > 0.02f)
                    {
                        states.rigidbody.isKinematic = true;
                        targetVelocity.y = 0;
                        states.rigidbody.drag = 4;
                    }

                }

                HandleRotation();
            }
            else
            {
                //states.collider.height = colStartHeight;
                states.rigidbody.isKinematic = false;
                states.rigidbody.drag = 0;
                targetVelocity.y = currentVelocity.y;
            }

            HandleAnimations();

            Debug.DrawRay((states.mTransform.position + Vector3.up * .2f), targetVelocity, Color.green, 0.01f, false);
            states.rigidbody.velocity = Vector3.Lerp(currentVelocity, targetVelocity, states.delta * states.adaptSpeed);

            return false;
        }

        void HandleRotation()
        {

            // added moveOverride as states.moveAmount, added it in the if state for states.lockOn, as well as the last Quaternion targetRotation so the user doesn't have to move for the camera to rotate on lock on

            Vector3 targetDir = Vector3.zero;
            float moveOverride = states.moveAmount;
            
            if (states.lockOn)
            {
                targetDir = states.target.position - states.mTransform.position;
                moveOverride = 1;
            }
            else 
            {
                // can get horizontal and vertical from PlayerStateManager (which inherits from CharacterStateManager)
                float h = states.horizontal;
                float v = states.vertical;

                targetDir = states.camera.transform.forward * v;
                targetDir += states.camera.transform.right * h;
            }

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = states.mTransform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(
                states.mTransform.rotation, tr,
                states.delta * moveOverride * states.rotationSpeed);
            
            states.mTransform.rotation = targetRotation;
            
        }

        void HandleAnimations()
        {
            if(states.isGrounded)
            {
                if (states.lockOn)
                {
                    // the below will pass new values to our animations
                    float v = Mathf.Abs(states.vertical);
                    float f = 0;
                    if (v > 0 && v <= 0.5f)
                        f = 0.5f;
                    else if (v > 0.5f)
                        f = 1;
                    
                    // we are adding the following code so that the values returned from moving left can be interpreted (negatives)
                    if (states.vertical < 0)
                        f = -f;

                    states.anim.SetFloat("forward", f, 0.2f, states.delta);

                    float h = Mathf.Abs(states.horizontal);
                    // the s is for sideways
                    float s = 0;
                    if (h > 0 && h <= 0.5f)
                        s = 0.5f;
                    else if (h > 0.5f)
                        s = 1;

                    // see note above for f
                    if (states.horizontal < 0)
                        s = -1;

                    states.anim.SetFloat("sideways", s, 0.2f, states.delta);
                }
                else
                {
                    // also, the above allows us to make sure that sideways is always 0 if we want
                    float m = states.moveAmount;
                    float f = 0;
                    if (m > 0 && m <= 0.5f)
                        f = 0.5f;
                    else if (m > 0.5f)
                        f = 1;

                    states.anim.SetFloat("forward", f, 0.2f, states.delta);
                    states.anim.SetFloat("sideways", 0, 0.2f, states.delta);
                }
            }
            else
            {

            }
        }

    }
}
