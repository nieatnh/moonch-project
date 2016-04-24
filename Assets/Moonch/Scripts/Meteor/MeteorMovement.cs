using UnityEngine;

public class MeteorMovement : MonoBehaviour
{

    public int maxMeteors = 1000;
    public float starSize = 0.2f;
    public float distance = 100;
    public float clipDistance = 5;
    public bool gamePaused = false;

    void Start()
    {
        transformation = transform;
        distanceSqr = distance * distance;
        clipDistanceSqr = clipDistance * clipDistance;
    }

    void Update()
    {
        if (gamePaused)
            return;

        transformation.Rotate(0.0f, 0.0f, 1 + transformation.rotation.z);

        transformation.position = new Vector3(transformation.position.x, transformation.position.y, transformation.position.z + 0.02f);
    }

    public void OnPauseGame()
    {
        Time.timeScale = 0;
        gamePaused = true;
    }

    public void OnResumeGame()
    {
        Time.timeScale = 1;
        gamePaused = false;
    }

    private Transform transformation;
    private float distanceSqr;
    private float clipDistanceSqr;
}
