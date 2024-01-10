using UnityEngine;

public class MobManager : MonoBehaviour
{

    public GameObject[] Mobs;
    public GameObject MobGrid;

    public float MinSpawnTime;
    public float MaxSpawnTime;
    public float NextSpawnDelay;

    public int MinSpawnAmount;
    public int MaxSpawnAmount;
    public int NextSpawnAmount;

    public MobType NextSpawnType;

    public float SpawnRange;

    public int SpawnedNumber;
    public int SpawnBuffer;
    public float NextBufferClean;

    private void Start()
    {
        NextSpawnAmount = Random.Range(MinSpawnAmount, MaxSpawnAmount);
        NextSpawnDelay = Time.time + Random.Range(MinSpawnTime, MaxSpawnTime);
        NextBufferClean = Time.time + 10.0f;
    }

    private void FixedUpdate() {

        var time = Time.time;

        if (time >= NextBufferClean)
        {
            SpawnedNumber /= 2;
        }
        
        if (time >= NextSpawnDelay && SpawnBuffer > SpawnedNumber) {

            Vector3 currentPosition = gameObject.transform.position;
            for (int i = NextSpawnAmount; i > 0; i--)
            {
                SpawnedNumber++;
                SpawnMobAtPosition(new Vector3(currentPosition.x + Random.Range(-SpawnRange, SpawnRange), currentPosition.y + Random.Range(-SpawnRange, SpawnRange), 0), Mobs[Random.Range(0, Mobs.Length)]);
            }

            NextSpawnAmount = Random.Range(MinSpawnAmount, MaxSpawnAmount);
            SpawnedNumber -= NextSpawnAmount;
            NextSpawnDelay = Time.time + Random.Range(MinSpawnTime, MaxSpawnTime);
            NextBufferClean = Time.time + MaxSpawnTime * 3f;

        }

    }

    public void SpawnMobAtPosition(Vector3 position, GameObject mob) {

        mob = Instantiate(mob, position, Quaternion.identity);
        mob.transform.SetParent(MobGrid.transform);
        mob.transform.localPosition = position;
        mob.SetActive(true);

    }

}
