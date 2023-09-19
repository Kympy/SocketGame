using UnityEngine;

public class Receiver : MonoBehaviour
{
	[SerializeField] private GameObject otherPlayerPrefab = null;
	private GameObject otherPlayer = null;
	private void Update()
	{
		if (NetClient.Instance.IsConnected == false) return;

		//Bullet bullet = NetClient.Instance.ReceiveOtherBullets();

		//if (bullet == null) return;

		//if (otherPlayer == null)
		//{
		//	Instantiate(otherPlayerPrefab);
		//}
		//otherPlayer.transform.position = bullet.position;
	}
}
