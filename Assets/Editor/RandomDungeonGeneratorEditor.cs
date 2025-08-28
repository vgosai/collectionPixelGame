using System.Collections;
using System.Collections.Generic;
using Codice.Client.Common.GameUI;
using UnityEditor;
using UnityEngine;

//This is an editor extension that we will use to select which generation function we want to use as well as let us do everything in the expector rather than inside the scene with a button
[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator generator;
    private void Awake()
    {
        generator = (AbstractDungeonGenerator)target; //target is the target of the custom inspector
    }

    public override void OnInspectorGUI() //this creates a button in the inspector
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }


}
