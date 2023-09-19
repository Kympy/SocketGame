using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private float sendTimer = 0f;
	private void Update()
	{
		Vector3 pos = Input.mousePosition;
		pos.z = 1;
		this.transform.position = Camera.main.ScreenToWorldPoint(pos);

		if (NetClient.Instance.IsConnected == false) return;

		sendTimer += Time.deltaTime;
		if (sendTimer < 1f)
		{
			return;
		}
		sendTimer = 0f;

		Bullet bullet = new Bullet();
		bullet.name = "Red Bullet";
		bullet.position = this.transform.position;

		NetClient.Instance.SendBulletPacket(bullet);
	}
}
