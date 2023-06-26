using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct PropulsionData : IComponentData
{
    public float3 Thrust;
    public float3 Vertical;
    public float3 Horizontal;
}
