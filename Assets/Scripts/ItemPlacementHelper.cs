using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class ItemPlacementHelper
{
    Dictionary<PlacementType, HashSet<Vector2Int>> potentialPlacementLocations = new Dictionary<PlacementType, HashSet<Vector2Int>>();
    HashSet<Vector2Int> roomFloorNoCooridor;

    public ItemPlacementHelper(HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCooridor)
    {
        Graph graph = new Graph(roomFloor);
        this.roomFloorNoCooridor = roomFloorNoCooridor;
        foreach (var position in roomFloorNoCooridor)
        {
            int neighboursCount8Dir = graph.GetNeighbours8Directions(position).Count;
            PlacementType type = neighboursCount8Dir < 8 ? PlacementType.NearWall : PlacementType.OpenSpace;
            if (potentialPlacementLocations.ContainsKey(type) == false)
            {
                potentialPlacementLocations[type] = new HashSet<Vector2Int>();
            }
            if (type == PlacementType.NearWall && graph.GetNeighbours4Directions(position).Count < 3) //dont place objects that block off paths
            {
                continue;
            }
            potentialPlacementLocations[type].Add(position);
        }
    }

    public Vector2? GetItemPlacementPosition(PlacementType placementType, int iterationsMax, Vector2Int size, bool addOffset)
    {
        int itemArea = size.x * size.y;
        if (potentialPlacementLocations[placementType].Count < itemArea)
        {
            return null;
        }
        int iteration = 0;
        while (iteration < iterationsMax)
        {
            iteration++;
            int index = UnityEngine.Random.Range(0, potentialPlacementLocations[placementType].Count);
            Vector2Int position = potentialPlacementLocations[placementType].ElementAt(index);
            if (itemArea > 1)
            {
                var (result, placementPositions) = PlaceBigItem(position, size, addOffset);
                if (result == false)
                {
                    continue;
                }
                potentialPlacementLocations[placementType].ExceptWith(placementPositions);
                potentialPlacementLocations[PlacementType.NearWall].ExceptWith(placementPositions);
            }
            else
            {
                potentialPlacementLocations[placementType].Remove(position);
            }
            return position;
        }
        return null; //when you put ? after return data type it means can return whatever the return type is and null.
    }
    private (bool, List<Vector2Int>) PlaceBigItem(
        Vector2Int originPosition,
        Vector2Int size,
        bool addOffset)
    {
        List<Vector2Int> positions = new List<Vector2Int>() { originPosition };
        int maxX = addOffset ? size.x + 1 : size.x; //most likely just condense into just changing addoffset into a int variable and do 2*offset to calculate maximum neccessary rectangle around object
        int maxY = addOffset ? size.y + 1 : size.y;
        int minX = addOffset ? -1 : 0;
        int minY = addOffset ? -1 : 0;

        for (int row = minX; row <= maxX; row++)
        {
            for (int col = minY; col <= maxY; col++)
            {
                if (col == 0 && row == 0)
                {
                    continue;
                }
                Vector2Int newPosToCheck = new Vector2Int(originPosition.x + row, originPosition.y + col);
                if (roomFloorNoCooridor.Contains(newPosToCheck) == false)
                {
                    return (false, positions);
                }
                positions.Add(newPosToCheck);
            }
        }
        return (true, positions);
    }
}

