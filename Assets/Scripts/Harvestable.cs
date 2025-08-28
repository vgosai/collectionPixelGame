using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//How we are going to break this down is a harvestable object that emits the particles based on interactions with the tool collider then the visuals will be paired as a child object so all tags and processing on the parent.
//Last thing is that we need to form a gameobject where the particle would be destroyed on exiting.
public class Harvestable : MonoBehaviour
{
    [field: SerializeField] public ToolType HarvestingType { get; private set; }
    [field: SerializeField] public ParticleSystem ReasourceEmitPS { get; private set; }
    private int amountHarvested = 0;
    [field: SerializeField] public int ReasouceCount { get; private set; }

    public bool TryHarvest(ToolType harvestingType, int amount)
    {
        
        if (harvestingType == HarvestingType)
        {
            Harvest(amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amountEmitted"></param> Represents the amount of particles the object emits on each hit
    private void Harvest(int amount)
    {
        int spawns = Mathf.Min(amount, ReasouceCount - amountHarvested);
        if (spawns > 0)
        {
            ReasourceEmitPS.Emit(spawns);
            amountHarvested += spawns;
        }
        if (amountHarvested >= ReasouceCount)
        {
            Destroy(gameObject);
        }
    }
}
