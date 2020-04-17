using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class BulletCollisionEventSystem : JobComponentSystem
{
    BuildPhysicsWorld m_BuildPhysicsWorldSystem;
    StepPhysicsWorld m_StepPhysicsWorldSystem;

    protected override void OnCreate()
    {
        m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    struct CollisionEventImpulseJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<BulletData> bulletGroup;
        public ComponentDataFromEntity<VirusData> virusGroup;

        public void Execute(CollisionEvent collision)
        {
            Entity entityA = collision.Entities.EntityA,
                   entityB = collision.Entities.EntityB;

            bool isTargetA = virusGroup.Exists(entityA);
            bool isTargetB = virusGroup.Exists(entityB);
            
            bool isBulletA = bulletGroup.Exists(entityA);
            bool isBulletB = bulletGroup.Exists(entityB);

            if (isBulletA && isTargetB)
            {
                var aliveComponent = virusGroup[entityB];
                aliveComponent.alive = false;
                virusGroup[entityB] = aliveComponent;
            }

            if (isBulletB && isTargetA)
            {
                var aliveComponent = virusGroup[entityA];
                aliveComponent.alive = false;
                virusGroup[entityA] = aliveComponent;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle jobHandle = new CollisionEventImpulseJob
        {
            bulletGroup = GetComponentDataFromEntity<BulletData>(),
            virusGroup = GetComponentDataFromEntity<VirusData>()
        }
        .Schedule(m_StepPhysicsWorldSystem.Simulation, ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);

        jobHandle.Complete();

        return jobHandle;
    }

}
