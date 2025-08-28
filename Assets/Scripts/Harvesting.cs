using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
To animate the tool being behind and in front of the player we changed the pivot point to be negative of the hilt of the tool so when the player is looking right then the pivot point is lower then the players pivot but when you look left we animate it so the tool moves up a bit moving its pivot past and also putting the tool behind the player. We go to the respective animations and key frame the child tool object in its respective animations.
Q: hand tool, W: position transform, E: Rotation, R: Scale Tool
What we are going to do is when the tool hits the harvestable it is going to trigger the particle system to create the gameobject who when they leave a killzone around the harvestable will then be destroyed and then also added to player who has most likely a inventory component on them.
*/
public class Harvesting : MonoBehaviour
{
    //When we set the tool we now want to swap out the sprite and tooltype scriptable object
    public Tool Tool { get
        {
            return _tool;
        }
        set
        {
            if (_tool != value)
            {
                _tool = value;
                //update sprite
                UpdateSprite();
            }
        }
    }

    //keep in mind to do null checks like at the start of the script tool might be set to null on the game tool object. Thus we run it at the start of the script to make sure that _tool isnt set to null at any point
    //update sprite to equiped tool
    private void UpdateSprite()
    {
        if (_tool != null)
        {
            _renderer.sprite = _tool.Sprite;
        }
        else
        {
            _renderer.sprite = null;
        }
    }

    [SerializeField] private Tool _tool; //keep in mind now that we want to use the property to set the tool setting it in the inspector will not run the property setter code thus the update will not occur! So we link our buttons with the property code.
    private SpriteRenderer _renderer;

    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Harvestable harvestable = collision.GetComponent<Harvestable>();
        if (harvestable != null)
        {
            int amountToHarvest = UnityEngine.Random.Range(Tool.MinHarvest, Tool.MaxHarvest);
            harvestable.TryHarvest(Tool.Type, amountToHarvest);
        }
    }
}
