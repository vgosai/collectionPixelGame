using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    [field: SerializeField] public Resource ResourceType { get; private set; }
}