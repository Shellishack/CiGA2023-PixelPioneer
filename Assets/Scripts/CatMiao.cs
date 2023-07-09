using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatMiao : MonoBehaviour
{
	public void miao()
	{
		GameManager.instance.GetComponents<AudioSource>().Last().Play();
	}
}