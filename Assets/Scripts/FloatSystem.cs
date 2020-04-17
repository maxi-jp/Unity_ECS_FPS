using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;

public class FloatSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dt = Time.DeltaTime;
        
        JobHandle jobHandle = Entities
            .WithName("FloatSystem")
            .ForEach( (ref PhysicsVelocity physics,
                ref Translation position,
                ref Rotation rotation,
                ref FloatData floatData) =>
            {
                float s = math.sin((dt + position.Value.x) * 0.5f) * floatData.speed;
                float c = math.cos((dt + position.Value.y) * 0.5f) * floatData.speed;

                float3 dir = new float3(s, c, s);
                physics.Linear += dir;
            })
            .Schedule(inputDeps);

        jobHandle.Complete();

        return jobHandle;
    }
}
