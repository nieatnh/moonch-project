using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject moonFacts;
    public int cardIndex;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraFly = player.GetComponentInParent<CameraFly>();
        playerFuel = player.GetComponent<PlayerFuel>();
        playerCards = player.GetComponent<PlayerCards>();
    }

    void OnTriggerEnter(Collider other)
    {
        // If the entering collider is the player...
        if (other.gameObject == player)
        {
            moonFacts.SetActive(true);
            moonFacts.GetComponent<MoonFacts>().selectedIndex = cardIndex;

            playerCards.CollectCard();
            playerFuel.RefillFuel(fuelIncrement);

            // Destroy the card from the game
            GameObject.Destroy(transform.parent.gameObject);
            cameraFly.OnPauseGame();
        }
    }

    public void ResumeGame()
    {
        cameraFly.OnResumeGame();
        moonFacts.SetActive(false);
    }

    private GameObject player;
    private CameraFly cameraFly;
    private PlayerFuel playerFuel;
    private PlayerCards playerCards;
    private int fuelIncrement = 10;
}
