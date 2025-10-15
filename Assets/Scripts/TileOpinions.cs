using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "TileOptions", menuName = "PCG/TileOptions")]
public class TileOpinions : ScriptableObject
{
    [SerializeField]
    private TileBase tile1, tile2, tile3, tile4;
    [SerializeField]
    private bool groundLevel;
    public bool GroundLevel { get; private set; }

    public TileBase GetRandomTile()
    {
        TileBase[] tiles = { tile1, tile2, tile3, tile4 };

        // Filter out any nulls (in case some slots aren’t filled)
        List<TileBase> validTiles = new List<TileBase>();
        foreach (var t in tiles)
            if (t != null) validTiles.Add(t);

        if (validTiles.Count == 0) return null;

        int index = Random.Range(0, validTiles.Count); // inclusive min, exclusive max
        return validTiles[index];
    }
}
