using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using TMPro;

public class NetClient : MonoSingleton<NetClient>
{
	public static int ClientID = 0;
	private string serverIP;
	private Socket socket = null;
	[SerializeField] private TextMeshProUGUI idUI = null;	
	
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
		Application.runInBackground = true;
		Screen.SetResolution(1366, 768, false);
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
			idUI.text = ClientID.ToString();
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
		//byte[] bufferSize = new byte[1];
		//bufferSize[0] = (byte)sendData.Length;
		//socket.Send(bufferSize);
		socket.Send(sendData);
		Debug.Log($"Client send packet");
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
