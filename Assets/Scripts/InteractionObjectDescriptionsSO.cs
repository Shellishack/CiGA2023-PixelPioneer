using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//��ScriptableObject���洢
//����ֱ���Ҽ�Create->ScriptableObject�´����µĶ���
[CreateAssetMenu(menuName = "ScriptableObject/InteractionObjectDescriptions", fileName = "IOD_")]
public class InteractionObjectDescriptionsSO : ScriptableObject
{
	public int currentIndex;
	public List<string> descriptions = new List<string>();

	public string GetCurrentDescription()
	{
		return descriptions[currentIndex];
	}

	private void OnValidate()
	{
		currentIndex = Mathf.Clamp(currentIndex, 0, descriptions.Count);
	}
}