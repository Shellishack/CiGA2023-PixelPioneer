using System.Collections;
using System.Linq;
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

	private InteractableObject iObject;
	//private InteractableObject autoiObject;

	public InteractableObject currentInteractableObject
	{
		get { return iObject; }
		set
		{
			iObject = value;
			if (value != null && value.showDescription)
			{
				ShowSelfTalk(iObject.descriptions.GetCurrentDescription());
			}
		}
	}

	//������ʾ���

	public TextMeshProUGUI selfTalk;
	public bool aoutSelfTalk = false;
	public bool isSelfTalking = false;
	public TextMeshProUGUI normalDialogue;
	private string currentText;
	private int currentTextLength;
	public TextMeshProUGUI sceneName;
	public GUISkin testSkin;
	private PlayableDirector playableDirector;
	private float dialogueExistTime = 0;

	[HideInInspector]
	public bool playerInDeadZone;

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

	private void Update()
	{
		if (playerInDeadZone)
		{
			Debug.Log("Player is dead!");
			playerInDeadZone = false;
		}
		if (aoutSelfTalk && !isSelfTalking)
		{
			aoutSelfTalk = false;
			ShowSelfTalk(iObject.descriptions.GetCurrentDescription());
			if (!aoutSelfTalk)
			{
				GameManager.instance.player.GetComponent<PlayerController>().isAllowPlayerAction = true;
			}
		}
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
	public void ShowSelfTalk(string text)
	{
		//�Զ�������һ�仰 ��ʱ��Ҳ��ܲ���
		if (text.FirstOrDefault() == '*')
		{
			text.Remove(0, 1);
			aoutSelfTalk = true;
			GameManager.instance.player.GetComponent<PlayerController>().isAllowPlayerAction = false;
			GameManager.instance.player.GetComponent<PlayerController>().moveState = false;
			iObject.descriptions.currentIndex++;
		}
		isSelfTalking = true;
		selfTalk.text = text;
		selfTalk.gameObject.SetActive(true);
		selfTalk.gameObject.GetComponent<PlayableDirector>().Play();
		StopCoroutine("AutoHideSelfTalk");
		StartCoroutine("AutoHideSelfTalk", selfTalk.gameObject.GetComponent<PlayableDirector>().playableAsset.duration);
	}

	public IEnumerator AutoHideSelfTalk(float time)
	{
		yield return new WaitForSeconds(time);
		selfTalk.gameObject.SetActive(false);
		isSelfTalking = false;
	}

	/// <summary>
	/// ��ʾ�Ի��������
	/// <paramref name="text"/>Ҫ��ʾ�ĶԻ�
	/// <paramref name="interval"/>���ֵ�ʱ����
	/// </summary>
	public void ShowNormalDialogue(string text, float interval, float existTime = 1)
	{
		normalDialogue.gameObject.SetActive(true);
		currentText = text;
		normalDialogue.text = "";
		currentTextLength = 0;
		dialogueExistTime = existTime;
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
			yield return new WaitForSeconds(dialogueExistTime);
			//normalDialogue.gameObject.SetActive(false);
			StartCoroutine("ProcessDialogueDisappear", interval);
		}
	}

	public IEnumerator ProcessDialogueDisappear(float interval)
	{
		normalDialogue.text = currentText.Substring(0, --currentTextLength);
		yield return new WaitForSeconds(interval / 4);
		if (currentTextLength != 0)
		{
			StartCoroutine("ProcessDialogueDisappear", interval);
		}
		else { normalDialogue.gameObject.SetActive(false); }
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
			ShowSelfTalk("������������Ŷ");
		}
		GUILayout.EndHorizontal();
	}
}