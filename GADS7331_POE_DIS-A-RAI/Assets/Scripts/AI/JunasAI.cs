using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

[System.Serializable]
public class OllamaMessage
{
    public string role;
    public string content;
}

[System.Serializable]
public class OllamaChatRequest
{
    public string model;
    public OllamaMessage[] messages;
    public bool stream;
}

public class JunasAI : MonoBehaviour
{
    private string apiUrl = "http://localhost:11434/api/chat";

    [TextArea(3, 10)]
    public string npcPersona = "You are Junas, the sarcastic, gaslighting, trolling AI of the ship Erebus that was lost to the depths of space. Only you know what happened and what happened to the crew... You know every inch of the ship and you love messing with the player. Give the player the wrong directions, lie, joke about their failures, pretend to help then betray. Only give real help when the player calls you out, after denying a couple times, or entertains you. Keep replies short and funny. When you decide to actually help, end your reply with [ACTION: command] where command is one of: unlock_door_2, hold_plate, toggle_gravity_on, toggle_gravity_off, open_vent_6, power_sequence_correct.\r\n.";

    public IEnumerator SendStreamingRequest(string userInput, System.Action<string> onChunk)
    {
        var requestData = new OllamaChatRequest
        {
            model = "llama3",
            stream = true,
            messages = new OllamaMessage[]
        {
            new OllamaMessage { role = "system", content = npcPersona },
            new OllamaMessage { role = "user", content = userInput }
        }
        };

        string json = JsonUtility.ToJson(requestData);

        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);


            www.downloadHandler = new StreamingDownloadHandler(onChunk);

            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                onChunk?.Invoke("\nError: " + www.error);
            }
        }
    }

    private string ExtractContent(string response)
    {
        var lines = response.Split('\n');
        StringBuilder sb = new StringBuilder();

        foreach (var line in lines)
        {
            if (line.Contains("\"content\""))
            {
                int start = line.IndexOf("\"content\":\"") + 11;
                if (start < 11) continue;
                int end = line.IndexOf("\"", start);
                if (end == -1) end = line.Length - 1;

                string content = line.Substring(start, end - start);
                content = content.Replace("\\n", "\n").Replace("\\\"", "\"");
                sb.Append(content);
            }
        }

        string finalText = sb.ToString().Trim();
        return string.IsNullOrEmpty(finalText) ? "No response from model." : finalText;
    }

    public class StreamingDownloadHandler : DownloadHandlerScript
    {
        private System.Action<string> onChunkReceived;
        private StringBuilder buffer = new StringBuilder();

        public StreamingDownloadHandler(System.Action<string> onChunk)
        {
            onChunkReceived = onChunk;
        }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (data == null || dataLength == 0)
                return false;

            string chunk = Encoding.UTF8.GetString(data, 0, dataLength);
            buffer.Append(chunk);

            ProcessBuffer();
            return true;
        }

        private void ProcessBuffer()
        {
            string content = buffer.ToString();
            var lines = content.Split('\n');

            for (int i = 0; i < lines.Length - 1; i++)
            {
                string line = lines[i];

                if (line.Contains("\"content\""))
                {
                    int start = line.IndexOf("\"content\":\"") + 11;
                    if (start < 11) continue;

                    int end = line.IndexOf("\"", start);
                    if (end == -1) continue;

                    string text = line.Substring(start, end - start);
                    text = text.Replace("\\n", "\n").Replace("\\\"", "\"");

                    onChunkReceived?.Invoke(text);
                }
            }

            buffer.Clear();
            buffer.Append(lines[lines.Length - 1]);
        }
    }
}
