using System.Collections;
using UnityEngine;

public class CameraFly : MonoBehaviour
{
    public float speed = 10.0f;
    public float sensitivity = 0.25f; // from 0 to 1
    public bool inverted = false;
    public bool gamePaused = false;
    public bool smooth = true;
    public float acceleration = 0.1f;
    private float originalSpeed;

    void Awake()
    {
        originalSpeed = speed;
    }

    void Update()
    {
        if (gamePaused)
            return;

        lastMouse = Input.mousePosition - lastMouse;
        if (!inverted)
        {
            lastMouse.y = -lastMouse.y;
        }

        lastMouse *= sensitivity;
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.y, transform.eulerAngles.y + lastMouse.x, 0);
        transform.eulerAngles = lastMouse;
        lastMouse = Input.mousePosition;

        // Movement of the camera
        Vector3 dir = new Vector3();

        // Always move forward
        dir.z += 1.0f;

        if (Input.GetKey(KeyCode.A)) dir.x -= 1.0f;
        if (Input.GetKey(KeyCode.D)) dir.x += 1.0f;
        dir.Normalize();

        if (dir != Vector3.zero)
        {
            // move
            if (actSpeed < 1)
                actSpeed += acceleration * Time.deltaTime * 40;
            else
                actSpeed = 1.0f;

            lastDir = dir;
        }
        else
        {
            // stop
            if (actSpeed > 0)
                actSpeed -= acceleration * Time.deltaTime * 20;
            else
                actSpeed = 0.0f;
        }

        //Debug.Log("SPEED: " + speed);
        if (smooth)
            transform.Translate(lastDir * actSpeed * speed * Time.deltaTime);
        else
            transform.Translate(dir * speed * Time.deltaTime);
    }

    internal void RunPowerUp(float powerAcceleration, float duration)
    {
        speed *= powerAcceleration;
        StartCoroutine(TerminatePowerUp(duration));
    }

    IEnumerator TerminatePowerUp(float time)
    {
        yield return new WaitForSeconds(time);
        speed = originalSpeed;
    }

    public void OnPauseGame()
    {
        Time.timeScale = 0;
        gamePaused = true;
        Object[] objects = FindObjectsOfType(typeof(MeteorSpawner));
        foreach (MeteorSpawner gameObject in objects)
        {
            gameObject.OnPauseGame();
        }
    }

    public void OnResumeGame()
    {
        Time.timeScale = 1;
        gamePaused = false;
        Object[] objects = FindObjectsOfType(typeof(MeteorSpawner));
        foreach (MeteorSpawner gameObject in objects)
        {
            gameObject.OnResumeGame();
        }
    }

    private float actSpeed = 0.0f; // from 0 to 1
    private Vector3 lastDir = new Vector3();
    private Vector3 lastMouse = new Vector3(255, 255, 255);
}
