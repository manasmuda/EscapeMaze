using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MazePreset", menuName = "Maze/Create Preset", order = 1)]
public class MazePreset : ScriptableObject {
    public GameObject prefabObject;
    public List<Material> sharedMaterials;
}
