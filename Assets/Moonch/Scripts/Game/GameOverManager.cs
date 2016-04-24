using UnityEngine;

public class GameOverManager : MonoBehaviour {

    public PlayerFuel playerFuel;

    public float restartDelay = 5f;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerFuel.currentFuel <= 0)
        {
            restartTimer += Time.deltaTime;

            if (restartTimer >= restartDelay)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    private Animator anim;
    private float restartTimer;
}
