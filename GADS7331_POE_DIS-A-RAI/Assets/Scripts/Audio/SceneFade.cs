using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    public static SceneFade Instance { get; private set; }

    const float FadeOutDuration = 0.45f;
    const float FadeInDuration = 0.55f;

    CanvasGroup group;
    bool loading;
    Coroutine fadeRoutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        BuildOverlay();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void Load(string sceneName)
    {
        if (Instance == null)
        {
            SceneManager.LoadScene(sceneName);
            return;
        }

        Instance.BeginLoad(sceneName);
    }

    void BeginLoad(string sceneName)
    {
        if (loading)
            return;

        loading = true;
        if (sceneName != "StartMenu" && sceneName != "WinState" && sceneName != "GameOver")
            GameAudio.StopMenuMusic();

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        yield return Fade(group.alpha, 1f, FadeOutDuration);
        SceneManager.LoadScene(sceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loading = false;
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(Fade(1f, 0f, FadeInDuration));
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        group.blocksRaycasts = true;
        group.alpha = from;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }

        group.alpha = to;
        group.blocksRaycasts = to > 0.01f;
        fadeRoutine = null;
    }

    void BuildOverlay()
    {
        GameObject canvasObject = new GameObject("FadeCanvas");
        canvasObject.transform.SetParent(transform, false);

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32767;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject imageObject = new GameObject("Fade");
        imageObject.transform.SetParent(canvasObject.transform, false);

        RectTransform rect = imageObject.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = imageObject.AddComponent<Image>();
        image.color = Color.black;
        image.raycastTarget = true;

        group = imageObject.AddComponent<CanvasGroup>();
        group.alpha = 1f;
        group.blocksRaycasts = true;
        group.interactable = false;
    }
}
