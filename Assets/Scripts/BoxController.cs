using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : HCMonobehavior
{
    public MeshRenderer Mesh;
    public List<Material> Mats;

    public List<int> Indexes = new List<int>();

    public override void OnSpawned()
    {
        base.OnSpawned();
        Mesh.material = Mats[Indexes.PickRandom()];
    }
}