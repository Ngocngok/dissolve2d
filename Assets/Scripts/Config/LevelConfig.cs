using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Configs/Levels")]
public class LevelConfig : ScriptableObject
{
    public List<int> ListHeight;
    public List<float> ListSpeed;
}