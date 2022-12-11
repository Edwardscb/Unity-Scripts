using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2
{

    public class InputManager : StateAction
    // because it is a StateAction, it needs to inherit from the abstract class
    {

        PlayerStateManager s;
        // not using StateManager because you have to do a casting to try to use PlayerStateManager from this code if you do try to use StateManager
        // while it's possible to still use PlayerStateManager, using casting, we still have to check that our state is PlayerStateManager
        // this way skips the middleman

        // Triggers & bumpers - for all the below private bools, could have them as one line (or maybe two?) by just declaring bool once and then doing something like:
        // bool Rb, Rt, Lb, Lt, isAttacking, ...etc...
        bool Rb;
        bool Rt;
        bool Lb;
        bool Lt;
        bool isAttacking;
        // Inventory
        bool inventoryInput;
        // Prompts
        bool b_Input;
        bool y_Input;
        bool x_Input;
        // Dpad
        bool leftArrow;
        bool rightArrow;
        bool upArrow;
        bool downArrow;
        //since it's a button, it doesn't need to be public (?)

        public InputManager(PlayerStateManager states)
        // connect input manager to something else we are going to use the constructor for the class
        {
            s = states;
        }

        public override bool Execute()
        // because it needs to return a bool, it may or may not break the flow of other interactions so false for now
        {

            bool retVal = false;
            isAttacking = false;

            s.horizontal = Input.GetAxis("Horizontal");
            s.vertical = Input.GetAxis("Vertical");
            // could also use GetButtonDown or GetButtonUp(?)
            Rb = Input.GetButton("RB");
            Rt = Input.GetButton("RT");
            Lb = Input.GetButton("LB");
            Lt = Input.GetButton("LT");
            inventoryInput = Input.GetButton("Inventory");
            b_Input = Input.GetButton("B");
            y_Input = Input.GetButtonDown("Y");
            x_Input = Input.GetButton("X");
            leftArrow = Input.GetButton("Left");
            rightArrow = Input.GetButton("Right");
            upArrow = Input.GetButton("Up");
            downArrow = Input.GetButton("Down");
            s.mouseX = Input.GetAxis("Mouse X");
            // the s in s.mouseX represents PlayerStateManager - we have to call it from there because that is where mouseX lives
            s.mouseY = Input.GetAxis("Mouse Y");

            s.moveAmount = Mathf.Clamp01(Mathf.Abs(s.horizontal) + Mathf.Abs(s.vertical)); 
            // find the move amount. Clamp01 makes it between 0 and 1 so you don't have to add those parameters at the end of the statement(?)
           
            

            retVal = HandleAttacking();
            
            
            return retVal;
            // if this returns true, it will basically skip all the other actions, because if you are attacking, you don't want to be doing something else
        }

        bool HandleAttacking()
        {
            if (Rb || Rt || Lb || Lt)
            {
                isAttacking = true;

            }

            // Logic that interrupts attack can be placed here
            if (y_Input)
            {
                isAttacking = false;
            }

            if (isAttacking) 
            {
                // Find the actual attack animation from the items etc..
                // play animation
                s.PlayTargetAnimation("Attack 1", true);
                s.ChangeState(s.attackStateId);
                
                
            }

            return isAttacking;
        }

        

    }
}
