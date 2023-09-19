using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using TMPro;

public class Receiver : MonoBehaviour
{
	[SerializeField] private GameObject otherPlayerPrefab = null;
	private Dictionary<int, GameObject> otherPlayersID = new Dictionary<int, GameObject>();
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
				continue;
			}

			byte[] bytes = new byte[512];
			NetClient.Instance.Socket.Receive(bytes);
			Bullet bullet = new Bullet();
			using (MemoryStream ms = new MemoryStream(bytes))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					bullet.position.x = br.ReadSingle();
					bullet.position.y = br.ReadSingle();
					bullet.position.z = br.ReadSingle();
				}
			}
			//Debug.Log($"Received : {bullet.position}");
			NetClient.Instance.Socket.Receive(bytes);
			int id = BitConverter.ToInt32(bytes, 0);

			if (otherPlayersID.ContainsKey(id) == false)
			{
				otherPlayersID.Add(id, Instantiate(otherPlayerPrefab, new Vector3(0, 0, 1f), Quaternion.identity));
				otherPlayersID[id].GetComponentInChildren<Canvas>().worldCamera = Camera.main;
				otherPlayersID[id].GetComponentInChildren<TextMeshProUGUI>().text = id.ToString();
			}
			otherPlayersID[id].transform.position = bullet.position;
		}
	}
}
