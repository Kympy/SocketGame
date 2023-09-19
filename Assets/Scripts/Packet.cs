
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Packet
{
	protected MemoryStream stream;
	protected BinaryFormatter formatter;

	public string name = "";
	public virtual byte[] ToByteArray() { return null; }
	public virtual T FromByteArray<T>(byte[] input) where T : class, new() { return null; }
}
