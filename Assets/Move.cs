using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float speedIncreaseRate = 0.5f; // unidades/s por segundo transcurrido

    // Velocidad actual global (la leen todos los Move activos — usamos el más reciente)
    public static float CurrentSpeed { get; private set; } = 10f;

    void Update()
    {
        float speed = baseSpeed + speedIncreaseRate * Time.timeSinceLevelLoad;
        CurrentSpeed = speed;
        transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destroy"))
        {
            Destroy(gameObject);
        }
    }
}
