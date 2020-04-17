using Unity.Entities;

[GenerateAuthoringComponent]
public struct LifetimeData : IComponentData
{
    public float lifeLeft;
}
