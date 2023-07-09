using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
			iObject = value == null ? iObject : value;
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

	public AudioSource selfTalkAudio;
	public AudioSource dialogueAudio;

	[HideInInspector]
	public bool playerInDeadZone;

	public GameObject CameraConfiners;
	public int confinerIndex = 0;
	public GameObject activeConfiner;
	public GameObject targetConfiner;

	public Image transition;
	public bool nextSceneState = false;

	public bool isSecondLevel = false;

	public Canvas Canvas;

	public SpriteRenderer element;

	public AudioSource fireBg;
	public AudioSource fireExit;

	public Sprite fireElement;

	public GameObject CG;

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
		//DontDestroyOnLoad(Canvas.gameObject);
		instance = this;
		playableDirector = GetComponent<PlayableDirector>();
		activeConfiner = CameraConfiners.transform.GetChild(0).gameObject;
		Debug.Log(activeConfiner.name);
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
				player.GetComponent<PlayerController>().isAllowPlayerAction = true;
			}
		}
		if (nextSceneState)
		{
			transition.color = new Color(transition.color.r, transition.color.g, transition.color.b, transition.color.a + Time.deltaTime / 2);
			//fsbg.volume -= 0.008f;
			if (SceneManager.GetActiveScene().name == "������")
			{
				GameManager.instance.GetComponents<AudioSource>()[1].volume -= Time.deltaTime / 4;
			}
			if (transition.color.a >= 1)
			{
				if (SceneManager.GetActiveScene().name == "��һ��") SceneManager.LoadScene("�ڶ���");
				if (SceneManager.GetActiveScene().name == "�ڶ���") SceneManager.LoadScene("������");
				if (SceneManager.GetActiveScene().name == "������") SceneManager.LoadScene("END");
			}
		}
		if (SceneManager.GetActiveScene().name == "�ڶ���" && !isSecondLevel)
		{
			//ssbg.volume += 0.008f;
			player.GetComponent<PlayerController>().absorbedElement = ElementEnum.Water;
			transition.color = new Color(transition.color.r, transition.color.g, transition.color.b, transition.color.a - Time.deltaTime / 2);
			if (transition.color.a <= 0)
			{
				isSecondLevel = true;
				GameManager.instance.ShowSceneName("-ɭ���в�-");
				GameManager.instance.ShowSelfTalk("w3=��ô������ô��Ļ�!");
			}
		}
		if (SceneManager.GetActiveScene().name == "������" && !isSecondLevel)
		{
			player.GetComponent<PlayerController>().isAllowPlayerAction = false;
			player.GetComponent<PlayerController>().faceDirction = 1;
			player.GetComponent<PlayerController>().moveSpeed = 100f;
			fireBg.Pause();
			//ssbg.volume += 0.008f;
			//player.GetComponent<PlayerController>().absorbedElement = ElementEnum.Water;
			transition.color = new Color(transition.color.r, transition.color.g, transition.color.b, transition.color.a - Time.deltaTime / 2);
			if (transition.color.a <= 0)
			{
				isSecondLevel = true;
				player.GetComponent<PlayerController>().moveState = true;
				GameManager.instance.ShowSceneName("-ɭ���-");
				GameManager.instance.ShowSelfTalk("w3=è��!......è�䣡");
				GameManager.instance.ShowSelfTalk("w6=è��!......è�䣡");
				GameManager.instance.ShowSelfTalk("w9=è��!......è�䣡");
			}
		}
		if (SceneManager.GetActiveScene().name == "END" && !isSecondLevel)
		{
			transition.color = new Color(transition.color.r, transition.color.g, transition.color.b, transition.color.a - Time.deltaTime / 2);
			if (transition.color.a <= 0)
			{
				isSecondLevel = true;
				CG.gameObject.SetActive(true);
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
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
			text = text.Remove(0, 1);
			aoutSelfTalk = true;
			GameManager.instance.player.GetComponent<PlayerController>().isAllowPlayerAction = false;
			GameManager.instance.player.GetComponent<PlayerController>().moveState = false;
			iObject.descriptions.currentIndex++;
		}
		//����һЩָ�����ƽ����� a���Ŷ���
		if (text.FirstOrDefault() == 'a')
		{
			text = text.Remove(0, 1);
			if (text.FirstOrDefault() == 'p')
			{
				text = text.Remove(0, 2);
				player.GetComponent<Animator>().SetTrigger(text);
			}
			else
			{
				text = text.Remove(0, 1);
				iObject.GetComponent<Animator>().SetTrigger(text);
			}
			if (!aoutSelfTalk) iObject.descriptions.currentIndex++;
			iObject.showDescription = false;//���Ŷ�����ʱ���ǲ���������
			return;
		}
		//ͨ��C#�ķ�����Ƶ��÷���
		else if (text.FirstOrDefault() == 'f')
		{
			string funName = text.Remove(0, 2);
			this.GetType().GetMethod(funName)?.Invoke(this, null);
			if (!aoutSelfTalk) iObject.descriptions.currentIndex++;
			return;
		}
		//�ȴ���������...
		else if (text.FirstOrDefault() == 'w')
		{
			text = text.Remove(0, 1);
			isSelfTalking = true;
			//dialogue��ʱ������������ƶ���
			//GameManager.instance.player.GetComponent<PlayerController>().isAllowPlayerAction = true;
			//GameManager.instance.player.GetComponent<PlayerController>().moveState = false;
			StartCoroutine("WaitTimeThenDoSomething", text);
			if (!aoutSelfTalk && iObject != null) iObject.descriptions.currentIndex++;
			return;
		}
		selfTalkAudio.Play();
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
		isSelfTalking = false;
		selfTalk.gameObject.SetActive(false);
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
			dialogueAudio.Play();
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
		//GUILayout.BeginHorizontal();
		//if (GUILayout.Button(new GUIContent("��ͨ�Ի���ʾ����"), testSkin.button))
		//{
		//	ShowNormalDialogue("����һ�δ����ĵĶԻ�", 0.05f);
		//}
		//if (GUILayout.Button(new GUIContent("����������ʾ����"), testSkin.button))
		//{
		//	ShowSceneName(sceneName.text);
		//}
		//if (GUILayout.Button(new GUIContent("����������ʾ����"), testSkin.button))
		//{
		//	ShowSelfTalk("������������Ŷ");
		//}
		//GUILayout.EndHorizontal();
	}

	public void ChnageCameraConfiner()
	{
		++confinerIndex;
		//Debug.Log("ChnageCameraConfiner");
		targetConfiner = CameraConfiners.transform.GetChild(confinerIndex).gameObject;
		activeConfiner.gameObject.transform.position = targetConfiner.gameObject.transform.position;
		//Debug.Log(targetConfiner.name);
		//StartCoroutine("MoveToNextPoint");
	}

	public IEnumerator WaitTimeThenDoSomething(string text)
	{
		//Debug.Log(text.FirstOrDefault());
		float waitTime = Convert.ToSingle(text.FirstOrDefault().ToString());
		text = text.Remove(0, 2);
		yield return new WaitForSeconds(waitTime);
		//iObject =
		if (text != "") ShowNormalDialogue(text, 0.05f);
		isSelfTalking = false;
	}

	public void NextScene()
	{
		nextSceneState = true;
	}

	public void NextScene3()
	{
		nextSceneState = true;
	}

	public void PlayCG()
	{
		nextSceneState = true;
		Debug.Log("asdasd");
	}
}