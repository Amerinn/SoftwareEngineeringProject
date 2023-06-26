using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerControlledTagAuthoring : MonoBehaviour
{

}

public class PlayerControlledTagBaker : Baker<PlayerControlledTagAuthoring>
{
    public override void Bake(PlayerControlledTagAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new PlayerControlledTag{ });
    }
}