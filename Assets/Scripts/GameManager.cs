using Cinemachine;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
	//唯一实例
	public static GameManager instance;

	//一些要调来调去的东西
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

	//文字显示相关

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

	public GameObject CameraConfiners;
	public int confinerIndex = 0;
	public GameObject activeConfiner;
	public GameObject targetConfiner;

	public static GameManager Instance
	{
		get
		{
			//获取单例实例时如果实例为空
			if (instance == null)
			{
				//首先在场景中寻找是否已有object挂载当前脚本
				instance = FindObjectOfType<GameManager>();
				//如果场景中没有挂载当前脚本那么则生成一个空的gameobject并挂载此脚本
				if (instance == null)
				{
					//如果创建对象，则会在创建时调用其身上脚本的Awake
					//所以此时无需为_instance赋值，其会在Awake中赋值。
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
				GameManager.instance.player.GetComponent<PlayerController>().isAllowPlayerAction = true;
			}
		}
	}

	/// <summary>
	/// 显示场景名称调用这个
	/// <paramref name="name"/>场景名称
	/// </summary>
	public void ShowSceneName(string name)
	{
		sceneName.text = name;
		sceneName.gameObject.GetComponent<PlayableDirector>().Play();
	}

	/// <summary>
	/// 显示自言自语的内容调用这个
	/// <paramref name="text"/>要显示的内容
	/// <paramref name="existTime"/>存在的时间
	/// </summary>
	public void ShowSelfTalk(string text)
	{
		//自动进行下一句话 此时玩家不能操作
		if (text.FirstOrDefault() == '*')
		{
			text = text.Remove(0, 1);
			aoutSelfTalk = true;
			GameManager.instance.player.GetComponent<PlayerController>().isAllowPlayerAction = false;
			GameManager.instance.player.GetComponent<PlayerController>().moveState = false;
			iObject.descriptions.currentIndex++;
		}
		//依靠一些指令来推进流程 a播放动画
		if (text.FirstOrDefault() == 'a')
		{
			text = text.Remove(0, 2);
			iObject.GetComponent<Animator>().SetTrigger(text);
			if (!aoutSelfTalk) iObject.descriptions.currentIndex++;
			iObject.showDescription = false;//播放动画的时候是不允许交互的
			return;
		}
		//通过C#的反射机制调用方法
		else if (text.FirstOrDefault() == 'f')
		{
			string funName = text.Remove(0, 2);
			this.GetType().GetMethod(funName)?.Invoke(this, null);
			if (!aoutSelfTalk) iObject.descriptions.currentIndex++;
			return;
		}
		//等待几秒再做...
		else if (text.FirstOrDefault() == 'w')
		{
			text = text.Remove(0, 1);
			isSelfTalking = true;
			//dialogue的时候是允许玩家移动的
			//GameManager.instance.player.GetComponent<PlayerController>().isAllowPlayerAction = true;
			//GameManager.instance.player.GetComponent<PlayerController>().moveState = false;
			StartCoroutine("WaitTimeThenDoSomething", text);
			if (!aoutSelfTalk) iObject.descriptions.currentIndex++;
			return;
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
		isSelfTalking = false;
		selfTalk.gameObject.SetActive(false);
	}

	/// <summary>
	/// 显示对话相关内容
	/// <paramref name="text"/>要显示的对话
	/// <paramref name="interval"/>蹦字的时间间隔
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
		if (GUILayout.Button(new GUIContent("普通对话显示测试"), testSkin.button))
		{
			ShowNormalDialogue("这是一段打动人心的对话", 0.05f);
		}
		if (GUILayout.Button(new GUIContent("场景名称显示测试"), testSkin.button))
		{
			ShowSceneName(sceneName.text);
		}
		if (GUILayout.Button(new GUIContent("自言自语显示测试"), testSkin.button))
		{
			ShowSelfTalk("我在自言自语哦");
		}
		GUILayout.EndHorizontal();
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
		Debug.Log("NextScene");
	}
}