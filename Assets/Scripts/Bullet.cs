using System.IO;
using UnityEngine;

[System.Serializable]
public class Bullet
{
	public string name;
	public Vector3 position;

	public static Bullet FromByteArray(byte[] input)
	{
		using(MemoryStream ms = new MemoryStream(input))
		{
			using(BinaryReader br = new BinaryReader(ms))
			{

				Bullet bullet = new Bullet();
				
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
				bw.Write(bullet.position.x);
				bw.Write(bullet.position.y);
				bw.Write(bullet.position.z);

				return ms.ToArray();
			}
		}
	}
}
