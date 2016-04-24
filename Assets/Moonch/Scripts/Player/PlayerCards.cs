using UnityEngine;

public class PlayerCards : MonoBehaviour
{
    internal void CollectCard()
    {
        collectedCards++;
    }

    private int collectedCards = 0;
}
