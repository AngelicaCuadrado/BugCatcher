using System.Collections;
using UnityEngine;

public class BeeFlockController : MonoBehaviour
{
    public float minVelocity = 1;
    public float maxVelocity = 8;
    public int flockSize = 5;
    public float centerWeight = 1;
    public float velocityWeight = 1;
    public float separationWeight = 1;
    public float followWeight = 1;
    public float randomizeWeight = 1;
    public Bee beePrefab;
    public Transform target;
    public Vector3 flockCenter;
    internal Vector3 flockVelocity;
    public ArrayList flockList = new ArrayList();

    public SwarmFSM fsm;


    public GameObject player;
    public float agroRange = 2f;
    void Start()
    {
        fsm = new SwarmFSM();
        flockSize = (int)Random.Range(1, flockSize);
        player = FindFirstObjectByType<PlayerHealth>().gameObject;
        target = transform;
        for (int i = 0; i < flockSize; i++)
        {
            Bee bee = Instantiate(beePrefab, transform.position, transform.rotation) as Bee;
            bee.transform.parent = transform;
            bee.controller = this;
            flockList.Add(bee);
        }

        //States
        //Chase
        var chaseState = fsm.CreateState("Chase");
        chaseState.onEnter = delegate
        {
            target = player.transform;
        };

        chaseState.onStay = delegate
        {
            target = player.transform;
        };

        chaseState.onExit = delegate
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
        fsm.AddTransition("Idle", "Chase", () => Vector3.Distance(transform.position, player.transform.position) <= agroRange);
        fsm.AddTransition("Chase", "Idle", () => Vector3.Distance(transform.position, player.transform.position) >= agroRange);

    }

    void Update()
    {
        fsm.Update();
        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        foreach (Bee bee in flockList)
        {
            center += bee.transform.position;
            velocity += bee.GetComponent<Rigidbody>().linearVelocity;
        }
        flockCenter = center / flockSize;
        flockVelocity = velocity / flockSize;
    }
}
