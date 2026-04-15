using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Crea el canvas de Game Over en runtime — sin configuracion en Inspector.
/// Agregar este script a cualquier GameObject vacio en la escena (ej: "GameManager").
/// </summary>
public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    private GameObject panel;

    void Awake()
    {
        Instance = this;
        BuildUI();
        panel.SetActive(false);
    }

    void BuildUI()
    {
        // --- Canvas ---
        GameObject canvasGO = new GameObject("GameOverCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 99;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasGO.AddComponent<GraphicRaycaster>();

        // --- Panel de fondo semitransparente ---
        panel = new GameObject("Panel");
        panel.transform.SetParent(canvasGO.transform, false);
        Image bg = panel.AddComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.78f);
        StretchFull(panel.GetComponent<RectTransform>());

        // --- Texto "GAME OVER" ---
        CreateText(panel.transform, "GAME OVER", 90, FontStyle.Bold, Color.white,
                   new Vector2(0.5f, 0.58f), new Vector2(800, 120));

        // --- Texto subtitulo ---
        CreateText(panel.transform, "Perdiste todas tus vidas", 36, FontStyle.Italic,
                   new Color(1f, 0.85f, 0.85f, 1f),
                   new Vector2(0.5f, 0.48f), new Vector2(700, 60));

        // --- Boton REINTENTAR ---
        CreateButton(panel.transform, "REINTENTAR", new Vector2(0.5f, 0.35f),
                     new Vector2(340, 80), new Color(0.18f, 0.55f, 1f, 1f), Restart);

        // --- Boton SALIR (opcional) ---
        CreateButton(panel.transform, "SALIR", new Vector2(0.5f, 0.22f),
                     new Vector2(340, 60), new Color(0.35f, 0.35f, 0.38f, 1f), QuitGame);
    }

    // --- Helpers ---

    void StretchFull(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    void CreateText(Transform parent, string content, int size, FontStyle style,
                    Color color, Vector2 anchorCenter, Vector2 sizeDelta)
    {
        GameObject go = new GameObject(content);
        go.transform.SetParent(parent, false);
        Text t = go.AddComponent<Text>();
        t.text = content;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = size;
        t.fontStyle = style;
        t.color = color;
        t.alignment = TextAnchor.MiddleCenter;
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = anchorCenter;
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = sizeDelta;
    }

    void CreateButton(Transform parent, string label, Vector2 anchorCenter,
                      Vector2 sizeDelta, Color btnColor, UnityEngine.Events.UnityAction action)
    {
        // Contenedor del boton
        GameObject btnGO = new GameObject(label + "_Btn");
        btnGO.transform.SetParent(parent, false);
        Image img = btnGO.AddComponent<Image>();
        img.color = btnColor;
        Button btn = btnGO.AddComponent<Button>();
        btn.onClick.AddListener(action);

        // Efecto hover
        ColorBlock colors = btn.colors;
        colors.highlightedColor = new Color(
            Mathf.Clamp01(btnColor.r + 0.15f),
            Mathf.Clamp01(btnColor.g + 0.15f),
            Mathf.Clamp01(btnColor.b + 0.15f));
        btn.colors = colors;

        RectTransform rt = btnGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = anchorCenter;
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = sizeDelta;

        // Texto del boton
        GameObject txtGO = new GameObject("Label");
        txtGO.transform.SetParent(btnGO.transform, false);
        Text t = txtGO.AddComponent<Text>();
        t.text = label;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = 32;
        t.fontStyle = FontStyle.Bold;
        t.color = Color.white;
        t.alignment = TextAnchor.MiddleCenter;
        RectTransform trt = txtGO.GetComponent<RectTransform>();
        trt.anchorMin = Vector2.zero;
        trt.anchorMax = Vector2.one;
        trt.offsetMin = trt.offsetMax = Vector2.zero;
    }

    // --- API publica ---

    public void Show()
    {
        panel.SetActive(true);
    }

    void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}