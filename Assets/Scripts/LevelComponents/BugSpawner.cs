using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    [Header("Swarm Prefabs")]
    public GameObject butterflyPrefab;
    public GameObject ladybugPrefab;
    public GameObject beePrefab;

    [Header("Spawn Settings")]
    public int swarmCount = 20;  
    public float spawnRadius = 30f; 
    public float minSpacing = 6f;  
    public float fixedY = 0f;       

    private void Start()
    {
        SpawnSwarm(butterflyPrefab, swarmCount);
        SpawnSwarm(ladybugPrefab, swarmCount);
        SpawnSwarm(beePrefab, swarmCount);
    }

    private void SpawnSwarm(GameObject prefab, int count)
    {
        int spawned = 0;
        int safetyCounter = 0;

        while (spawned < count && safetyCounter < count * 10)
        {
            safetyCounter++;

            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 candidatePos = new Vector3(randomCircle.x, fixedY, randomCircle.y) + transform.position;

            bool tooClose = false;
            foreach (Transform child in transform)
            {
                if (Vector3.Distance(child.position, candidatePos) < minSpacing)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                Instantiate(prefab, candidatePos, Quaternion.identity, transform);
                spawned++;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        Vector3 up = Vector3.up;
        UnityEditor.Handles.color = new Color(0f, 1f, 0f, 0.2f);
        UnityEditor.Handles.DrawSolidDisc(transform.position, up, spawnRadius);
    }
}
