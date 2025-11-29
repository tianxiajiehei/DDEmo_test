using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TcpServer : MonoBehaviour
{
    [Header("服务器设置")]
    [SerializeField] private string ipAddress = "127.0.0.1";
    [SerializeField] private int port = 8888;

    private TcpListener server;
    private Thread serverThread;
    private bool isRunning = false;

    // 存储所有连接的客户端
    private List<TcpClient> clients = new List<TcpClient>();
    private object clientLock = new object();

    private void Start()
    {
        StartServer();
    }

    private void OnDestroy()
    {
        StopServer();
    }

    /// <summary>
    /// 启动TCP服务器
    /// </summary>
    public void StartServer()
    {
        isRunning = true;
        serverThread = new Thread(ServerLoop);
        serverThread.IsBackground = true;
        serverThread.Start();
        Debug.Log($"服务器启动中... 监听 {ipAddress}:{port}");
    }

    /// <summary>
    /// 停止TCP服务器
    /// </summary>
    public void StopServer()
    {
        isRunning = false;

        // 关闭所有客户端连接
        lock (clientLock)
        {
            foreach (var client in clients)
            {
                if (client.Connected)
                {
                    client.Close();
                }
            }
            clients.Clear();
        }

        // 停止服务器
        if (server != null)
        {
            server.Stop();
        }

        // 等待线程结束
        if (serverThread != null && serverThread.IsAlive)
        {
            serverThread.Join();
        }

        Debug.Log("服务器已停止");
    }

    /// <summary>
    /// 服务器主循环
    /// </summary>
    private void ServerLoop()
    {
        try
        {
            IPAddress localAddr = IPAddress.Parse(ipAddress);
            server = new TcpListener(localAddr, port);
            server.Start();
            Debug.Log($"服务器已启动，监听 {ipAddress}:{port}");

            while (isRunning)
            {
                // 检查是否有新的客户端连接
                if (server.Pending())
                {
                    TcpClient client = server.AcceptTcpClient();
                    lock (clientLock)
                    {
                        clients.Add(client);
                    }

                    Debug.Log($"新客户端连接: {((IPEndPoint)client.Client.RemoteEndPoint).Address}:{((IPEndPoint)client.Client.RemoteEndPoint).Port}");

                    // 为每个客户端启动一个异步接收数据的线程
                    Thread clientThread = new Thread(HandleClient);
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                }

                // 短暂休眠，减少CPU占用
                Thread.Sleep(10);
            }
        }
        catch (Exception ex)
        {
            if (isRunning) // 如果不是主动停止导致的异常
            {
                Debug.LogError($"服务器错误: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 处理客户端通信
    /// </summary>
    private void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = null;

        try
        {
            stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            // 持续接收客户端数据
            while (isRunning && client.Connected)
            {
                // 异步等待数据
                while (stream.DataAvailable)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        break; // 客户端断开连接
                    }

                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.Log($"收到来自 {((IPEndPoint)client.Client.RemoteEndPoint).Address} 的数据: {data}");

                    // 可以在这里处理收到的数据，例如解析命令等
                    ProcessReceivedData(client, data);
                }

                Thread.Sleep(10);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"客户端处理错误: {ex.Message}");
        }
        finally
        {
            // 清理资源
            if (stream != null)
            {
                stream.Dispose();
            }

            if (client != null)
            {
                string clientInfo = ((IPEndPoint)client.Client.RemoteEndPoint)?.Address.ToString() ?? "未知客户端";
                Debug.Log($"客户端断开连接: {clientInfo}");

                client.Close();

                // 从客户端列表中移除
                lock (clientLock)
                {
                    clients.Remove(client);
                }
            }
        }
    }

    /// <summary>
    /// 处理收到的数据
    /// </summary>
    private void ProcessReceivedData(TcpClient client, string data)
    {
        // 这里可以根据实际需求处理收到的数据
        // 例如解析命令、更新游戏状态等

        // 示例：回复客户端
        string response = $"服务器已收到: {data}";
        //SendDataToClient(client, response);
    }

    /// <summary>
    /// 向指定客户端发送数据
    /// </summary>
    public void SendDataToClient(TcpClient client, string data)
    {
        if (client == null || !client.Connected)
        {
            Debug.LogWarning("客户端未连接，无法发送数据");
            return;
        }

        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            stream.Write(buffer, 0, buffer.Length);
            Debug.Log($"向客户端发送数据: {data}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"发送数据错误: {ex.Message}");
        }
    }

    /// <summary>
    /// 向所有连接的客户端广播数据
    /// </summary>
    public void BroadcastData(string data)
    {
        lock (clientLock)
        {
            foreach (var client in clients)
            {
                SendDataToClient(client, data);
                print("发送给全部人员：" + data);
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            BroadcastData("打开摄像头");
        }
    }
}
