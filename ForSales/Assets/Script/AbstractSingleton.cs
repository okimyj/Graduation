using UnityEngine;
using System.Collections;

public class AbstractSingleton<Type> : MonoBehaviour
{
	public const string APP_OBJECT_NAME = "Application";
	private static Type _instance;
	public static Type Instance
	{
		get
		{
			if (_instance == null)
			{
				//Debug.Log ("AbstractSingleton " + typeof(Type) + "," + UnityEngine.StackTraceUtility.ExtractStackTrace());
				_instance = (Type)(object)FindObjectOfType(typeof(Type));
				if (_instance == null)
				{
					GameObject application = GameObject.Find(APP_OBJECT_NAME);
					if (application != null)
						_instance = (Type)(object)application.AddComponent(typeof(Type));
					else
					{
						_instance = (Type)(object)new GameObject(APP_OBJECT_NAME).AddComponent(typeof(Type));

					}
				}
				((AbstractSingleton<Type>)(object)_instance)._Init();
			}

			return _instance;
		}
	}

	static public bool HasInstance()
	{
		return _instance != null;
	}
	virtual public void _Init()
	{
	}
}
