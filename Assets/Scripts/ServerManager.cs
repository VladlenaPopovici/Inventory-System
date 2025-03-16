using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{
    private static ServerManager instance;
    public static ServerManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendDataToServer(string itemId, string eventType)
    {
        StartCoroutine(PostRequest(itemId, eventType));
    }

    private IEnumerator PostRequest(string itemId, string eventType)
    {
        string jsonData = $"{{\"item_id\":\"{itemId}\",\"event_type\":\"{eventType}\"}}";

        UnityWebRequest request = new UnityWebRequest("https://wadahub.manerai.com/api/inventory/status", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data sent successfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error sending data: " + request.error);
        }
    }
}
