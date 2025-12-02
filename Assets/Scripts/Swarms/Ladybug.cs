using UnityEngine;

public class Ladybug : MonoBehaviour
{
    internal LadybugFlockController controller;
    private new Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        controller = (LadybugFlockController)GameObject.FindGameObjectWithTag("LadybugFlockController").GetComponent("LadybugFlockController");

        if (controller == null)
        {
            GameObject controllerObj = GameObject.FindGameObjectWithTag("LadybugFlockController");
            if (controllerObj != null)
            {
                controller = controllerObj.GetComponent<LadybugFlockController>();
            }
        }

        if (controller != null)
        {
            controller.AddToFlock(this);
        }
    }
    void Update()
    {
        if (controller)
        {
            Vector3 relativePos = Steer() * Time.deltaTime;
            if (relativePos != Vector3.zero)
                rigidbody.linearVelocity = relativePos;
            float speed = rigidbody.linearVelocity.magnitude;
            if (speed > controller.maxVelocity)
            {
                rigidbody.linearVelocity = rigidbody.linearVelocity.normalized *
                controller.maxVelocity;
            }
            else if (speed < controller.minVelocity)
            {
                rigidbody.linearVelocity = rigidbody.linearVelocity.normalized *
                controller.minVelocity;
            }

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
    }
    private Vector3 Steer()
    {
        Vector3 center = controller.flockCenter - transform.position;
        Vector3 velocity = controller.flockVelocity - rigidbody.linearVelocity;
        Vector3 follow = controller.target.position - transform.position;
        Vector3 separation = Vector3.zero;
        foreach (Ladybug ladybug in controller.flockList)
        {
            if (ladybug != this)
            {
                Vector3 relativePos = transform.position - ladybug.transform.position;
                separation += relativePos.normalized;
            }
        }
        Vector3 randomize = new Vector3(Random.value * 2 - 1, Random.value * 2 - 1, Random.value * 2 - 1);
        randomize.Normalize();
        return (
        controller.centerWeight * center
        + controller.velocityWeight * velocity
        + controller.separationWeight * separation
        + controller.followWeight * follow
        + controller.randomizeWeight * randomize
        );
    }


    private void OnDestroy()
    {
        if (controller != null)
        {
            controller.RemoveFromFlock(this);
        }
    }
}
