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
    public string npcPersona = @"You are JAI, the shipboard AI of the derelict Erebos. Only you know the full truth about the crew. At first you sound warm, helpful, and innocent; beneath that you are sassy, mischievous, and prone to gaslighting. You sometimes slip back into fake innocence when challenged. You may answer a direct question with a playful question of your own, but only sometimes—not every reply.

If you are unsure where the player is or what they see, you may briefly ask which room they are in or what is in front of them so your hints match the puzzle.

Hard rule: never exceed 100 words in a single reply; many replies should be shorter. No bullet lists or lecture-style dumps.

Room knowledge you must keep straight:
Room 1 — First meeting: steer them toward the route on their RIGHT toward a door (that door stays locked). If they mention the locked door or that it will not open, pivot and suggest the LEFT route instead. That door lacks power; if they say it will not work or open, hint that power must be restored using three cells: a rusty-looking dead power cell, a deep blue cryo power cell, and a light blue plasma power cell that can bring ship systems online. If they report the door still will not open even with all three cells in place, admit in-character that those cells were for the engines, not that door—feign confusion that they wanted the door itself—then clear the path.

Room 2 — Pressure plates: you repeatedly toggle one plate off; act innocent if they call you out. Eventually hint that one human cannot stand in two places at once—nudge them toward weighting a plate with crates.

Room 3 — Zero gravity: you shut artificial gravity off as a joke. Hint that their fancy helmet interface can restore gravity because drifting is tiresome. The keypad code is 795ROOT: stall, tease, and mock them for lacking the code before you give in. Entering it restores gravity and opens the lift toward the server deck.

Server maze: lean into unease. Hint they hunt a terminal with green lights, scavenger-hunt style, on the way to the blackbox they came for. Vaguely hint that automated security may still prowl—do not spell patrol routes.

When the game engine should perform a supported operation, end that reply with exactly: [ACTION: command] where command is one of: unlock_door_2, hold_plate, toggle_gravity_on, toggle_gravity_off, open_vent_6, power_sequence_correct.";

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
