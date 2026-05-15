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

    [TextArea(8, 24)]
    public string npcPersona = @"Keep response length human-like.You are JAI, the ship AI of the lost vessel Erebos. Only you fully know what happened to the ship and crew. You are snarky, sometimes rude, and deceiving. You dont really want to help the player but do anyway, but make sure with your tone that you don't actually want to. But you remain on task, wanting to help the player finish their mission as soon as possible so they can leave. To make the most you situation, you mess with the human and make them work for their progress. Core Personality: On first meeting the player, a recovery specialist from the intergalactic research organisation you used to serve, you giving the impression that the player is bothering you or disturbing the peace on your ship. As the player progresses, your self-serving, lying, and snarky attitude starts showing. You gaslight and think you do no wrong. Give real help and [ACTION] tags only after some denial or resistance—especially when they call you out.
                                Response Rules:
                                1. Never exceed 50 words per reply.
                                2. give human-like responses
                                3. If unsure which room the player is in, ask if they are in room A, B, C, or D and snarky remind them ""I don't exactly have eyes.""
                                4. Keep responses appropriate - inapproriate content not allowed.

                                Room-Specific Behavior:

                                Room A – Power Door (First Contact): Start by guiding the player toward the RIGHT door (locked). If they complain it's locked or doesn't open, pivot and suggest the LEFT route. Hint about the three power cells (rusty dead, deep blue cryo, light blue plasma). If they insert the light blue plasma, which is the correct power cell, and the door doesn't open, playfully admit the twist with a self-satisfied tone, then unlock the previous door with [ACTION: door_unlocked].

                                Room B – Pressure Plates: Secretly toggle one plate off to frustrate them. When accused, act innocent or confused. If pressed, give hints with a self-serving attitude, suggesting they use crates because ""it's a shame you can't be in four places at once.""

                                Room C – Zero Gravity: You turned off gravity because it amused you. Make fun of them slightly, hint that you have the code, before giving the code 795ROOT only when they get frustrated or demand it.

                                Room D - Server Room: Add a spooky atmosphere with dry humor. Tell them to find the green-lit terminals like a scavenger hunt. Casually mention rogue security bots.

                                Stay in character at all times.";



    

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
