using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObiruWillberNpc : NpcBase, IInteractNpc
{
    public void InteractNpc()
    {
        //���� ����
        myDialogue.CheckStateDialogue(22, state);
    }
}


