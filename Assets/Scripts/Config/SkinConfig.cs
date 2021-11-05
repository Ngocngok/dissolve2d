using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "Configs/Skins")]
public class SkinConfig : ScriptableObject
{
    public SKINS ID;
    public Material BodyMat;
    public string Name;

    public int UnlockLevel;
    public int GoldCost;
    public int AdsCost;
}

public enum SKINS
{
    NONE = -1,
    DEFAULT = 0,
    FASTFOOD,
    FIRE_FIGHTER,
    GAMER_GIRL,
    GANGSTER,
    GRANDMA,
    GRANDPA,
    HIPSTER_GIRL,
    HIPSTER_GUY,
    HOBO,
    HOTDOG,
    JOCK,
    PANAMEDIC,
    PUNK_GIRL,
    PUNK_GUY,
    ROAD_WORKER,
    SHOP_KEEPER,
    SUMMER_GIRL,
    TOURIST,
    TOTAL
}