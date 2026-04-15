using UnityEngine;
using TMPro;

/// <summary>
/// Singleton que acumula el score (distancia recorrida) y lo muestra en UI.
/// Agregar este componente a un GameObject vacio en la escena (ej: "GameManager").
/// Asignar un TextMeshProUGUI en el Inspector para mostrar el score.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Score")]
    [SerializeField] private float scoreMultiplier = 1f;

    private float score = 0f;
    public float Score => score;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        // Time.timeScale == 0 cuando hay Game Over: el score se detiene automaticamente
        score += Move.CurrentSpeed * scoreMultiplier * Time.deltaTime;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString("D5");
    }
}