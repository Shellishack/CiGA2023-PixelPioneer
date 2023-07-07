using System.Collections;
using TMPro;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
	//Ψһʵ��
	public static GameManager instance;

	//һЩҪ������ȥ�Ķ���
	public GameObject player;

	//������ʾ���

	public TextMeshProUGUI selfTalk;
	public TextMeshProUGUI normalDialogue;
	private string currentText;
	private int currentTextLength;
	public TextMeshProUGUI sceneName;
	public GUISkin testSkin;
	private PlayableDirector playableDirector;

	public static GameManager Instance
	{
		get
		{
			//��ȡ����ʵ��ʱ���ʵ��Ϊ��
			if (instance == null)
			{
				//�����ڳ�����Ѱ���Ƿ�����object���ص�ǰ�ű�
				instance = FindObjectOfType<GameManager>();
				//���������û�й��ص�ǰ�ű���ô������һ���յ�gameobject�����ش˽ű�
				if (instance == null)
				{
					//���������������ڴ���ʱ���������Ͻű���Awake
					//���Դ�ʱ����Ϊ_instance��ֵ�������Awake�и�ֵ��
					new GameObject("singleton of " + typeof(GameManager)).AddComponent<GameManager>();
				}
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		playableDirector = GetComponent<PlayableDirector>();
	}

	/// <summary>
	/// ��ʾ�������Ƶ������
	/// <paramref name="name"/>��������
	/// </summary>
	public void ShowSceneName(string name)
	{
		sceneName.text = name;
		sceneName.gameObject.GetComponent<PlayableDirector>().Play();
	}

	/// <summary>
	/// ��ʾ������������ݵ������
	/// <paramref name="text"/>Ҫ��ʾ������
	/// <paramref name="existTime"/>���ڵ�ʱ��
	/// </summary>
	public void ShowSelfTalk(string text, float existTime)
	{
		selfTalk.text = text;
		selfTalk.gameObject.SetActive(true);
		selfTalk.gameObject.GetComponent<PlayableDirector>().Play();
		StopCoroutine("AutoHideSelfTalk");
		StartCoroutine("AutoHideSelfTalk", existTime);
	}

	public IEnumerator AutoHideSelfTalk(float time)
	{
		yield return new WaitForSeconds(time);
		selfTalk.gameObject.SetActive(false);
	}

	/// <summary>
	/// ��ʾ�Ի��������
	/// <paramref name="text"/>Ҫ��ʾ�ĶԻ�
	/// <paramref name="interval"/>���ֵ�ʱ����
	/// </summary>
	public void ShowNormalDialogue(string text, float interval)
	{
		normalDialogue.gameObject.SetActive(true);
		currentText = text;
		normalDialogue.text = "";
		StartCoroutine("ProcessNormalDialogue", interval);
	}

	public IEnumerator ProcessNormalDialogue(float interval)
	{
		normalDialogue.text = currentText.Substring(0, ++currentTextLength);
		yield return new WaitForSeconds(interval);
		if (currentTextLength != currentText.Length)
		{
			StartCoroutine("ProcessNormalDialogue", interval);
		}
		else
		{
			currentTextLength = 0;
			yield return new WaitForSeconds(2);
			normalDialogue.gameObject.SetActive(false);
		}
	}

	private void OnGUI()
	{
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(new GUIContent("��ͨ�Ի���ʾ����"), testSkin.button))
		{
			ShowNormalDialogue("����һ�δ����ĵĶԻ�", 0.05f);
		}
		if (GUILayout.Button(new GUIContent("����������ʾ����"), testSkin.button))
		{
			ShowSceneName(sceneName.text);
		}
		if (GUILayout.Button(new GUIContent("����������ʾ����"), testSkin.button))
		{
			ShowSelfTalk("������������Ŷ", 2);
		}
		GUILayout.EndHorizontal();
	}
}