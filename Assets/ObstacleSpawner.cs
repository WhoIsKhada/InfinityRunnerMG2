using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject Obstacle;

    private float[] lanePositions = { -2.5f, 0f, 2.5f };

    [SerializeField] private float spawnOffsetZ = -5f;
    [SerializeField] private float spawnOffsetY = 0.5f;

    void Start()
    {
        int[] lanes = { 0, 1, 2 };
        Shuffle(lanes);

        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPos = new Vector3(
                lanePositions[lanes[i]],
                transform.position.y + spawnOffsetY,
                transform.position.z + spawnOffsetZ
            );

            GameObject obs = Instantiate(Obstacle, spawnPos, Quaternion.identity);

            // Forzar Is Trigger en el collider para que OnTriggerEnter funcione
            // Esto tambien evita que la fisica empuje al jugador verticalmente
            Collider col = obs.GetComponent<Collider>();
            if (col != null) col.isTrigger = true;

            // Rigidbody cinematico: necesario para que Unity genere eventos de trigger
            Rigidbody rb = obs.GetComponent<Rigidbody>();
            if (rb == null) rb = obs.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;

            // Mover con el terreno; Move.cs destruye el objeto al tocar el DestroyTrigger
            obs.AddComponent<Move>();
        }
    }

    void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int tmp = array[i];
            int j = Random.Range(0, i + 1);
            array[i] = array[j];
            array[j] = tmp;
        }
    }
}