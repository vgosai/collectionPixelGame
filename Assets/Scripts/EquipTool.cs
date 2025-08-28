using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO make robust inventory system that has inventory that you can drag tools into hotbar
//Keep in mind to actually make button do something when clicked you must assign this script to the onclick unity event and the corresponding function
public class EquipTool : MonoBehaviour
{
    [field: SerializeField] public Tool Tool { get;  set; }//public set since attached to hotbar since you might want to change the tool equiped on the hotbar with other scripts.
    private Harvesting _targetHarvesting;

    public void Start() //keep in mind that we do this search for each button.equiptool script existing so we might want to make a manager script that assigns the harvesting to each equiptool object
    {
        _targetHarvesting = FindAnyObjectByType<PlayerController>().GetComponentInChildren<Harvesting>();
    }
    public void ChangeTool()
    {
        if (_targetHarvesting != null)
        {
            _targetHarvesting.Tool = Tool;
        }
        else
        {
            Debug.LogWarning("There is no playercontroller component to get harvestable");
        }
    }
}
