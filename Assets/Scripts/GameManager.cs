using System.Collections;
using TMPro;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
	//一些要调来调取的东西
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
			//获取单例实例时如果实例为空
			if (_instance == null)
			{
				//首先在场景中寻找是否已有object挂载当前脚本
				_instance = FindObjectOfType<GameManager>();
				//如果场景中没有挂载当前脚本那么则生成一个空的gameobject并挂载此脚本
				if (_instance == null)
				{
					//如果创建对象，则会在创建时调用其身上脚本的Awake
					//所以此时无需为_instance赋值，其会在Awake中赋值。
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
		if (GUILayout.Button(new GUIContent("普通对话显示测试"), testSkin.button))
		{
			ShowNormalDialogue();
		}
		if (GUILayout.Button(new GUIContent("场景名称显示测试"), testSkin.button))
		{
			ShowSceneName();
		}
		if (GUILayout.Button(new GUIContent("自言自语显示测试"), testSkin.button))
		{
			ShowSelfTalk();
		}
		GUILayout.EndHorizontal();
	}
}