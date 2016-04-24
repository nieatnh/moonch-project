using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public int maxMeteors = 1000;
    public float starSize = 0.2f;
    public float distance = 100;
    public float clipDistance = 5;
    public GameObject meteor;

    void Start()
    {
        transformation = transform;
        distanceSqr = distance * distance;
        clipDistanceSqr = clipDistance * clipDistance;
    }

    void Awake()
    {
        cameraFly = GetComponentInParent<CameraFly>();
    }

    void Update()
    {
        if (points == null)
        {
            CreateMeteors();
        }

        for (int i = 0; i < points.Length; i++)
        {
            if ((points[i].position - transformation.position).sqrMagnitude > distanceSqr)
            {
                points[i].position = Random.insideUnitSphere.normalized * distance + transformation.position;
                meteors[i].transform.position = Random.insideUnitSphere.normalized * distance + transformation.position;
            }

            if ((points[i].position - transformation.position).sqrMagnitude <= clipDistanceSqr)
            {
                float percent = (points[i].position - transformation.position).sqrMagnitude / clipDistanceSqr;
                points[i].startColor = new Color(1, 1, 1, percent);
                points[i].startSize = percent * starSize;
            }
        }

        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }

    private void CreateMeteors()
    {
        points = new ParticleSystem.Particle[maxMeteors];
        meteors = new GameObject[maxMeteors];

        for (int i = 0; i < maxMeteors; i++)
        {
            points[i].position = Random.insideUnitSphere * distance + transformation.position;
            points[i].startColor = new Color(1, 1, 1, 1);
            points[i].startSize = starSize;
            meteors[i] = Instantiate(meteor, points[i].position, meteor.transform.rotation) as GameObject;
        }
    }

    public void OnPauseGame()
    {
        Time.timeScale = 0;        

        // Pause all meteor movement too
        foreach (var meteorGameObject in meteors)
        {
            meteorGameObject.GetComponent<MeteorMovement>().OnPauseGame();
        }
    }

    public void OnResumeGame()
    {
        Time.timeScale = 1;

        // Resume all meteor movement too
        foreach (var meteorGameObject in meteors)
        {
            meteorGameObject.GetComponent<MeteorMovement>().OnResumeGame();
        }
    }

    private Transform transformation;
    private ParticleSystem.Particle[] points;
    private GameObject[] meteors;
    private CameraFly cameraFly;
    private float distanceSqr;
    private float clipDistanceSqr;
}
