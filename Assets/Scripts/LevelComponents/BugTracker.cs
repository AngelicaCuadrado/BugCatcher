using UnityEngine;

public class BugTracker : MonoBehaviour
{
    public int butterflyRequirement;
    public int currentButterflies;
    public int ladybugRequirment;
    public int currentLadybugs;

    void Start()
    {
        // Optional: reset counts at start
        currentButterflies = 0;
        currentLadybugs = 0;
    }

    public void RegisterButterflyCaught()
    {
        currentButterflies++;
        Debug.Log($"[BugTracker] Butterflies caught: {currentButterflies}/{butterflyRequirement}");
        // TODO: check for win condition here if you want
        // if (currentButterflies >= butterflyRequirement && currentLadybugs >= ladybugRequirment) { ... }
    }

    public void RegisterLadybugCaught()
    {
        currentLadybugs++;
        Debug.Log($"[BugTracker] Ladybugs caught: {currentLadybugs}/{ladybugRequirment}");
        // TODO: same as above if needed
    }
}
