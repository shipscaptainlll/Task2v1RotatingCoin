using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AreaSpawner : MonoBehaviour
{
    [SerializeField] Collider exampleMesh;
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] Transform crystalsHolder;

    //transfered to pool objects
    [SerializeField] CoinStateManager coinStateManager;
    [SerializeField] CoinXpController coinXpController;
    [SerializeField] Transform lookAt;
    [SerializeField] SoundManager soundManager;

    [SerializeField] int spawnRate;

    public List<CrystalBehavior> activeCrystals = new List<CrystalBehavior>();
    public int SpawnRate { get { return spawnRate; } set { spawnRate = value; Debug.Log("current spawn rate is " + spawnRate); } }
    private ObjectPool<GameObject> crystalsPool;
    Coroutine ContinuousSpawnerCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        crystalsPool = new ObjectPool<GameObject>(() => {
            GameObject newCrystalHolder = Instantiate(crystalPrefab, crystalsHolder);
            Transform crystal = newCrystalHolder.transform.Find("Crystall");
            crystal.GetComponent<CrystalBehavior>().RandomizeValues();
            crystal.GetComponent<CrystalBehavior>().coinStateManager = coinStateManager;
            crystal.GetComponent<CrystalBehavior>().areaSpawner = this;
            crystal.GetComponent<CrystalBehavior>().coinXpController = coinXpController;
            crystal.GetComponent<CrystalBehavior>().SoundManager = soundManager;
            crystal.GetComponent<CrystalHealthController>().lookAtPoint = lookAt;     

            return newCrystalHolder;
        }, crystal =>
        {
            crystal.gameObject.SetActive(true);
            activeCrystals.Add(crystal.transform.Find("Crystall").GetComponent<CrystalBehavior>());
            crystal.transform.Find("Crystall").GetComponent<CrystalBehavior>().RandomizeValues();
            crystal.transform.Find("Crystall").GetComponent<CrystalHealthController>().RefillHP();
        }, crystal =>
        {
            activeCrystals.Remove(crystal.transform.Find("Crystall").GetComponent<CrystalBehavior>());
            crystal.gameObject.SetActive(false);
            Debug.Log("gained 2 xp");
        }, crystal =>
        {
            Destroy(crystal.gameObject);
        }, false, 30, 40);

        InitiateSpawning();
    }



    public void InitiateSpawning()
    {
        ContinuousSpawnerCoroutine = StartCoroutine(ContinuousSpawner());
    }

    public void StopSpawning()
    {
        if (ContinuousSpawnerCoroutine != null)
        {
            StopCoroutine(ContinuousSpawnerCoroutine);
            ContinuousSpawnerCoroutine = null;
        }
    }

    IEnumerator ContinuousSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(60 / spawnRate);
            if (crystalsPool.CountActive < 30)
            {
                SpawnInsideArea(exampleMesh);
            }
            yield return null;
        }
    }

    public void SpawnInsideArea(Collider area)
    {
        Vector3 min = area.bounds.min;
        Vector3 max = area.bounds.max;
        Vector3 point = new Vector3();
        bool gotHit = false;
        RaycastHit hit;

        for (int i = 0; i < 100; i++)
        {
            point.x = Random.Range(min.x, max.x);
            point.y = Random.Range(min.y, max.y);
            point.z = Random.Range(min.z, max.z);

            Ray ray = new Ray(point + new Vector3(0, 3, 0), Vector3.down);
            hit = new RaycastHit();
            gotHit = Physics.Raycast(ray, out hit, 200.0f);
            

            if (gotHit && hit.collider == exampleMesh)
            {
                //GameObject choosenPrefab = GetRandomPrefab();

                //GameObject newInstance = Instantiate(choosenPrefab, point, choosenPrefab.transform.rotation, spawnParent);
                GameObject newInstance = crystalsPool.Get();
                newInstance.transform.position = new Vector3(point.x, 0.135f, point.z);
                newInstance.transform.Find("Crystall").Rotate(new Vector3(0, 0, Random.Range(0, 180)));
                newInstance.transform.Find("Crystall").GetComponent<CrystalHealthController>().VisualizeOreHealth();
                return;
            } 
        }
    }

    public void DestroyCrystal(GameObject crystal)
    {
        crystalsPool.Release(crystal);
    }

}
