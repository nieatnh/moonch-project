using UnityEngine;

public class MeteorDamage : MonoBehaviour
{

    public int damageAmount = 10;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerFuel = player.GetComponent<PlayerFuel>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Damage();
            GameObject.Destroy(this);
        }
    }

    void Damage()
    {
        if (playerFuel.currentFuel > 0)
        {
            playerFuel.TakeDamage(damageAmount);
        }
    }

    private GameObject player;
    private PlayerFuel playerFuel;
    private Animator anim;
    private float timer;
}
