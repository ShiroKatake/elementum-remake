using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityQueue : MonoBehaviour
{


    public List<AbilitySlot> queue;
    private static int maxQueueLength = 1;

    

    public void Save()
    {
        foreach (AbilitySlot a in queue)
        {
            {
                if (a.element != null)
                {
                    GameData.queue.Add(a.Element.Name);
                }
                else
                {
                    GameData.queue.Add("empty");
                }
            }
        }
    }

    private void Start()
    {
        //create a refence to the ability slots in the scene
        for (int i = 0; i < 5; i++)
        {
            queue[i] = GameObject.Find("/Player Camera/Slots/Ability Slot " + (i + 1).ToString()).GetComponent<AbilitySlot>();
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
            if (queue[queue.Count - (6-maxQueueLength)].Occupied)
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
                if (queue[i].Occupied)
                {
                    lastActiveSlot = i;
                }
            }

            return lastActiveSlot;
        }
    } 

    public bool Empty
    {
        get
        {
            if (queue[0].occupied)
            {
                return false;
            }
            return true;
        }
    }

    //When a player increases the maximum length of their queue, this will be called
    public void Add()
    {
        queue[maxQueueLength].gameObject.SetActive(true);
        queue[maxQueueLength].active = true;
        maxQueueLength += 1;
    }

    public void AddElement(string element)
    {
        foreach (AbilitySlot a in queue)
        {
            if (!a.Occupied && a.active)
            {
                a.SetElement(element);
                return;
            }
        }
    }

    private void DrawUI()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < maxQueueLength)
            {
                
            }
            else
            {
                queue[i].gameObject.SetActive(false);
                queue[i].active = false;
            }

        }
    }

    public bool Activate(GameObject player)
    {
        return queue[LastOccupiedSlot].Activate(player);
    }

}
