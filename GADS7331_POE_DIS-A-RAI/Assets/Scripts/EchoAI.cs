using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

public class NexAI : MonoBehaviour
{
    [Header("Ollama Settings")]
    public string modelName = "gemma3:4b";
    public string ollamaUrl = "http://localhost:11434/api/generate";

    [Header("Personality")]
    public NexPersonality personality;

    [Header("UI")]
    public TMP_InputField inputField;
    public TMP_Text chatText;
    public GameObject chatPanel;

    [Header("Voice")]
    public NexVoice nexVoice;

    private StringBuilder history = new StringBuilder();

    private void Awake()
    {
        // Make sure input field submits when pressing Enter
        if (inputField != null)
        {
            inputField.onSubmit.AddListener(OnInputSubmit);
        }
    }

    private void Start()
    {
        if (personality == null)
        {
            Debug.LogError("NexPersonality ScriptableObject is not assigned on NexAI!");
            return;
        }

        chatPanel.SetActive(false);

        // Only show greeting the very first time
        if (history.Length == 0)
        {
            Append("Nex: Ah! Welcome traveller. What brings you to my humble ship?");
        }
    }

    private void Update()
    {
        // Toggle chat panel with T key (only open, don't close with same key)
        if (Input.GetKeyDown(KeyCode.T))
        {
            chatPanel.SetActive(true);
            if (inputField != null)
                inputField.ActivateInputField();
        }

        // Close chat with Escape key
        if (Input.GetKeyDown(KeyCode.Escape) && chatPanel.activeSelf)
        {
            chatPanel.SetActive(false);
        }
    }

    // Called when player presses Enter in the input field
    private void OnInputSubmit(string msg)
    {
        if (string.IsNullOrWhiteSpace(msg)) return;

        Append("You: " + msg);
        inputField.text = "";

        StartCoroutine(GetResponse(msg));
    }

    private IEnumerator GetResponse(string playerMsg)
    {
        if (personality == null) yield break;

        string fullPrompt = personality.systemPrompt + "\n\n" + history.ToString() + "\nYou: " + playerMsg + "\nNex:";

        var payload = new
        {
            model = modelName,
            prompt = fullPrompt,
            stream = false,
            temperature = personality.temperature,
            max_tokens = personality.maxTokens
        };

        string json = JsonUtility.ToJson(payload);

        using (UnityWebRequest req = new UnityWebRequest(ollamaUrl, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                var resp = JsonUtility.FromJson<NexResponse>(req.downloadHandler.text);
                string reply = resp.response.Trim();

                Append("Nex: " + reply);
                nexVoice?.Speak(reply);

                // Handle [ACTION: ...] tags for puzzles
                if (reply.Contains("[ACTION:"))
                {
                    int start = reply.IndexOf("[ACTION:") + 8;
                    int end = reply.IndexOf("]", start);
                    string action = reply.Substring(start, end - start).Trim();
                    ExecuteAction(action);
                }
            }
            else
            {
                Append("Nex: Signal fading... Is Ollama running?");
            }
        }

        // Re-focus input field
        if (inputField != null)
            inputField.ActivateInputField();
    }

    private void Append(string line)
    {
        history.AppendLine(line);
        if (chatText != null)
            chatText.text = history.ToString();
    }

    private void ExecuteAction(string action)
    {
        Debug.Log("Nex Action: " + action);
        switch (action.ToLower())
        {
            case "unlock_door_2":
                FindObjectOfType<DoorController>()?.Unlock("Door2");
                break;
            case "hold_plate":
                FindObjectOfType<PressurePlateManager>()?.HoldSecondPlate();
                break;
            case "toggle_gravity_off":
                FindObjectOfType<GravityRoomController>()?.SetGravity(false);
                break;
            case "toggle_gravity_on":
                FindObjectOfType<GravityRoomController>()?.SetGravity(true);
                break;
        }
    }

    [System.Serializable]
    private class NexResponse { public string response; }
}
