using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PickupReasources : MonoBehaviour
{
    [field: SerializeField] public Character_Inventory Inventory { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision) //Keep in mind oncollision is for physics object entering physics objects when trigger collider encounters a collider then this one
    {
        ResourcePickup pickup = collision.gameObject.GetComponent<ResourcePickup>();

        if (pickup)
        {
            Inventory.AddResources(pickup.ResourceType, 1);
            Destroy(pickup.gameObject);
        }
    }
}
