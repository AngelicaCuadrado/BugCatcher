using UnityEngine;

public class Net : MonoBehaviour
{
    public BugTracker bugtracker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bugtracker = FindFirstObjectByType<BugTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FriendlyBug")) 
        {
            Destroy(other.gameObject);
        }
        //Add objective points
    }
}
