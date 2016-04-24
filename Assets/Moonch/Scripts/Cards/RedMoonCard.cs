using UnityEngine;
using System.Collections;

public class RedMoonCard : MonoBehaviour {

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraFly = player.GetComponentInParent<CameraFly>();
        playerFuel = player.GetComponent<PlayerFuel>();
        playerCards = player.GetComponent<PlayerCards>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerCards.CollectCard();
            playerFuel.RefillFuel(fuelIncrement);

            // Destroy the card from the game
            GameObject.Destroy(transform.parent.gameObject);
            cameraFly.RunPowerUp(5f, 10f);
        }
    }

    private GameObject player;
    private CameraFly cameraFly;
    private PlayerFuel playerFuel;
    private PlayerCards playerCards;
    private int fuelIncrement = 10;
}
