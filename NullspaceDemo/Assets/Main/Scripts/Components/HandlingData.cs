using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct HandlingData : IComponentData
{
    public float3 Yaw;
    public float3 Pitch;
    public float3 Roll;
}
