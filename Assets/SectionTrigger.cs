using UnityEngine;
using System.Collections;

public class SectionTrigger : MonoBehaviour
{
    public GameObject Paredes; // ← debe ser prefab de Assets/
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.gameObject.CompareTag("Player"))
        {
            triggered = true;
            Instantiate(Paredes, new Vector3(0, 0, -25), Quaternion.identity);
            StartCoroutine(ResetTrigger());
        }
    }

    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        triggered = false;
    }
}