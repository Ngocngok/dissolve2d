using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

public static class PoolHelper
{
    public static Transform Spawn(this GameObject obj, POOL pool)
    {
        string poolName = pool.ToString();
        if (pool == POOL.NONE)
            poolName = obj.name;
        return PoolManager.Pools[poolName].Spawn(obj, Vector3.zero, Quaternion.identity);
    }

    public static Transform Spawn(this GameObject obj, Transform parent, POOL pool)
    {
        string poolName = pool.ToString();
        if (pool == POOL.NONE)
            poolName = obj.name;
        return PoolManager.Pools[poolName].Spawn(obj, Vector3.zero, Quaternion.identity, parent);
    }

    public static Transform Spawn(this GameObject prefab, Vector3 pos, Quaternion rot, POOL pool)
    {
        string poolName = pool.ToString();
        if (pool == POOL.NONE)
            poolName = prefab.name;
        return PoolManager.Pools[poolName].Spawn(prefab, pos, rot);
    }

    public static Transform Spawn(this GameObject prefab, Vector3 pos, Quaternion rot, Transform parent, POOL pool)
    {
        string poolName = pool.ToString();
        if (pool == POOL.NONE)
            poolName = prefab.name;
        return PoolManager.Pools[poolName].Spawn(prefab, pos, rot, parent);
    }

    public static void Despawn(this GameObject prefab, POOL pool)
    {
        string poolName = pool.ToString();
        if (pool == POOL.NONE)
        {
            poolName = prefab.name;
            if (poolName.IndexOf('(') > 0)
            {
                poolName = prefab.name.Substring(0, poolName.IndexOf('('));
            }
        }

        PoolManager.Pools[poolName].Despawn(prefab.transform);
    }
}