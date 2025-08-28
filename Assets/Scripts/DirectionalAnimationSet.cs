using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

/*
This holds the different animation directions data in one and will tell you which one to play based off of the input direction.
Why we use scriptableObject: A ScriptableObject is ideal for storing reusable, condition-based data that you can access at runtime or configure in the editor. It’s lightweight, doesn’t require a GameObject, and it centralizes your data, 
which is great for clean and scalable code.
Keep in mind a instance of a scriptable object exists in the assets folder but not in the scene and you can use it as a database that you can reference for data whenever you need it across the game. Its for stuff that never really changes and you might need a gameobject in the scene to be materialized with this data. 
Overall it is good for storing data that will be consistent
The idea is whenever we reference the scriptable object we can pull all this data.
Summary
Based on a normalized input direction, returns the vector2 direction in the list of directionstocheck that is closest to the input direction by comparing the distance between the inputs and all directions vector2 list. param inputDirection the normalized direction to compare to the directions and returns the closest direction.
*/
//for the parameter we are taking the direction we expect to be facing not the actual vector so we just a normalized vector so we have the direction so no magnitude.
//Get closest direction to the input cause we only move in 4 direction so we want the vector which isnt going to be exact cause it might 
//Return Animation Clip based on closest direction
[CreateAssetMenu(fileName = "DirectionalAnimationSet", menuName = "GathererTopDownRPG/DirectionalAnimationSet")]
public class DirectionalAnimationSet : ScriptableObject
{
    [field: SerializeField] public AnimationClip Up { get; private set; }
    [field: SerializeField] public AnimationClip Down { get; private set; }
    [field: SerializeField] public AnimationClip Left { get; private set; }
    [field: SerializeField] public AnimationClip Right { get; private set; }

    /// <summary>
    /// After comparing the expected facing direction to the direction options, return the animation clip
    /// that most closely matches the input facing direction
    /// </summary>
    /// <param name="facingDirection">The directional input for controlling where the character faces</param>
    /// <returns>The animation clip most closely matching the facingDirection</returns>
    /// <exception cref="ArgumentException">Throws when a closest direction is unaccounted for returned by GetClosestDirection</exception>
    public AnimationClip GetFacingClip(Vector2 facingDirection)
    {
        // Get the closest direction to the input
        Vector2 closestDirection = GetClosestDirection(facingDirection);

        // Return the animation clip based on closest direction
        if (closestDirection == Vector2.left)
        {
            return Left;
        }
        else if (closestDirection == Vector2.right)
        {
            return Right;
        }
        else if (closestDirection == Vector2.up)
        {
            return Up;
        }
        else if (closestDirection == Vector2.down)
        {
            return Down;
        }
        else
        {
            throw new ArgumentException($"Direction not expected {closestDirection}");
        }
    }

    /// <summary>
    /// Based on a normalized input direction, return the Vector2 direction in the list of directionsToCheck
    /// that is closest to the input direction by comparing the distance between the input
    /// and all direction Vector2s in the list
    /// </summary>
    /// <param name="inputDirection">The normalized direction to compare to the directionsToCheck</param>
    /// <returns>The closest direction represented as a Vector2</returns>
    public Vector2 GetClosestDirection(Vector2 inputDirection)
    {
        Vector2 normalizedDirection = inputDirection.normalized;

        Vector2 closestDirection = Vector2.zero;
        float closestDistance = 0f;
        bool firstSet = false;

        Vector2[] directionsToCheck = new Vector2[4] { Vector2.down, Vector2.up, Vector2.left, Vector2.right };

        for (int i = 0; i < directionsToCheck.Length; i++)
        {
            if (!firstSet)
            {
                // Set when no comparison to be made yet
                closestDirection = directionsToCheck[i];
                closestDistance = Vector2.Distance(inputDirection, directionsToCheck[i]);
                firstSet = true;
            }
            else
            {
                // Compare to the current closest direction and distance
                float nextDistance = Vector2.Distance(inputDirection, directionsToCheck[i]);

                if (nextDistance < closestDistance)
                {
                    // Closer vector found!
                    closestDistance = nextDistance;
                    closestDirection = directionsToCheck[i];
                }
            }
        }

        return closestDirection;
    }
}