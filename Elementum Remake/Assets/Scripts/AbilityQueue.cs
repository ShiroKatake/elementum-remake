using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityQueue : MonoBehaviour
{


    public List<AbilitySlot> queue;
    private int maxQueueLength = 2;
    private SpriteRenderer uIslot;

    public GameObject Air;
    public GameObject Fire;
    public GameObject Earth;

    public void Save()
    {
        foreach (AbilitySlot a in queue)
        {
            if (a.element != null)
            {
                GameData.queue.Add(a.Element.Name);
            }
        }
    }

    private void Awake()
    {
        
        //create a refence to the ability slots in the scene
        for (int i = 0; i < maxQueueLength; i++)
        {
            queue[i] = GameObject.Find("/Player Camera/Slots/Ability Slot " + (i + 1).ToString()).GetComponent<AbilitySlot>();
            try
            {
                Debug.Log("trying :" + GameData.queue[i]);
                GameObject e = null;
                switch (GameData.queue[i])
                {
                    case "Air":
                        e = Instantiate(Air);
                        break;
                    case "Fire":
                        e = Instantiate(Fire);
                        break;
                    case "Earth":
                        e = Instantiate(Earth);
                        break;
                }
                AddElement(e);
            }
            catch { }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawUI();
    }

    public bool Full
    {
        get
        {
            if (queue[queue.Count - 1].occupied)
            {
                return true;
            }
            return false;
        }
    }

    public int LastOccupiedSlot
    {
        get
        {
            int lastActiveSlot = 0;
            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i].occupied)
                {
                    lastActiveSlot = i;
                }
            }

            return lastActiveSlot;
        }
    } 

    //When a player increases the maximum length of their queue, this will be called
    public void Add()
    {
        if (queue.Count < maxQueueLength)
        {
            queue.Add(new AbilitySlot());
        }
    }

    public void AddElement(GameObject element)
    {
        foreach (AbilitySlot a in queue)
        {
            if (!a.occupied)
            {
                a.SetElement(element);
                return;
            }
        }
    }

    private void DrawUI()
    {
    }

    public void Activate(PlayerMovement player)
    {
        queue[LastOccupiedSlot].Activate(player);
    }

}
