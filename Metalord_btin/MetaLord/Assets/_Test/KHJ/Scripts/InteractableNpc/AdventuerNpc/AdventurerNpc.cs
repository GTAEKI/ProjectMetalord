using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        // ���谡 34 
        myDialogue.CheckStateDialogue(34, state);
    }
}