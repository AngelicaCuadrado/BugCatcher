using UnityEngine;

public class Net : MonoBehaviour
{
    public BugTracker bugtracker;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip catchSound;

    void Start()
    {
        if (bugtracker == null)
        {
            bugtracker = FindFirstObjectByType<BugTracker>();
        }

        if (audioSource == null)
        {
            // Try to get one on this object, or from the parent (Player)
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = GetComponentInParent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bugtracker == null) return;

        bool caughtSomething = false;

        // Did we catch a Butterfly?
        Butterfly butterfly = other.GetComponent<Butterfly>();
        if (butterfly != null)
        {
            bugtracker.RegisterButterflyCaught();
            Destroy(butterfly.gameObject);
            caughtSomething = true;
        }

        // Did we catch a Ladybug?
        Ladybug ladybug = other.GetComponent<Ladybug>();
        if (!caughtSomething && ladybug != null)
        {
            bugtracker.RegisterLadybugCaught();
            Destroy(ladybug.gameObject);
            caughtSomething = true;
        }

        // Play Sound
        if (caughtSomething && audioSource != null && catchSound != null)
        {
            audioSource.PlayOneShot(catchSound);
        }
    }
}