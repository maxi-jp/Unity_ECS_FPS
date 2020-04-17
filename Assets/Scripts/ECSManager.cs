using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
    public static EntityManager entityManager;

    // prefabs references
    public GameObject virusPrefab;

    int virusCount = 500;

    BlobAssetStore store;

    // Start is called before the first frame update
    void Start()
    {
        store = new BlobAssetStore();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        
        Entity virus = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, settings);

        for (int i = 0; i < virusCount; i++)
        {
            Entity instance = entityManager.Instantiate(virus);
            float x = UnityEngine.Random.Range(-50.0f, 50.0f);
            float y = UnityEngine.Random.Range(-50.0f, 50.0f);
            float z = UnityEngine.Random.Range(-50.0f, 50.0f);

            float3 position = new float3(x, y, z);
            entityManager.SetComponentData(instance, new Translation { Value = position });
            entityManager.SetComponentData(instance, new FloatData { speed = UnityEngine.Random.Range(0.1f, 1.0f) });
        }
    }

    void OnDestroy()
    {
        store.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
