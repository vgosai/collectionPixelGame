using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableObject_", menuName = "PCG/PlaceableObject")]
public class PlaceableObject : ScriptableObject
{
    public Vector2Int size;
    public PlacementType placementType;
    public bool addOffset;
    public int iterationsMax;
}
