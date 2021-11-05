using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game")]
public class GameConfig : ScriptableObject
{
    public int MaxOfflineRemindMinute = 720;
}