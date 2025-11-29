using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPClient : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;
    private bool isConnected = false;

    [Header("Connection Settings")]
    public string serverIP = "127.0.0.1";
    public int serverPort = 8080;

    [Header("Message Settings")]
    public string messageToSend = "Hello Server!";
    public float sendInterval = 1f;
    private float timer = 0f;

    void Start()
    {
        ConnectToServer();
    }

    void Update()
    {
        // 定时发送消息示例
        //if (isConnected)
        //{
        //    timer += Time.deltaTime;
        //    if (timer >= sendInterval)
        //    {
        //        SendMessageToServer(messageToSend);
        //        timer = 0f;
        //    }
        //}
    }

    void OnDestroy()
    {
        Disconnect();
    }

    public void ConnectToServer()
    {
        try
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse(serverIP), serverPort);
            stream = client.GetStream();
            isConnected = true;

            // 启动接收线程
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Debug.Log("Connected to server at " + serverIP + ":" + serverPort);
        }
        catch (Exception e)
        {
            Debug.LogError("Connection error: " + e.Message);
        }
    }

    private void ReceiveData()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        while (isConnected)
        {
            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    // 连接已关闭
                    Disconnect();
                    return;
                }

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("Received from server: " + receivedMessage);

                // 在主线程处理消息
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    HandleReceivedMessage(receivedMessage);
                });
            }
            catch (Exception e)
            {
                if (isConnected) // 只在连接状态下报告错误
                {
                    Debug.LogError("Receive error: " + e.Message);
                    Disconnect();
                }
                return;
            }
        }
    }

    public void SendMessageToServer(string message)
    {
        if (!isConnected || stream == null)
        {
            Debug.LogWarning("Not connected to server.");
            return;
        }

        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Debug.Log("Sent to server: " + message);
        }
        catch (Exception e)
        {
            Debug.LogError("Send error: " + e.Message);
            Disconnect();
        }
    }

    private void HandleReceivedMessage(string message)
    {
        SpeechSpeak speechSpeak = this.GetComponent<SpeechSpeak>();
        speechSpeak.ReciveScore(message);
        // 在这里处理接收到的消息
        // 例如更新UI、触发游戏事件等
        Debug.Log("Processing message: " + message);
    }

    public void Disconnect()
    {
        if (!isConnected) return;

        isConnected = false;

        try
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }

            if (client != null)
            {
                client.Close();
                client = null;
            }

            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Abort();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Disconnect error: " + e.Message);
        }

        Debug.Log("Disconnected from server");
    }
}