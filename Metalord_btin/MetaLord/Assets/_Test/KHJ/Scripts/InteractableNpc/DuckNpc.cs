using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        //���� 40
        myDialogue.CheckStateDialogue(40, state);
    }
}

