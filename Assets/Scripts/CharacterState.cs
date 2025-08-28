using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic setup for how a character should behave during certain gameplay conditions
/// </summary>
[CreateAssetMenu(fileName = "CharacterState", menuName = "GathererTopDownRPG/CharacterState")]
public class CharacterState : ScriptableObject
{
    [field: SerializeField] public bool CanMove { get; set; } = true;
    [field: SerializeField] public bool CanExitWhilePlaying { get; set; } = true;
}