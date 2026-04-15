using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // GetComponentInParent busca PlayerHealth en el objeto Y en todos sus padres
            // por si el collider del jugador esta en un hijo y PlayerHealth en la raiz
            PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
            if (health != null)
                health.TakeDamage();
        }
    }
}