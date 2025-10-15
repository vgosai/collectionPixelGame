using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class Graph 
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

    List<Vector2Int> graph;

    public Graph(IEnumerable<Vector2Int> vertices)
    {
        graph = new List<Vector2Int>(vertices);
    }

    public List<Vector2Int> GetNeighbours4Directions(Vector2Int startPosition)
    {
        return GetNeighbours(startPosition, cardinalDirectionsList);
    }
    public List<Vector2Int> GetNeighbours8Directions(Vector2Int startPosition)
    {
        return GetNeighbours(startPosition, eightDirectionsList);
    }

    private List<Vector2Int> GetNeighbours(Vector2Int startPosition, List<Vector2Int> neighboursList)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        foreach (var neighbourDirection in neighboursList)
        {
            Vector2Int potentialNeighbour = startPosition + neighbourDirection;
            if (graph.Contains(potentialNeighbour))
            {
                neighbours.Add(potentialNeighbour);
            }
        }
        return neighbours;
    }

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
