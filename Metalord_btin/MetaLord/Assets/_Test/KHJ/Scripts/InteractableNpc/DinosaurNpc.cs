using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        // ���� 19
        myDialogue.CheckStateDialogue(19, state);
    }
}

