using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public NPCEvents npcEvents;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");

        }
        instance = this;

        npcEvents = new NPCEvents();
    }
}
