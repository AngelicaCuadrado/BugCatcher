using System.Collections;
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
    public ArrayList flockList = new ArrayList();

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
        for (int i = 0; i < flockSize; i++)
        {
            Butterfly butterfly = Instantiate(butterflyPrefab, transform.position, transform.rotation);
            butterfly.transform.parent = transform;
            butterfly.controller = this;
            flockList.Add(butterfly);
        }

        //States
        //Evade
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

        //Idle
        var idleState = fsm.CreateState("Idle");
        idleState.onEnter = delegate
        {
        };

        idleState.onStay = delegate
        {
            transform.position = Vector3.Lerp(transform.position, flockCenter, Time.deltaTime * 0.5f);
        };

        idleState.onExit = delegate
        {

        };

        //Transitions
        fsm.AddTransition("Idle", "Evade", () => Vector3.Distance(transform.position, player.transform.position) <= agroRange);
        fsm.AddTransition("Evade", "Idle", () => Vector3.Distance(transform.position, player.transform.position) >= agroRange);
    }

    void Update()
    {
        fsm.Update();

        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        foreach (Butterfly butterfly in flockList)
        {
            center += butterfly.transform.position;
            velocity += butterfly.GetComponent<Rigidbody>().linearVelocity;
        }
        flockCenter = center / flockSize;
        flockVelocity = velocity / flockSize;
    }
}
