using System.Collections.Generic;
using UnityEngine;

public class CombatEffects : MonoBehaviour
{
    const float ShockDuration = 0.55f;
    const float BallSpeed = 55f;

    static CombatEffects instance;
    static Material particleMaterial;
    static Texture2D softCircle;

    readonly List<ElectricBall> balls = new List<ElectricBall>();
    readonly List<BotShock> shocks = new List<BotShock>();

    class ElectricBall
    {
        public Transform transform;
        public Vector3 target;
        public bool stopping;
    }

    class BotShock
    {
        public Transform host;
        public ParticleSystem sparks;
        public float until;
        public bool stopping;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        if (FindAnyObjectByType<CombatEffects>() != null)
            return;

        GameObject host = new GameObject("CombatEffects");
        DontDestroyOnLoad(host);
        host.AddComponent<CombatEffects>();
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public static void SpawnElectricBall(Vector3 from, Vector3 to)
    {
        CombatEffects fx = Ensure();
        GameObject ball = new GameObject("ElectricBall");
        ball.transform.position = from;

        ParticleSystem core = CreateSystem(ball);
        ConfigureCore(core, new Color(0.75f, 0.95f, 1f), new Color(0.25f, 0.7f, 1f), 0.16f, 0.28f, 18f);

        GameObject sparkObject = new GameObject("Sparks");
        sparkObject.transform.SetParent(ball.transform, false);
        ParticleSystem sparks = CreateSystem(sparkObject);
        ConfigureSparks(sparks, new Color(0.6f, 0.9f, 1f), new Color(0.85f, 0.95f, 1f), 0.04f, 0.09f, 70f, 2.2f);

        fx.balls.Add(new ElectricBall
        {
            transform = ball.transform,
            target = to
        });
    }

    public static void ShockBot(Transform bot, Vector3 point)
    {
        if (bot == null)
            return;

        CombatEffects fx = Ensure();
        for (int i = 0; i < fx.shocks.Count; i++)
        {
            BotShock existing = fx.shocks[i];
            if (existing.host != bot || existing.stopping)
                continue;

            existing.until = Time.time + ShockDuration;
            existing.sparks.transform.position = point;
            if (!existing.sparks.isPlaying)
                existing.sparks.Play();
            return;
        }

        GameObject shockObject = new GameObject("BotElectricity");
        shockObject.transform.SetParent(bot, false);
        shockObject.transform.position = point;

        ParticleSystem sparks = CreateSystem(shockObject);
        ConfigureSparks(sparks, new Color(0.45f, 0.85f, 1f), Color.white, 0.05f, 0.14f, 55f, 1.4f);
        var main = sparks.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        fx.shocks.Add(new BotShock
        {
            host = bot,
            sparks = sparks,
            until = Time.time + ShockDuration
        });
    }

    public static void Explode(Vector3 position)
    {
        Ensure();
        GameObject explosion = new GameObject("BotExplosion");
        explosion.transform.position = position;

        ParticleSystem burst = CreateSystem(explosion);
        var main = burst.main;
        main.duration = 0.35f;
        main.loop = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.35f, 0.8f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(2.5f, 7.5f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.18f, 0.55f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0.85f, 0.35f), new Color(1f, 0.28f, 0.05f));
        main.gravityModifier = 0.35f;
        main.maxParticles = 80;

        var emission = burst.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 48) });

        var shape = burst.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.25f;

        var size = burst.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
            new Keyframe(0f, 0.4f),
            new Keyframe(0.2f, 1f),
            new Keyframe(1f, 0.05f)));

        Fade(burst, new Color(1f, 0.7f, 0.2f), new Color(0.15f, 0.05f, 0.02f));
        burst.Play();

        GameObject sparkObject = new GameObject("ExplosionSparks");
        sparkObject.transform.SetParent(explosion.transform, false);
        ParticleSystem sparks = CreateSystem(sparkObject);
        ConfigureSparks(sparks, new Color(1f, 0.95f, 0.6f), new Color(1f, 0.4f, 0.1f), 0.04f, 0.1f, 0f, 6f);
        sparks.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var sparkMain = sparks.main;
        sparkMain.loop = false;
        sparkMain.duration = 0.2f;
        sparkMain.startLifetime = new ParticleSystem.MinMaxCurve(0.25f, 0.6f);
        var sparkEmission = sparks.emission;
        sparkEmission.rateOverTime = 0f;
        sparkEmission.SetBursts(new[] { new ParticleSystem.Burst(0f, 24) });
        sparks.Play();

        Object.Destroy(explosion, 1.6f);
    }

    void Update()
    {
        float step = BallSpeed * Time.deltaTime;
        for (int i = balls.Count - 1; i >= 0; i--)
        {
            ElectricBall ball = balls[i];
            if (ball.transform == null)
            {
                balls.RemoveAt(i);
                continue;
            }

            if (!ball.stopping)
            {
                ball.transform.position = Vector3.MoveTowards(ball.transform.position, ball.target, step);
                if (Vector3.Distance(ball.transform.position, ball.target) <= 0.08f)
                {
                    ParticleSystem core = ball.transform.GetComponent<ParticleSystem>();
                    if (core != null)
                        core.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    ball.stopping = true;
                    Destroy(ball.transform.gameObject, 0.35f);
                }
            }
        }

        for (int i = shocks.Count - 1; i >= 0; i--)
        {
            BotShock shock = shocks[i];
            if (shock.sparks == null)
            {
                shocks.RemoveAt(i);
                continue;
            }

            if (!shock.stopping && Time.time >= shock.until)
            {
                shock.sparks.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                shock.stopping = true;
                Destroy(shock.sparks.gameObject, 0.45f);
            }
            else if (shock.stopping && !shock.sparks.IsAlive(true))
            {
                shocks.RemoveAt(i);
            }
        }
    }

    static CombatEffects Ensure()
    {
        if (instance != null)
            return instance;

        GameObject host = new GameObject("CombatEffects");
        DontDestroyOnLoad(host);
        instance = host.AddComponent<CombatEffects>();
        return instance;
    }

    static ParticleSystem CreateSystem(GameObject host)
    {
        ParticleSystem system = host.AddComponent<ParticleSystem>();
        system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = system.main;
        main.playOnAwake = false;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystemRenderer renderer = host.GetComponent<ParticleSystemRenderer>();
        renderer.material = ParticleMaterial();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        return system;
    }

    static void ConfigureCore(ParticleSystem system, Color bright, Color deep, float minSize, float maxSize, float rate)
    {
        var main = system.main;
        main.loop = true;
        main.startLifetime = 0.18f;
        main.startSpeed = 0.15f;
        main.startSize = new ParticleSystem.MinMaxCurve(minSize, maxSize);
        main.startColor = new ParticleSystem.MinMaxGradient(bright, deep);
        main.maxParticles = 40;

        var emission = system.emission;
        emission.rateOverTime = rate;

        var shape = system.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.04f;

        Fade(system, bright, deep);
        system.Play();
    }

    static void ConfigureSparks(ParticleSystem system, Color bright, Color deep, float minSize, float maxSize, float rate, float speed)
    {
        var main = system.main;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.08f, 0.22f);
        main.startSpeed = speed;
        main.startSize = new ParticleSystem.MinMaxCurve(minSize, maxSize);
        main.startColor = new ParticleSystem.MinMaxGradient(bright, deep);
        main.maxParticles = 60;

        var emission = system.emission;
        emission.rateOverTime = rate;

        var shape = system.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.12f;

        var noise = system.noise;
        noise.enabled = true;
        noise.strength = 1.1f;
        noise.frequency = 2.4f;
        noise.scrollSpeed = 1.5f;

        Fade(system, bright, deep);
        system.Play();
    }

    static void Fade(ParticleSystem system, Color bright, Color deep)
    {
        var color = system.colorOverLifetime;
        color.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(bright, 0f),
                new GradientColorKey(deep, 1f)
            },
            new[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0.85f, 0.35f),
                new GradientAlphaKey(0f, 1f)
            });
        color.color = gradient;
    }

    static Material ParticleMaterial()
    {
        if (particleMaterial != null)
            return particleMaterial;

        Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        if (shader == null)
            shader = Shader.Find("Particles/Standard Unlit");
        if (shader == null)
            shader = Shader.Find("Sprites/Default");

        particleMaterial = new Material(shader);
        Texture2D circle = SoftCircle();
        if (particleMaterial.HasProperty("_BaseMap"))
            particleMaterial.SetTexture("_BaseMap", circle);
        if (particleMaterial.HasProperty("_BaseColor"))
            particleMaterial.SetColor("_BaseColor", Color.white);
        if (particleMaterial.HasProperty("_Color"))
            particleMaterial.SetColor("_Color", Color.white);
        if (particleMaterial.HasProperty("_Surface"))
            particleMaterial.SetFloat("_Surface", 1f);
        if (particleMaterial.HasProperty("_Blend"))
            particleMaterial.SetFloat("_Blend", 2f);
        if (particleMaterial.HasProperty("_SrcBlend"))
            particleMaterial.SetFloat("_SrcBlend", 1f);
        if (particleMaterial.HasProperty("_DstBlend"))
            particleMaterial.SetFloat("_DstBlend", 1f);
        if (particleMaterial.HasProperty("_ZWrite"))
            particleMaterial.SetFloat("_ZWrite", 0f);
        particleMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        particleMaterial.renderQueue = 3000;
        return particleMaterial;
    }

    static Texture2D SoftCircle()
    {
        if (softCircle != null)
            return softCircle;

        const int size = 64;
        softCircle = new Texture2D(size, size, TextureFormat.RGBA32, false);
        softCircle.wrapMode = TextureWrapMode.Clamp;
        float center = (size - 1) * 0.5f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = (x - center) / center;
                float dy = (y - center) / center;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01(1f - distance);
                alpha *= alpha;
                softCircle.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }

        softCircle.Apply();
        return softCircle;
    }
}
