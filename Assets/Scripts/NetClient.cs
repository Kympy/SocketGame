using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEditor;

public class NetClient : MonoSingleton<NetClient>
{
	private string serverIP;
	private Socket socket = null;
	public bool IsConnected
	{
		get
		{
			if (socket == null) return false;
			return socket.Connected;
		}
	}
	private void Start()
	{
		CreateSocket(ServerConfig.LocalServerIP);
	}
	public void CreateSocket(string argServerIP)
	{
		socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		serverIP = argServerIP;
		IPAddress serverIPAddress = IPAddress.Parse(serverIP);
		IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, ServerConfig.PortNumber);

		try
		{
			socket.Connect(serverEndPoint);
			Debug.Log("Connecting...");
		}
		catch (System.Exception e)
		{
			Debug.LogError(e.ToString());
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
			return;
		}
		Debug.Log("Client Socket Created and connected");
	}
	public void SendBulletPacket(Bullet bulletPacket)
	{
		if (socket == null)
		{
			Debug.LogWarning("Client socket is NULL.");
			CreateSocket(serverIP);
			return;
		}
		byte[] sendData = Bullet.ToByteArray(bulletPacket);
		byte[] bufferSize = new byte[1];
		bufferSize[0] = (byte)sendData.Length;
		socket.Send(bufferSize);
		socket.Send(sendData);
		Debug.Log($"Client send packet {bulletPacket.name}");
	}
	public Bullet ReceiveOtherBullets()
	{
		byte[] bytes = new byte[512];
		socket.Receive(bytes);
		
		Bullet bullet = Bullet.FromByteArray(bytes);
		return bullet;
	}
	public void SetIP(string targetIP)
	{
		serverIP = targetIP;
	}
	protected override void OnApplicationQuit()
	{
		base.OnApplicationQuit();
		if (socket != null)
		{
			socket.Close();
			socket = null;
		}
		Debug.LogWarning("Socket Closed");
	}
}
