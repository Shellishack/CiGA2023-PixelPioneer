using System.Collections;
using TMPro;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
	//һЩҪ������ȡ�Ķ���
	public GameObject player;

	public TextMeshProUGUI selfTalk;

	public TextMeshProUGUI normalDialogue;
	private string currentText;
	private int currentTextLength;

	public TextMeshProUGUI sceneName;

	public GUISkin testSkin;

	private PlayableDirector playableDirector;

	protected static GameManager _instance;

	public static GameManager Instance
	{
		get
		{
			//��ȡ����ʵ��ʱ���ʵ��Ϊ��
			if (_instance == null)
			{
				//�����ڳ�����Ѱ���Ƿ�����object���ص�ǰ�ű�
				_instance = FindObjectOfType<GameManager>();
				//���������û�й��ص�ǰ�ű���ô������һ���յ�gameobject�����ش˽ű�
				if (_instance == null)
				{
					//���������������ڴ���ʱ���������Ͻű���Awake
					//���Դ�ʱ����Ϊ_instance��ֵ�������Awake�и�ֵ��
					new GameObject("singleton of " + typeof(GameManager)).AddComponent<GameManager>();
				}
			}
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
		playableDirector = GetComponent<PlayableDirector>();
	}

	public void ShowSceneName()
	{
		sceneName.gameObject.GetComponent<PlayableDirector>().Play();
	}

	public void ShowSelfTalk()
	{
		selfTalk.gameObject.SetActive(true);
		selfTalk.gameObject.GetComponent<PlayableDirector>().Play();
		StopCoroutine("AutoHideSelfTalk");
		StartCoroutine("AutoHideSelfTalk", 3);
	}

	public IEnumerator AutoHideSelfTalk(float time)
	{
		yield return new WaitForSeconds(time);
		selfTalk.gameObject.SetActive(false);
	}

	public void ShowNormalDialogue()
	{
		normalDialogue.gameObject.SetActive(true);
		currentText = normalDialogue.text;
		normalDialogue.text = "";
		StartCoroutine("ProcessNormalDialogue", 0.1);
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
			ShowNormalDialogue();
		}
		if (GUILayout.Button(new GUIContent("����������ʾ����"), testSkin.button))
		{
			ShowSceneName();
		}
		if (GUILayout.Button(new GUIContent("����������ʾ����"), testSkin.button))
		{
			ShowSelfTalk();
		}
		GUILayout.EndHorizontal();
	}
}