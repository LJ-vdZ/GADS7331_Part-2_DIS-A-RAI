using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance { get; private set; }

    enum FootstepMode
    {
        Off,
        Walk,
        Run
    }

    GameAudioLibrary library;
    AudioSource oneShotSource;
    AudioSource footstepSource;
    AudioSource mapSource;
    AudioSource dialogueSource;
    AudioSource jaiSource;
    AudioSource menuSource;

    FootstepMode footstepMode = FootstepMode.Off;
    GameObject shipMapPanel;
    string shipMapScene = "";
    readonly HashSet<int> hookedButtonIds = new HashSet<int>();
    float nextButtonScan;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        if (FindAnyObjectByType<GameAudio>() != null)
            return;

        GameObject services = new GameObject("GameServices");
        DontDestroyOnLoad(services);
        services.AddComponent<GameAudio>();
        services.AddComponent<SceneFade>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        library = Resources.Load<GameAudioLibrary>("GameAudioLibrary");
        if (library == null)
            Debug.LogError("GameAudioLibrary is missing from a Resources folder. Sound effects will not play.");

        oneShotSource = CreateSource(false);
        footstepSource = CreateSource(true);
        mapSource = CreateSource(true);
        dialogueSource = CreateSource(true);
        jaiSource = CreateSource(true);
        menuSource = CreateSource(true);

        Preload(library);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        UpdateShipMap();
        if (Time.unscaledTime >= nextButtonScan)
        {
            nextButtonScan = Time.unscaledTime + 0.5f;
            HookUiButtons();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hookedButtonIds.Clear();
        shipMapPanel = null;
        shipMapScene = "";
        SetPlayerFootsteps(false, false);
        EndDialogueTyping();
        EndJaiTyping();
        StopShipMap();

        if (IsMenuScene(scene.name))
            PlayMenuMusic();
        else
            StopMenuMusic();

        HookUiButtons();
    }

    public static AudioClip RobotWalkingClip
    {
        get
        {
            if (Instance == null || Instance.library == null)
                return null;
            return Instance.library.robotWalking;
        }
    }

    public static void PlayUiClick()
    {
        if (Instance == null || Instance.library == null)
            return;
        if (!IsClickScene(SceneManager.GetActiveScene().name))
            return;
        Instance.PlayOneShot(Instance.library.uiClick, 0.8f);
    }

    public static void PlayEquipGun()
    {
        if (Instance == null || Instance.library == null)
            return;
        Instance.PlayOneShot(Instance.library.equipGun, 0.85f);
    }

    public static void PlayGunShot()
    {
        if (Instance == null || Instance.library == null)
            return;
        Instance.PlayOneShot(Instance.library.gunShot, 0.8f);
    }

    public static void SetPlayerFootsteps(bool moving, bool sprinting)
    {
        Instance?.ApplyFootsteps(moving, sprinting);
    }

    public static void BeginDialogueTyping()
    {
        if (Instance == null || Instance.library == null)
            return;
        Instance.Loop(Instance.dialogueSource, Instance.library.dialogueText, 0.55f);
    }

    public static void EndDialogueTyping()
    {
        if (Instance == null)
            return;
        if (Instance.dialogueSource.isPlaying)
            Instance.dialogueSource.Stop();
    }

    public static void BeginJaiTyping()
    {
        if (Instance == null || Instance.library == null)
            return;
        Instance.Loop(Instance.jaiSource, Instance.library.jaiTyping, 0.5f);
    }

    public static void EndJaiTyping()
    {
        if (Instance == null)
            return;
        if (Instance.jaiSource.isPlaying)
            Instance.jaiSource.Stop();
    }

    public static void StopMenuMusic()
    {
        if (Instance == null)
            return;
        if (Instance.menuSource.isPlaying)
            Instance.menuSource.Stop();
    }

    void PlayMenuMusic()
    {
        if (library == null)
            return;
        Loop(menuSource, library.menu, 0.38f);
    }

    void ApplyFootsteps(bool moving, bool sprinting)
    {
        FootstepMode next = !moving ? FootstepMode.Off : sprinting ? FootstepMode.Run : FootstepMode.Walk;
        if (next == footstepMode)
            return;

        footstepMode = next;
        if (next == FootstepMode.Off || library == null)
        {
            footstepSource.Stop();
            return;
        }

        AudioClip clip = next == FootstepMode.Run ? library.running : library.walking;
        footstepSource.volume = next == FootstepMode.Run ? 0.58f : 0.48f;
        Loop(footstepSource, clip, footstepSource.volume);
    }

    void UpdateShipMap()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (shipMapPanel == null || shipMapScene != scene.name)
        {
            shipMapPanel = FindNamedInScene("ShipMapPanel", scene);
            shipMapScene = scene.name;
        }

        bool open = shipMapPanel != null && shipMapPanel.activeInHierarchy;
        if (open && library != null)
            Loop(mapSource, library.shipMapGlitch, 0.42f);
        else
            StopShipMap();
    }

    void StopShipMap()
    {
        if (mapSource != null && mapSource.isPlaying)
            mapSource.Stop();
    }

    void HookUiButtons()
    {
        if (!IsClickScene(SceneManager.GetActiveScene().name))
            return;

        Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Button button in buttons)
        {
            if (button == null || !button.gameObject.scene.IsValid())
                continue;
            if (!hookedButtonIds.Add(button.GetInstanceID()))
                continue;
            button.onClick.AddListener(PlayUiClick);
        }
    }

    static GameObject FindNamedInScene(string objectName, Scene scene)
    {
        Transform[] transforms = FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Transform item in transforms)
        {
            if (item.name == objectName && item.gameObject.scene == scene)
                return item.gameObject;
        }
        return null;
    }

    void PlayOneShot(AudioClip clip, float volume)
    {
        if (clip == null || oneShotSource == null)
            return;
        oneShotSource.PlayOneShot(clip, volume);
    }

    void Loop(AudioSource source, AudioClip clip, float volume)
    {
        if (source == null || clip == null)
            return;

        source.volume = volume;
        source.loop = true;
        if (source.isPlaying && source.clip == clip)
            return;

        source.clip = clip;
        source.Play();
    }

    static void Preload(GameAudioLibrary clips)
    {
        if (clips == null)
            return;

        Load(clips.uiClick);
        Load(clips.equipGun);
        Load(clips.gunShot);
        Load(clips.jaiTyping);
        Load(clips.menu);
        Load(clips.shipMapGlitch);
        Load(clips.dialogueText);
        Load(clips.walking);
        Load(clips.running);
        Load(clips.robotWalking);
    }

    static void Load(AudioClip clip)
    {
        if (clip != null)
            clip.LoadAudioData();
    }

    AudioSource CreateSource(bool loop)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = loop;
        source.spatialBlend = 0f;
        return source;
    }

    static bool IsMenuScene(string sceneName)
    {
        return sceneName == "StartMenu" || sceneName == "WinState" || sceneName == "GameOver";
    }

    static bool IsClickScene(string sceneName)
    {
        return IsMenuScene(sceneName) || sceneName == "Gameplay";
    }
}
