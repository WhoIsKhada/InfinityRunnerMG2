using System.Collections;
using UnityEngine;

/// <summary>
/// Attach to the Player GameObject.
/// Called by SectionTrigger every time a new road section spawns.
/// Randomizes both sideways (X) and backways (Y) shader curvature simultaneously.
/// </summary>
public class SidewaysTrigger : MonoBehaviour
{
    [Header("Shader Materials")]
    public Material[] myMaterials;

    [Header("Transition")]
    public float lerpTime = 2f;

    [Header("Sideways Curve (X axis) — _Sideways_Strength")]
    public float minSidewaysStrength = -0.005f;
    public float maxSidewaysStrength = 0.005f;

    [Header("Backways Curve (Y axis) — _Backways_Strength")]
    public float minBackwaysStrength = -0.005f;
    public float maxBackwaysStrength = 0.005f;

    private float currentSideways;
    private float currentBackways;
    private bool isLerping = false;

    private const string PROP_SIDEWAYS = "_Sideways_Strength";
    private const string PROP_BACKWAYS = "_Backways_Strength";

    private void Start()
    {
        if (myMaterials != null && myMaterials.Length > 0 && myMaterials[0] != null)
        {
            currentSideways = myMaterials[0].GetFloat(PROP_SIDEWAYS);
            currentBackways = myMaterials[0].GetFloat(PROP_BACKWAYS);
        }
        else
        {
            Debug.LogWarning("[SidewaysTrigger] No materials assigned in the Inspector.");
        }
    }

    /// <summary>
    /// Called by SectionTrigger when a new section spawns.
    /// </summary>
    public void OnNewSectionSpawned()
    {
        if (!isLerping)
            StartCoroutine(ChangeCurveStrength());
    }

    private IEnumerator ChangeCurveStrength()
    {
        isLerping = true;

        float elapsed = 0f;
        float startSideways = currentSideways;
        float startBackways = currentBackways;
        float targetSideways = Random.Range(minSidewaysStrength, maxSidewaysStrength);
        float targetBackways = Random.Range(minBackwaysStrength, maxBackwaysStrength);

        Debug.Log($"[SidewaysTrigger] Sideways: {startSideways:F4} → {targetSideways:F4} | Backways: {startBackways:F4} → {targetBackways:F4}");

        while (elapsed < lerpTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / lerpTime);

            currentSideways = Mathf.Lerp(startSideways, targetSideways, t);
            currentBackways = Mathf.Lerp(startBackways, targetBackways, t);

            foreach (Material mat in myMaterials)
            {
                if (mat != null)
                {
                    mat.SetFloat(PROP_SIDEWAYS, currentSideways);
                    mat.SetFloat(PROP_BACKWAYS, currentBackways);
                }
            }

            yield return null;
        }

        // Snap to exact target
        currentSideways = targetSideways;
        currentBackways = targetBackways;
        foreach (Material mat in myMaterials)
        {
            if (mat != null)
            {
                mat.SetFloat(PROP_SIDEWAYS, currentSideways);
                mat.SetFloat(PROP_BACKWAYS, currentBackways);
            }
        }

        isLerping = false;
    }

    private void OnDisable() => ResetMaterials();
    private void OnApplicationQuit() => ResetMaterials();

    private void ResetMaterials()
    {
        if (myMaterials == null) return;
        foreach (Material mat in myMaterials)
        {
            if (mat != null)
            {
                mat.SetFloat(PROP_SIDEWAYS, 0f);
                mat.SetFloat(PROP_BACKWAYS, 0f);
            }
        }
    }
}
