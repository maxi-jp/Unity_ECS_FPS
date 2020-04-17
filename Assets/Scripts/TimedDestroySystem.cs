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

        return inputDeps;
    }
}
