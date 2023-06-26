using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public readonly partial struct MovementAspect: IAspect
{
    public readonly RefRW<PropulsionData> propulsionData;
    public readonly RefRW<HandlingData> handlingData;
    readonly RefRW<PhysicsVelocity> velocity;

    [BurstCompile]
    public void ApplyPropulsion()
    {
        var thrust = propulsionData.ValueRO.Thrust[0];
        var horizontal = propulsionData.ValueRO.Horizontal[0];
        var vertical = propulsionData.ValueRO.Vertical[0];
        velocity.ValueRW.Linear = new float3(horizontal, vertical, thrust);
    }

    [BurstCompile]
    public void ApplyHandling()
    {
        var roll = handlingData.ValueRO.Roll[0];
        var pitch = handlingData.ValueRO.Pitch[0];
        var yaw = handlingData.ValueRO.Yaw[0];
        velocity.ValueRW.Angular = new float3(pitch, yaw, roll);

    }


    /* readonly RefRW<PropulsionData> data;
    //readonly RefRW<LocalTransform> transform;
    readonly RefRW<PhysicsVelocity> velocity; */

    /*
    public void ApplyPropulsion()
    {
         if (data.ValueRO.Current < data.ValueRO.Max)
            data.ValueRW.Current += data.ValueRO.Acceleration;
        // transform.ValueRW.Position += new float3(0, 0, data.ValueRO.Current);
        velocity.ValueRW.Linear += new float3(0, 0, data.ValueRO.Current);
    }*/

}
