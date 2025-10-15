using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using System.Linq;

public static class DecorandHarvestableGeneration
{
    public static void CreateDecorandObjects(HashSet<Vector2Int> floorPositions, List<TileOpinions> tiles,
        List<GameObject> harvestables, PlaceableObject placeableobject, float decorFreq, Dictionary<Vector2Int, HashSet<Vector2Int>> roomMapsDictionairy,
        HashSet<Vector2Int> corridors, TilemapVisualizer visualizer)
    {
        HashSet<Vector2Int> roomsNoCorridors = floorPositions.Except(corridors).ToHashSet();
        var spotsForHarvestables = FindSpotForHarvestables(roomMapsDictionairy, corridors, roomsNoCorridors, harvestables, placeableobject);
        var spotsToDecorate = FindSpotsToDecorate(roomsNoCorridors, tiles, decorFreq);
        CreateHarvestables(spotsForHarvestables, harvestables, visualizer);
        CreateDecorations(spotsToDecorate, visualizer);
    }

    private static void CreateHarvestables(List<Vector2Int> spotsForHarvestables, List<GameObject> harvestables, TilemapVisualizer visualizer)
    {
        if (harvestables == null || harvestables.Count == 0)
        {
            Debug.LogWarning("No harvestables being placed!");
            return;
        }
        GameObject harvest = GameObject.Find("Harvest_Nodes");
        if (harvest != null)
        {
            Object.Destroy(harvest);
        }
        GameObject Harvest_Nodes = new GameObject("Harvest_Nodes");
        Object.Instantiate(Harvest_Nodes, Vector3.zero, Quaternion.identity);       
        foreach (var pos in spotsForHarvestables)
        {
            if (pos != null)
            {
                // Pick a random prefab
                GameObject prefab = harvestables[Random.Range(0, harvestables.Count)];

                // Convert tile cell position → world position
                Vector3Int cellPos = new Vector3Int(pos.x, pos.y, 0);
                Debug.Log("position" + pos);
                Vector3 worldPos = visualizer.FloorTileMap.CellToWorld(cellPos);

                // (Optional) offset to center of tile
                worldPos += visualizer.FloorTileMap.cellSize / 2;

                // Instantiate
                Object.Instantiate(prefab, worldPos, Quaternion.identity, Harvest_Nodes.transform);
            }

        }
    }

    private static List<Vector2Int> FindSpotForHarvestables(Dictionary<Vector2Int, HashSet<Vector2Int>> roomMapsDictionairy,
        HashSet<Vector2Int> corridors, HashSet<Vector2Int> roomsNoCorridor, List<GameObject> harvestables, PlaceableObject placeableobject)
    {
        List<Vector2Int> spotsForHarvestable = new List<Vector2Int>();
        
        foreach (var room in roomMapsDictionairy)
        {
            Debug.Log("The Number of Rooms in this Dungeon: " + roomMapsDictionairy.Count);
            Debug.Log("number of tiles in current room: " + room.Value.Count);
            ItemPlacementHelper itemPlacementHelper = new ItemPlacementHelper(room.Value, roomsNoCorridor);
            //Debug.Log("Trying to find Item Positions");
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    itemPlacementHelper.GetItemPlacementPosition(placeableobject.placementType, placeableobject.iterationsMax, placeableobject.size, placeableobject.addOffset);
                    spotsForHarvestable.Add(Vector2Int.RoundToInt(itemPlacementHelper.GetItemPlacementPosition(placeableobject.placementType, placeableobject.iterationsMax, placeableobject.size, placeableobject.addOffset).Value));
                    Vector2Int.RoundToInt(itemPlacementHelper.GetItemPlacementPosition(placeableobject.placementType, placeableobject.iterationsMax, placeableobject.size, placeableobject.addOffset).Value);
                }
                catch (System.Exception)
                {
                    Debug.Log("No More Room Left to Place items in this room");
                }
            }
        }
        return spotsForHarvestable;
    }

    private static void CreateDecorations(Dictionary<Vector2Int, TileOpinions> spotsToDecorate, TilemapVisualizer visualizer)
    {
        foreach (var spot in spotsToDecorate)
        {
            visualizer.PaintDecorTile(spot);
        }
    }

    private static Dictionary<Vector2Int, TileOpinions> FindSpotsToDecorate(HashSet<Vector2Int> floorPositions, List<TileOpinions> tiles, float decorFreq)
    {
        Dictionary<Vector2Int, TileOpinions> spotsToDecorate = new();
        List<Vector2Int> decoratableFloor = new List<Vector2Int>(floorPositions);

        // iterate backwards so RemoveAt is safe and O(1) amortized
        for (int i = decoratableFloor.Count - 1; i >= 0; i--)
        {
            if (Random.value < decorFreq)
            {
                var floor = decoratableFloor[i];
                var chosenTileset = tiles[Random.Range(0, tiles.Count)];
                spotsToDecorate[floor] = chosenTileset; // unique key per floor
                decoratableFloor.RemoveAt(i);
            }
        }

        return spotsToDecorate;
        }
}


