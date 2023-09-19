using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static volatile T instance;
	private static Object lockObject = new Object();
	private static bool isShuttingDown = false;
	public static T Instance
	{
		get
		{
			if (isShuttingDown == true) return null;

			lock (lockObject)
			{
				if (instance == null)
				{
					instance = FindObjectOfType<T>();
				}
				if (instance == null)
				{
					instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
				}
			}
			return instance;
		}
	}
	protected virtual void Awake()
	{
		if (instance != null)
		{
			DestroyImmediate(this);
			return;
		}
		instance = this as T;
		DontDestroyOnLoad(this);
	}
	protected virtual void OnApplicationQuit()
	{
		isShuttingDown = true;
		instance = null;
	}
}

