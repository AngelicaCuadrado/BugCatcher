using UnityEngine;

public class Net : MonoBehaviour
{
    public BugTracker bugtracker;

    void Start()
    {
        // Auto-find if not assigned in Inspector
        if (bugtracker == null)
        {
            bugtracker = FindFirstObjectByType<BugTracker>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bugtracker == null) return;

        // Did we catch a Butterfly?
        Butterfly butterfly = other.GetComponent<Butterfly>();
        if (butterfly != null)
        {
            bugtracker.RegisterButterflyCaught();
            Destroy(butterfly.gameObject);
            return;
        }

        // Did we catch a Ladybug?
        Ladybug ladybug = other.GetComponent<Ladybug>();
        if (ladybug != null)
        {
            bugtracker.RegisterLadybugCaught();
            Destroy(ladybug.gameObject);
            return;
        }

        // If you still want a generic catch-all:
        // if (other.CompareTag("FriendlyBug"))
        // {
        //     Destroy(other.gameObject);
        // }
    }
}
