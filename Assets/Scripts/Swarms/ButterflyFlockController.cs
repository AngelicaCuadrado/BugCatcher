using System.Collections.Generic;
using UnityEngine;

public class ButterflyFlockController : MonoBehaviour
{
    public float minVelocity = 1;
    public float maxVelocity = 8;
    public int flockSize = 5;
    public float centerWeight = 1;
    public float velocityWeight = 1;
    public float separationWeight = 1;
    public float followWeight = 1;
    public float randomizeWeight = 1;
    public Butterfly butterflyPrefab;
    public Transform target;
    public Vector3 flockCenter;
    internal Vector3 flockVelocity;

    // Changed from ArrayList to a typed list so we can clean up nulls safely
    public List<Butterfly> flockList = new List<Butterfly>();

    public SwarmFSM fsm;

    private Transform evadeTarget;

    public GameObject player;
    public float agroRange = 2f;

    void Start()
    {
        evadeTarget = new GameObject("EvadeTarget").transform;
        evadeTarget.parent = transform;
        evadeTarget.position = transform.position;

        fsm = new SwarmFSM();
        flockSize = Random.Range(1, flockSize + 1);
        player = FindFirstObjectByType<PlayerHealth>().gameObject;
        target = transform;

        // Spawn butterflies and register them in the flock list
        for (int i = 0; i < flockSize; i++)
        {
            Butterfly butterfly = Instantiate(butterflyPrefab, transform.position, transform.rotation);
            butterfly.transform.parent = transform;
            butterfly.controller = this;
            AddToFlock(butterfly);
        }

        // Evade
        var evadeState = fsm.CreateState("Evade");
        evadeState.onEnter = delegate
        {
            Vector3 dir = transform.position - player.transform.position;
            dir.y = 0f;
            dir = dir.normalized;

            evadeTarget.position = transform.position + dir * agroRange * 2f;
            evadeTarget.position = new Vector3(evadeTarget.position.x, transform.position.y, evadeTarget.position.z);

            target = evadeTarget;
        };

        evadeState.onStay = delegate
        {
            Vector3 dir = transform.position - player.transform.position;
            dir.y = 0f;
            dir = dir.normalized;

            evadeTarget.position = transform.position + dir * agroRange * 2f;
            evadeTarget.position = new Vector3(evadeTarget.position.x, transform.position.y, evadeTarget.position.z);

            target = evadeTarget;
        };

        evadeState.onExit = delegate
        {
            target = transform;
        };

        // Idle
        var idleState = fsm.CreateState("Idle");
        idleState.onEnter = delegate { };

        idleState.onStay = delegate
        {
            transform.position = Vector3.Lerp(transform.position, flockCenter, Time.deltaTime * 0.5f);
        };

        idleState.onExit = delegate { };

        // Transitions
        fsm.AddTransition("Idle", "Evade", () => Vector3.Distance(transform.position, player.transform.position) <= agroRange);
        fsm.AddTransition("Evade", "Idle", () => Vector3.Distance(transform.position, player.transform.position) >= agroRange);
    }

    public void AddToFlock(Butterfly butterfly)
    {
        if (butterfly != null && !flockList.Contains(butterfly))
        {
            flockList.Add(butterfly);
        }
    }

    public void RemoveFromFlock(Butterfly butterfly)
    {
        if (butterfly != null)
        {
            flockList.Remove(butterfly);
        }
    }

    void Update()
    {
        fsm.Update();

        // Clean nulls + compute center & velocity only from alive butterflies
        if (flockList.Count == 0)
        {
            flockCenter = transform.position;
            flockVelocity = Vector3.zero;
            return;
        }

        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        int aliveCount = 0;

        for (int i = flockList.Count - 1; i >= 0; i--)
        {
            Butterfly butterfly = flockList[i];

            if (butterfly == null)
            {
                flockList.RemoveAt(i);
                continue;
            }

            center += butterfly.transform.position;

            Rigidbody rb = butterfly.GetComponent<Rigidbody>();
            if (rb != null)
            {
                velocity += rb.linearVelocity;
            }

            aliveCount++;
        }

        if (aliveCount > 0)
        {
            flockCenter = center / aliveCount;
            flockVelocity = velocity / Mathf.Max(1, aliveCount);
        }
        else
        {
            flockCenter = transform.position;
            flockVelocity = Vector3.zero;
        }
    }
}
