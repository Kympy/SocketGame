using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class NetClient : MonoSingleton<NetClient>
{
	public static int ClientID = 0;
	private string serverIP;
	private Socket socket = null;
	
	public Socket Socket
	{
		get { return socket; }
	}
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
		
		while(true)
		{
			if (socket.Poll(0, SelectMode.SelectRead) == false) continue;
			byte[] bytes = new byte[512];
			socket.Receive(bytes);
			ClientID = BitConverter.ToInt32(bytes, 0);
			Debug.Log($"Got unique ID : {ClientID}");
			//using (MemoryStream ms = new MemoryStream(bytes))
			//{
			//	using(BinaryReader br = new BinaryReader(ms))
			//	{
			//		ClientID = br.ReadInt32();
			//		Debug.Log($"Got unique ID : {ClientID}");
			//	}
			//}
			break;
		}
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
		Debug.Log($"Client send packet");
	}
	public Bullet ReceiveOtherBullets()
	{
		if (socket == null) return null;
		List<byte> buffer = new List<byte>();
		byte[] bytes = new byte[512];
		try
		{
			int read = socket.Receive(bytes);
			for(int i = 0; i < read; i++)
			{
				buffer.Add(bytes[i]);
			}
			if (buffer.Count > 0)
			{
				Debug.Log("Success!!");
				return null;
			}
		}
		catch(System.Exception e)
		{
			Debug.LogError(e.ToString());
			return null;
		}
		Debug.Log("Received.");
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
