using UnityEngine;

public class Butterfly : MonoBehaviour
{
    internal ButterflyFlockController controller;
    private new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        // Find controller by tag if not already assigned
        if (controller == null)
        {
            GameObject controllerObj = GameObject.FindGameObjectWithTag("ButterflyFlockController");
            if (controllerObj != null)
            {
                controller = controllerObj.GetComponent<ButterflyFlockController>();
            }
        }

        // Make sure this butterfly is registered in the flock list
        if (controller != null)
        {
            controller.AddToFlock(this);
        }
    }

    void Update()
    {
        if (controller == null)
            return;

        Vector3 relativePos = Steer() * Time.deltaTime;
        if (relativePos != Vector3.zero)
            rigidbody.linearVelocity = relativePos;

        float speed = rigidbody.linearVelocity.magnitude;
        if (speed > controller.maxVelocity)
        {
            rigidbody.linearVelocity = rigidbody.linearVelocity.normalized * controller.maxVelocity;
        }
        else if (speed < controller.minVelocity)
        {
            rigidbody.linearVelocity = rigidbody.linearVelocity.normalized * controller.minVelocity;
        }

        // Clamp height (same as your original logic)
        if (transform.position.y < 0f)
        {
            Vector3 pos = transform.position;
            pos.y = 0f;
            transform.position = pos;

            if (rigidbody.linearVelocity.y < 0f)
                rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, 0f, rigidbody.linearVelocity.z);
        }

        if (transform.position.y > 2f)
        {
            Vector3 pos = transform.position;
            pos.y = 2f;
            transform.position = pos;

            if (rigidbody.linearVelocity.y > 0f)
                rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, 0f, rigidbody.linearVelocity.z);
        }
    }

    private Vector3 Steer()
    {
        if (controller == null)
            return Vector3.zero;

        Vector3 center = controller.flockCenter - transform.position;
        Vector3 velocity = controller.flockVelocity - rigidbody.linearVelocity;
        Vector3 follow = controller.target.position - transform.position;
        Vector3 separation = Vector3.zero;

        // IMPORTANT: skip null entries and self so destroyed butterflies don't cause errors
        foreach (Butterfly butterfly in controller.flockList)
        {
            if (butterfly == null || butterfly == this)
                continue;

            Vector3 relativePos = transform.position - butterfly.transform.position;
            separation += relativePos.normalized;
        }

        Vector3 randomize = new Vector3(Random.value * 2 - 1, Random.value * 2 - 1, Random.value * 2 - 1);
        randomize.Normalize();

        return
            controller.centerWeight * center +
            controller.velocityWeight * velocity +
            controller.separationWeight * separation +
            controller.followWeight * follow +
            controller.randomizeWeight * randomize;
    }

    private void OnDestroy()
    {
        // When destroyed by the net, remove from flock to avoid MissingReferenceException
        if (controller != null)
        {
            controller.RemoveFromFlock(this);
        }
    }
}
