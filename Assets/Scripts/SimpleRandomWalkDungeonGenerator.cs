using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    //We are putting everything into a wrapper abstract dungeon generator class for each algorithm thus we have moved the Visualizer for the tilemap field and startposition into the abstract class as those are consist with all implementations

    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters; //we make this protected because we want corridorgenerator to be able to access it


    /*private void Awake()
    {
        RunProceduralGeneration();
    }*/
    /// <summary>
    /// Generates the floor positions for this algorithm in this case we are just running the randomwalk algorithm to generate our floor
    /// </summary> Keep in mind this is a method from abstract class must be overrided. 
    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    
    /// <summary>
    /// Runs the randomwalk functions based on the procedural generation parameters set by the class, startpositions, iterations, and walklength, and start randomly
    /// </summary>
    /// <returns></returns> The final floor positions for the program
    /// <exception cref="NotImplementedException"></exception>
    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < randomWalkParameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength); //keep in mind the start position will be the same everytime so we will kind of generate a island when we start from the same location
            floorPositions.UnionWith(path); //We use union because it allows us to combine our floor from previous iterations into the final floor positions without copying any duplicates
            if (randomWalkParameters.startRandomlyEachIteration) //If we start from different locations we can get that corridor kind of look with each run of the simple random walk vs that island
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count)); //selects random position from hashSet we need elementat because hashset is unordered list so we dont have anyway of indexing to select a "random position", so we use System.Linqs element at class to put into indexable list and random at function with a range set to 0 to the count of the floor position hashset
            }
        }
        return floorPositions;
    }
}
