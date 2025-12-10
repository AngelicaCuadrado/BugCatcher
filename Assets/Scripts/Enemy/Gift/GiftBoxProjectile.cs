using UnityEngine;
using UnityEngine.AI;

public class GiftBoxProjectile : MonoBehaviour
{
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private GameObject tinySpiderPrefab;
    [SerializeField] private float destroyDelay = 0.05f;

    [Header("Tiny Spider Scale")]
    [SerializeField] private Vector3 tinySpiderScale = new Vector3(0.5f, 0.5f, 0.5f);

    bool hasTriggered = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasTriggered) return;
        hasTriggered = true;

        Destroy(gameObject, destroyDelay);
    }

    void OnDestroy()
    {
        if (!Application.isPlaying) return;

        Vector3 spawnPos = transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPos, out hit, 2f, NavMesh.AllAreas))
            spawnPos = hit.position;

        // Explosion VFX – no scaling, use prefab as-is
        if (explosionVFX != null)
        {
            var fx = Instantiate(explosionVFX, spawnPos, Quaternion.identity);
            Destroy(fx, 2f);
        }

        // Tiny spider
        if (tinySpiderPrefab != null)
        {
            var tiny = Instantiate(tinySpiderPrefab, spawnPos, Quaternion.identity);
            tiny.transform.localScale = tinySpiderScale;

            var agent = tiny.GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.Warp(spawnPos);
        }
    }
}
