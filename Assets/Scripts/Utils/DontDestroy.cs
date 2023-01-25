using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	// Use this for initialization
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
