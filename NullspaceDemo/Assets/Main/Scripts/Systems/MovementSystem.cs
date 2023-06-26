using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct MovementSystem : ISystem
{
  public void OnCreate(ref SystemState state) { }
  public void OnDestroy(ref SystemState state) { }

  [BurstCompile]
  public void OnUpdate(ref SystemState state)
  {
    {
      foreach (var movementAspect in SystemAPI.Query<MovementAspect>())
      {
        movementAspect.ApplyPropulsion();
        movementAspect.ApplyHandling();
      }
    }
  }
}
