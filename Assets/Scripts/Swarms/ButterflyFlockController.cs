using System.Collections;
using UnityEngine;

public class ButterflyFlockController : MonoBehaviour
{
    public float minVelocity = 1;
    public float maxVelocity = 8;
    public int flockSize = 20;
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
    void Start()
    {
        for (int i = 0; i < flockSize; i++)
        {
            Butterfly butterfly = Instantiate(butterflyPrefab, transform.position, transform.rotation) as Butterfly;
            butterfly.transform.parent = transform;
            butterfly.controller = this;
            flockList.Add(butterfly);
        }
    }

    void Update()
    {
        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        foreach (Butterfly butterfly in flockList)
        {
            center += butterfly.transform.localPosition;
            velocity += butterfly.GetComponent<Rigidbody>().linearVelocity;
        }
        flockCenter = center / flockSize;
        flockVelocity = velocity / flockSize;
    }
}
