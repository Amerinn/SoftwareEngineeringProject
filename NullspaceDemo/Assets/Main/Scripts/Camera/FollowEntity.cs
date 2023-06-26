using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{
    private Entity target;
    void LateUpdate()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        if(target != Entity.Null)
        {
            transform.position = entityManager.GetComponentData<LocalTransform>(target).Position;
            transform.rotation = entityManager.GetComponentData<LocalTransform>(target).Rotation;
        }
        else
            target = getTarget();
    }

    private Entity getTarget()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityQuery query = entityManager.CreateEntityQuery(typeof(PlayerControlledTag));
        NativeArray<Entity> array = query.ToEntityArray(Allocator.Temp);
        if (array.Length > 0)
            return array[0];
        else
            return Entity.Null;
    }
}
