using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class ProceduralGenerationAlgorithms //Keep in mind if you have a static class can't be a monobehavior right one instance can't slap it on multiple game objects
{
    //Random Walk will make a ranodm path using an agent then we will generate a room at the end of the corridor walk first algorithm
    //In random walk we place an agent then we ask the agent to select a random direction and walk in that direction. 
    //This algorithm literally takes a starting point in the grid and then randomly goes in a direction not going in the same spot for the walklength
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)//hashset is a collection of unique elements if the type implements gethashcode and get equals methods
    {
        //Using hashset guarentees no dupilicates cause we do not want random walk to go on the same location twice. Hashset also has general set functions union, so on
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition);
        var previousPosition = startPosition; //var lets the compiler figure out the type based on context
        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }

    /// <summary>
    /// overall still just a random walk algorithm but in one direction
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="corridorLength"></param>
    /// <returns></returns>
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength) // Because we want access to the last position to get the next position we want a list so we have the order/direction of the path
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition); //might have duplicate position so we still use hashset later to get rid of dups.
        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;
    }

    /// <summary>
    /// BoundsInt is a struct that represents a axis aligned bounding box with all integer values
    /// </summary>
    /// <returns></returns>
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);
        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= minHeight * 2)//horizontal split
                    {
                        SplitHorizontally(minWidth, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }

                }
            }
        }
        return roomsList;
    }

    /// <summary>
    /// To do these splits we must calculate where we are going to do the split along the horizontal length of the room as well as fill our the bounds value of the new room
    /// Keep in mind we dont split at the borders so we must split at at least 1 or the room.size.x-1. 
    /// When we set the values for the new rooms left room is just the same height but x value is just room.min(this is the bottom left point of the bounding box) to the selected split point
    /// Then for the right room the split point value which is then room.size.x thus the width is room.size.x - splitpoint
    /// </summary>
    /// <param name="minWidth"></param>
    /// <param name="roomsQueue"></param>
    /// <param name="room"></param>
    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var xSplit = Random.Range(3, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySplit = Random.Range(3, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0)
    };
    
    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1,1),
        new Vector2Int(1,-1),
        new Vector2Int(-1,-1),
        new Vector2Int(-1,1)
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),
        new Vector2Int(1,1),
        new Vector2Int(1,0),
        new Vector2Int(1,-1),
        new Vector2Int(0,-1),
        new Vector2Int(-1,-1),
        new Vector2Int(-1,0),
        new Vector2Int(-1,1)
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
