using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Audio.ProcessorInstance;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private TextMeshProUGUI outputText;

    [SerializeField]
    private Button sendButton;

    [SerializeField]
    private JunasAI JunasAI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sendButton.onClick.AddListener(OnSendClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSendClicked()
    {
        string userText = inputField.text.Trim();

        if (string.IsNullOrEmpty(userText)) return;

        inputField.text = "";

        outputText.text = "";

        StartCoroutine(JunasAI.SendStreamingRequest(userText, OnStreamChunk));
    }

    private void OnStreamChunk(string chunk)
    {
        outputText.text += chunk;
    }
}
