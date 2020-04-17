using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;

public class TimedDestroySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dt = Time.DeltaTime;

        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity,
                ref Translation position,
                ref LifetimeData lifetimeData) =>
            {
                lifetimeData.lifeLeft -= dt;
                if (lifetimeData.lifeLeft <= 0.0f)
                {
                    // destroy the bullet
                    EntityManager.DestroyEntity(entity);
                }
            })
            .Run();

        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity,
                ref Translation position,
                ref VirusData virusData) =>
            {
                if (!virusData.alive)
                {
                    // destroy the virus
                    EntityManager.DestroyEntity(entity);

                    // instantiate some white blood cells
                    for (int i = 0; i < 100; i++)
                    {
                        float3 offset = (float3)UnityEngine.Random.insideUnitSphere * 2.0f;
                        var splat = ECSManager.entityManager.Instantiate(ECSManager.whiteCell);
                        float3 randomDir = (float3)UnityEngine.Random.insideUnitSphere * 2.0f;

                        ECSManager.entityManager.SetComponentData(splat, new Translation { Value = position.Value + offset });
                        ECSManager.entityManager.SetComponentData(splat, new PhysicsVelocity { Linear = randomDir });
                    }
                }
            })
            .Run();

        return inputDeps;
    }
}
