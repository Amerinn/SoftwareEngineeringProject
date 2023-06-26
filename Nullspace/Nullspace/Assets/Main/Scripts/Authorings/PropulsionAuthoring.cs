using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PropulsionAuthoring : MonoBehaviour
{
    [Header("Current, Limit, Change")]
    public float3 Thrust;
    [Header("Current, Limit, Change")]
    public float3 Vertical;
    [Header("Current, Limit, Change")]
    public float3 Horizontal;
}

public class PropulsionBaker : Baker<PropulsionAuthoring>
{
  public override void Bake(PropulsionAuthoring authoring)
  {
    var entity = GetEntity(TransformUsageFlags.None);
    AddComponent(entity, new PropulsionData
    {
      Thrust = authoring.Thrust,
      Vertical = authoring.Vertical,
      Horizontal = authoring.Horizontal
    });
  }
}