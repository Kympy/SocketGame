using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class Receiver : MonoBehaviour
{
	[SerializeField] private GameObject otherPlayerPrefab = null;
	private GameObject otherPlayer = null;

	private void Start()
	{
		StartCoroutine(ReceiveBullet());
	}
	private void Update()
	{
		//if (NetClient.Instance.IsConnected == false) return;

		//Bullet bullet = NetClient.Instance.ReceiveOtherBullets();

		//if (bullet == null) return;

		//if (otherPlayer == null)
		//{
		//	Instantiate(otherPlayerPrefab);
		//}
		//otherPlayer.transform.position = bullet.position;
	}
	public IEnumerator ReceiveBullet()
	{
		while(true)
		{
			yield return null;
			if (NetClient.Instance.IsConnected == false) yield return null;
			if (NetClient.Instance.Socket.Poll(0, SelectMode.SelectRead) == false)
			{
				//Debug.Log("Received nothing");
				continue;
			}

			List<byte> buffer = new List<byte>();
			byte[] bytes = new byte[512];

			int read = NetClient.Instance.Socket.Receive(bytes);
			for (int i = 0; i < read; i++)
			{
				buffer.Add(bytes[i]);
			}
			if (buffer.Count > 0)
			{
				Debug.Log("Received.");
				continue;
				Bullet bullet = Bullet.FromByteArray(bytes);
			}
		}
	}
}
