using System.Collections.Generic;
using UnityEngine;

public class LadybugFlockController : MonoBehaviour
{
    public List<Ladybug> flockList = new List<Ladybug>();

    public void AddToFlock(Ladybug ladybug)
    {
        if (ladybug != null && !flockList.Contains(ladybug))
        {
            flockList.Add(ladybug);
        }
    }

    public void RemoveFromFlock(Ladybug ladybug)
    {
        if (ladybug != null)
        {
            flockList.Remove(ladybug);
        }
    }

    void Update()
    {
        // Just clean up nulls so any future logic here is safe
        for (int i = flockList.Count - 1; i >= 0; i--)
        {
            if (flockList[i] == null)
            {
                flockList.RemoveAt(i);
            }
        }

        // You can add ladybug flock behavior here later if you want
    }
}
