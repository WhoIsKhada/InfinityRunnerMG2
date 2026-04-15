using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private int currentLane = 1;
    private float[] lanePositions = { -2.5f, 0f, 2.5f };
    private bool isMoving = false;
    public float laneChangeSpeed = 10f;

    void Start()
    {
        // Bloquear eje Y y rotaciones para que los obstaculos no empujen al jugador hacia arriba
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY
                           | RigidbodyConstraints.FreezeRotationX
                           | RigidbodyConstraints.FreezeRotationY
                           | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeLane(1);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            ChangeLane(-1);
    }

    void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;

        if (targetLane < 0 || targetLane > 2) return;
        if (isMoving) return;

        currentLane = targetLane;
        StartCoroutine(MoveToLane(lanePositions[currentLane]));
    }

    IEnumerator MoveToLane(float targetX)
    {
        isMoving = true;

        while (Mathf.Abs(transform.position.x - targetX) > 0.01f)
        {
            float newX = Mathf.MoveTowards(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        isMoving = false;
    }
}