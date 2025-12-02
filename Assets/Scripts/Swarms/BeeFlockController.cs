using System.Collections;
using UnityEngine;

public class BeeFlockController : MonoBehaviour
{
    public float minVelocity = 1;
    public float maxVelocity = 8;
    public int flockSize = 20;
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
    void Start()
    {
        for (int i = 0; i < flockSize; i++)
        {
            Bee bee = Instantiate(beePrefab, transform.position, transform.rotation) as Bee;
            bee.transform.parent = transform;
            bee.controller = this;
            flockList.Add(bee);
        }
    }

    void Update()
    {
        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        foreach (Bee bee in flockList)
        {
            center += bee.transform.localPosition;
            velocity += bee.GetComponent<Rigidbody>().linearVelocity;
        }
        flockCenter = center / flockSize;
        flockVelocity = velocity / flockSize;
    }
}
