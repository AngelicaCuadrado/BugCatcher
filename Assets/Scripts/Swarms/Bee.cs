using UnityEngine;

public class Bee : MonoBehaviour
{
    internal BeeFlockController controller;
    private new Rigidbody rigidbody;

    [SerializeField]
    int damage;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        controller = (BeeFlockController)GameObject.FindGameObjectWithTag("BeeFlockController").GetComponent("BeeFlockController");
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
        }
    }
    private Vector3 Steer()
    {
        Vector3 center = controller.flockCenter - transform.localPosition;
        Vector3 velocity = controller.flockVelocity - rigidbody.linearVelocity;
        Vector3 follow = controller.target.localPosition - transform.localPosition;
        Vector3 separation = Vector3.zero;
        foreach (Bee bee in controller.flockList)
        {
            if (bee != this)
            {
                Vector3 relativePos = transform.localPosition - bee.transform.localPosition;
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

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Bee has entered");
        //damage player
        if (other.GetComponent<PlayerHealth>()) 
        {
            Debug.Log("Bee Dealing Damage");
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            player.TakeDamage(damage);
        }
    }
}
