using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int DungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1; //offset so the new rooms arent just touching each other
    [SerializeField]
    private bool randomWalkRooms = false; //use randomwalk or partitioning and the RandomWalk parameters are protected so are usable for this class
    [SerializeField]
    private List<TileOpinions> tileOpinions;
    [SerializeField]
    [Range(0, 1)]
    private float decorFreq;
    [SerializeField]
    private List<GameObject> harvestables;
    [SerializeField]
    private PlaceableObject placeableObject;
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomMapsDictionairy = new Dictionary<Vector2Int, HashSet<Vector2Int>>(); //key is the start position of each rooms generation and the value is that rooms final hashSet
    private Dictionary<Vector2Int, Dictionary<Vector2Int, HashSet<Vector2Int>>> adjacencyGraph = new Dictionary<Vector2Int, Dictionary<Vector2Int, HashSet<Vector2Int>>>();
    private HashSet<Vector2Int> corridors;
    
    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(DungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomList);
            Debug.Log("num floor tiles: " + floor.Count);
        }
        else
        {
            floor = CreateSimpleRooms(roomList);
        }
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomList)
        {
            Vector2Int center = (Vector2Int)Vector3Int.RoundToInt(room.center);
            roomCenters.Add(center);
        }
        corridors = ConnectRooms(roomCenters);
        Debug.Log("num corridor tiles: " + corridors.Count);
        floor.UnionWith(corridors);
        Debug.Log("num floor tiles after adding corridors: " + floor.Count);
        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        DecorandHarvestableGeneration.CreateDecorandObjects(floor, tileOpinions, harvestables, placeableObject, decorFreq, roomMapsDictionairy, corridors, tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomList.Count; i++)
        {
            var roomBounds = roomList[i];
            var roomCenters = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenters);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
            try
            {
                roomMapsDictionairy.Add(roomCenters, roomFloor);
            }
            catch (ArgumentException)
            {
                Debug.Log("already added");
            }

        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);
        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            Dictionary<Vector2Int, HashSet<Vector2Int>> edges = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
            adjacencyGraph.Add(currentRoomCenter, edges);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            edges.Add(closest, newCorridor);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            //TODO if you want to design each room procedurally you will have to save each room as a separate hash set or object and process it further
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);//currently though we only care about using the same floors to generate everything so no additional processing. 
                }
            }
        }
        return floor;
    }
}
