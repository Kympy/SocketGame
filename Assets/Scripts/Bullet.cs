using System.IO;
using UnityEngine;

[System.Serializable]
public class Bullet
{
	public string name;
	public Vector3 position;

	public Bullet()
	{
		name = NetClient.Instance.Socket.GetHashCode().ToString();
	}
	public static Bullet FromByteArray(byte[] input)
	{
		if (input == null || input.Length == 0)
		{
			Debug.LogWarning("Input is NULL.");
			return null;
		}
		using(MemoryStream ms = new MemoryStream(input))
		{
			using(BinaryReader br = new BinaryReader(ms))
			{

				Bullet bullet = new Bullet();
				
				bullet.name = br.ReadString();
				bullet.position.x = br.ReadSingle();
				bullet.position.y = br.ReadSingle();
				bullet.position.z = br.ReadSingle();

				return bullet;
			}
		}
	}

	public static byte[] ToByteArray(Bullet bullet)
	{
		using (MemoryStream ms = new MemoryStream())
		{
			using (BinaryWriter bw = new BinaryWriter(ms))
			{
				bw.Write(bullet.name);
				bw.Write(bullet.position.x);
				bw.Write(bullet.position.y);
				bw.Write(bullet.position.z);

				return ms.ToArray();
			}
		}
	}
}
