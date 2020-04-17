using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
    public static EntityManager entityManager;

    // reference to the camera of the scene
    public Camera camera;

    public float minPositionValue = -50.0f;
    public float maxPositionValue = 50.0f;

    // prefabs references
    public GameObject virusPrefab;
    public GameObject redCellsPrefab;
    public GameObject bulletPrefab;

    public int virusCount = 500;
    public int redCellsCount = 500;
    public int bulletsCount = 10;

    BlobAssetStore store;

    private Entity bullet;

    // Start is called before the first frame update
    void Start()
    {
        store = new BlobAssetStore();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        
        Entity virus = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, settings);
        Entity redBlood = GameObjectConversionUtility.ConvertGameObjectHierarchy(redCellsPrefab, settings);
        bullet = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);

        // instantiate the viruses
        for (int i = 0; i < virusCount; i++)
        {
            Entity instance = entityManager.Instantiate(virus);
            float x = UnityEngine.Random.Range(minPositionValue, maxPositionValue);
            float y = UnityEngine.Random.Range(minPositionValue, maxPositionValue);
            float z = UnityEngine.Random.Range(minPositionValue, maxPositionValue);

            float3 position = new float3(x, y, z);
            entityManager.SetComponentData(instance, new Translation { Value = position });
            entityManager.SetComponentData(instance, new FloatData { speed = UnityEngine.Random.Range(0.1f, 1.0f) });
        }

        // instantiante the blood cells
        for (int i = 0; i < redCellsCount; i++)
        {
            Entity instance = entityManager.Instantiate(redBlood);
            float x = UnityEngine.Random.Range(minPositionValue, maxPositionValue);
            float y = UnityEngine.Random.Range(minPositionValue, maxPositionValue);
            float z = UnityEngine.Random.Range(minPositionValue, maxPositionValue);

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
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < bulletsCount; i++)
            {
                Entity instance = entityManager.Instantiate(bullet);

                // random initial bullet position
                Vector3 startPos = camera.transform.position + UnityEngine.Random.insideUnitSphere * 2.0f;

                entityManager.SetComponentData(instance, new Translation { Value = startPos });
                entityManager.SetComponentData(instance, new Rotation { Value = camera.transform.rotation });
            }
        }
    }

}
