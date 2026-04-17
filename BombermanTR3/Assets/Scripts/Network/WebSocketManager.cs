using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

public class WebSocketManager : MonoBehaviour
{
    public static WebSocketManager Instance { get; private set; }

    private ClientWebSocket ws;
    private CancellationTokenSource cts;
    private Queue<Action> mainThreadQueue = new Queue<Action>();

    public event Action<string, string> OnMissatgeRebut;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        while (mainThreadQueue.Count > 0)
        {
            mainThreadQueue.Dequeue().Invoke();
        }
    }

    public async Task ConnectAsync(string codiSala)
    {
        try
        {
            ws = new ClientWebSocket();
            cts = new CancellationTokenSource();
            Uri serverUri = new Uri("ws://localhost:8080/ws");

            await ws.ConnectAsync(serverUri, cts.Token);

            string msg = "{\"tipus\":\"unir_sala\",\"codiSala\":\"" + codiSala + "\"}";
            await SendMessageRaw(msg);

            _ = StartReceiving();
        }
        catch (Exception)
        {
        }
    }

    public async void SendMessage(string tipus, string jsonData)
    {
        try
        {
            string fullJson = jsonData;
            if (string.IsNullOrEmpty(jsonData) || jsonData == "{}")
            {
                fullJson = "{\"tipus\":\"" + tipus + "\"}";
            }
            else
            {
                fullJson = jsonData.Replace("{", "{\"tipus\":\"" + tipus + "\",");
            }

            await SendMessageRaw(fullJson);
        }
        catch (Exception)
        {
        }
    }

    private async Task SendMessageRaw(string message)
    {
        if (ws == null || ws.State != WebSocketState.Open) return;

        byte[] bytes = Encoding.UTF8.GetBytes(message);
        await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cts.Token);
    }

    private async Task StartReceiving()
    {
        byte[] buffer = new byte[1024 * 4];

        try
        {
            while (ws.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cts.Token);
                }
                else
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    ProcessMessage(message);
                }
            }
        }
        catch (Exception)
        {
        }
    }

    private void ProcessMessage(string json)
    {
        try
        {
            BaseMessage msg = JsonUtility.FromJson<BaseMessage>(json);
            mainThreadQueue.Enqueue(() => OnMissatgeRebut?.Invoke(msg.tipus, json));
        }
        catch (Exception)
        {
        }
    }

    public async void Disconnect()
    {
        try
        {
            if (ws != null)
            {
                cts.Cancel();
                
                if (ws.State == WebSocketState.Open || ws.State == WebSocketState.CloseReceived || ws.State == WebSocketState.CloseSent)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Tancant sessió", CancellationToken.None);
                }
                
                ws.Dispose();
                ws = null;
            }
        }
        catch (Exception)
        {
        }
    }

    [Serializable]
    private class BaseMessage { public string tipus; }
}
