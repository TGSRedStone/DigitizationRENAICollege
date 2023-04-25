using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North,
    South
}

[System.Serializable]
public struct BuildingSetting
{
    public string BuildingName;
    public Vector3 BuildingPos;
    public Direction Direction;
    public float CameraDistance;
}

namespace Config
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "ScriptObjects/Configs/BuildingConfig")]
    public class BuildingConfig : ScriptableObject
    {
        public List<BuildingSetting> buildingSettings;
    }
}