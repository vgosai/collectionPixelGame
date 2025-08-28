using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Big Point: When you have things like the pickupgravity script on the child object it uses the collider attached to that object but because you want the actually picking up to be done with the primary small player collider at the feet of the player attaching the plickupreasources to the parent object might not make it so you are using the exact collider that you want So the bext way to fix this is to put the script onto a child object so it uses the specific collider.
public class PickupGravity : MonoBehaviour
{
    [field: SerializeField] public float GravitySpeed { get; private set; } = 0.2f;
    private List<ResourcePickup> _nearbyResources = new();

    private void FixedUpdate() //cause these objects dont have a rigidbody we dont want to just add a force to them so we just want to move the position a set amount each fixedupdate toward the center point of the player
    {
        // Make sure all pickups in the list are valid because once we pickup the thing the reasource is just null and that is left in the nearbyreasources list so a good habit is to regularly clean up your lists after removal.
        _nearbyResources.RemoveAll(pickup => pickup == null);

        foreach (ResourcePickup pickup in _nearbyResources)
        {
            Vector2 directionToCenter = (transform.position - pickup.transform.position).normalized; //just get direction so we can multiply by speed so we can control slurping rate

            pickup.transform.Translate(directionToCenter * GravitySpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //detect object to grab
    {
        ResourcePickup pickup = collision.gameObject.GetComponent<ResourcePickup>();

        if (pickup)
        {
            _nearbyResources.Add(pickup);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //objects moves too far from player
    {
        ResourcePickup pickup = collision.gameObject.GetComponent<ResourcePickup>();

        if (pickup)
        {
            _nearbyResources.Remove(pickup);
        }
    }
}