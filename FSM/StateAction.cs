using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2
{
    public abstract class StateAction
    // base class for our actions which means it will be an abstract class
    {
        public abstract bool Execute();
        // so if we want to interrupt the state we won't have to use any force-exit
        // it's just an Execute() with nothing on the signature
    }
}
