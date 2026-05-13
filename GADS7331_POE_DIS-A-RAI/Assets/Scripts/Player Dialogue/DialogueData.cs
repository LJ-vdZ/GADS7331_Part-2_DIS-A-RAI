using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("Dialogue Info")]
    public Sprite icon;
    public string alertTitle;
    [TextArea(4, 8)]
    public string dialogueText;
}
