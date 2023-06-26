using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class HandlingAuthoring : MonoBehaviour
{
    [Header("Current, Limit, Change")]
    public float3 Yaw;
    [Header("Current, Limit, Change")]
    public float3 Pitch;
    [Header("Current, Limit, Change")]
    public float3 Roll;
}

public class HandlingBaker : Baker<HandlingAuthoring>
{
  public override void Bake(HandlingAuthoring authoring)
  {
    var entity = GetEntity(TransformUsageFlags.None);
    AddComponent(entity, new HandlingData
    {
      Yaw = authoring.Yaw,
      Pitch = authoring.Pitch,
      Roll = authoring.Roll
    });
  }
}