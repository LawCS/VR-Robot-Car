using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text; // Required for encoding strings to bytes

public class CommandSender : MonoBehaviour
{
    private string url = "http://192.168.149.34:9030/command"; // The URL to which the commands are sent, including the port

    // Public method to initiate sending a command
    public void SendCommand(string command)
    {
        // Start the coroutine to send the command to the server
        StartCoroutine(PostCommand(command));
    }

    // Coroutine that handles sending the command via HTTP POST
    IEnumerator PostCommand(string command)
    {
        // Create a dictionary with the command to send
        var dict = new Dictionary<string, string> { { "command", command } };

        // Serialize the dictionary to a JSON string using a helper class
        string json = JsonUtility.ToJson(new SerializableDictionary(dict));

        // Log the JSON string to ensure it's correctly formatted
        Debug.Log("JSON being sent: " + json);

        // Setup the UnityWebRequest with the target URL and POST method
        var request = new UnityWebRequest(url, "POST");

        // Convert the JSON string into bytes for sending
        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);

        // Set the request's upload handler with the byte array
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);

        // Set up the download handler to process the response
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // Ensure the request includes the content type header as 'application/json'
        request.SetRequestHeader("Content-Type", "application/json");

        // Wait for the request to complete
        yield return request.SendWebRequest();

        // Check the result of the request and log the appropriate message
        if (request.result != UnityWebRequest.Result.Success)
        {
            // Log an error message if the request failed
            Debug.Log("Error sending command: " + request.error);
        }
        else
        {
            // Log a success message if the request was successful
            Debug.Log("Command sent successfully: " + command);
        }
    }

    // Helper class to facilitate serialization of the dictionary
    [System.Serializable]
    private class SerializableDictionary
    {
        public string command; // The command to be serialized

        // Constructor to set the command based on the provided dictionary
        public SerializableDictionary(Dictionary<string, string> dict)
        {
            this.command = dict["command"];
        }
    }
}
