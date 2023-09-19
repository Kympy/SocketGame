using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private const string Horizontal = "Horizontal";
	private const string Vertical = "Vertical";
	private float sendTimer = 0f;
	private void Update()
	{
		if (Application.isFocused == false) return;

		float horizontal = Input.GetAxis(Horizontal);
		float vertical = Input.GetAxis(Vertical);

		transform.position += new Vector3(horizontal, vertical) * Time.deltaTime;

		if (NetClient.Instance.IsConnected == false) return;

		//sendTimer += Time.deltaTime;
		//if (sendTimer < 1f)
		//{
		//	return;
		//}
		//sendTimer = 0f;

		Bullet bullet = new Bullet();
		bullet.position = this.transform.position;

		NetClient.Instance.SendBulletPacket(bullet);
	}
}
