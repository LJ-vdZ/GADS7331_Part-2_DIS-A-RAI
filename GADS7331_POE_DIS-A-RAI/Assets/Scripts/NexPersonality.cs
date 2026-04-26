using UnityEngine;

[CreateAssetMenu(fileName = "NexPersonality", menuName = "Detour/Nex Personality")]
public class NexPersonality : ScriptableObject
{
    [TextArea(15, 40)]
    public string systemPrompt;

    [Header("Response Settings")]
    public float temperature = 0.87f;
    
    public int maxTokens = 260;

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

}
