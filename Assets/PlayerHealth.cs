using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;
    private bool isInvincible = false;
    public float invincibilityTime = 0.5f;
    public float blinkInterval = 0.08f;

    [Header("UI — Corazones")]
    [SerializeField] private GameObject[] heartIcons;

    private Renderer playerRenderer;

    void Start()
    {
        Time.timeScale = 1f;
        playerRenderer = GetComponent<Renderer>();
        UpdateHeartsUI();
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        lives--;
        UpdateHeartsUI();

        if (lives <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibilityFrames());
    }

    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        float elapsed = 0f;
        while (elapsed < invincibilityTime)
        {
            if (playerRenderer != null)
                playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }
        if (playerRenderer != null)
            playerRenderer.enabled = true;
        isInvincible = false;
    }

    void Die()
    {
        if (playerRenderer != null)
            playerRenderer.enabled = true;

        Time.timeScale = 0f;

        // Mostrar canvas de Game Over (creado por GameOverUI.cs)
        if (GameOverUI.Instance != null)
            GameOverUI.Instance.Show();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // fallback
    }

    private void UpdateHeartsUI()
    {
        if (heartIcons == null) return;
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (heartIcons[i] != null)
                heartIcons[i].SetActive(i < lives);
        }
    }
}