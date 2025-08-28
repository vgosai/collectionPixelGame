using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Connects a state to a directional set of animations for a character
/// Can get the correct facing clip based on the state from the directional animation set using a passed
/// in facing direction
/// </summary>
[Serializable]
public class StateAnimationSetDictionary : SerializableDictionary<CharacterState, DirectionalAnimationSet>
{
    /// <summary>
    /// Check the StateAnimations dictionary for a corresponding set of animations and then return
    /// the matching directional clip based on the facing direction if it exists in the dictionary.
    /// </summary>
    /// <param name="characterState">State to check for corresponding animations</param>
    /// <param name="facingDirection">The direction to get the corresponding animation from the directional animation set for</param>
    /// <returns>The directional animation clip matching the state and facing direction if the animation set is found in the dictionary</returns>
    public AnimationClip GetFacingClipFromState(CharacterState characterState, Vector2 facingDirection)
    {
        if (TryGetValue(characterState, out DirectionalAnimationSet animationSet))
        {
            return animationSet.GetFacingClip(facingDirection);
        }
        else
        {
            Debug.LogError($"Character state {characterState.name} is not found in the StateAnimations dictionary");
        }

        // Failed to find animation set and animation clip corresponding to the character state
        return null;
    }
}