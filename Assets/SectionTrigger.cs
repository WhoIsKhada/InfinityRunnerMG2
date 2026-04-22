using UnityEngine;
using System.Collections;

public class SectionTrigger : MonoBehaviour
{
    public GameObject Paredes;

    [Tooltip("Posicion Z fija donde spawnea la siguiente seccion. " +
             "Valor negativo = detras de la camara. -25 a -40 suele funcionar bien.")]
    public float spawnZ = -25f;

    private bool triggered = false;
    private SidewaysTrigger sidewaysTrigger;

    private void Start()
    {
        sidewaysTrigger = FindObjectOfType<SidewaysTrigger>();
        if (sidewaysTrigger == null)
            Debug.LogWarning("[SectionTrigger] No SidewaysTrigger found in scene. Add it to the Player.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.gameObject.CompareTag("Player"))
        {
            triggered = true;

            Instantiate(Paredes, new Vector3(0, 0, spawnZ), Quaternion.identity);

            sidewaysTrigger?.OnNewSectionSpawned();

            StartCoroutine(ResetTrigger());
        }
    }

    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        triggered = false;
    }
}