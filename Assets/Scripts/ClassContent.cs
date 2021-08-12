using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class ClassContent : MonoBehaviour
{

	public static ClassContent sharedInstance;

	[Serializable]
	public class Questions
	{
		public string name;
		public string question;
		public string answerTrue;
		public string answerFalse01;
		public string answerFalse02;
		public string answerFalse03;
	}

	[Serializable]
	public class Content
	{
		public string name;
		public string description;
	}

	public TMP_Text textNameContent;
	public TMP_Text textDescriptionContent;

	private int index = 0;
	private int contentCount;

	[Header("JSON")]
	public string fileName = "Questions";
	public Questions[] questions;
	public Content[] contents;
	private string fileFormat = ".json";

	private void Awake()
	{
		sharedInstance = this;
	}

	private void Start()
	{
		StartCoroutine(LoadJsonData());
	}

	#region JSON ------------------------------------

	IEnumerator LoadJsonData()
	{

#if UNITY_STANDALONE_WIN
		string filePath = Path.Combine(Application.streamingAssetsPath, fileName + fileFormat);
		LoadData(filePath);

#elif UNITY_IOS
		string filePath = Path.Combine(Application.dataPath + "/Raw", fileName + fileFormat);
		LoadData(filePath);

#elif UNITY_ANDROID
		string filePath = Path.Combine(Application.streamingAssetsPath + "/", fileName + fileFormat);
		string fileJson;
		if (filePath.Contains("://") || filePath.Contains(":///"))
		{
			UnityWebRequest www = UnityWebRequest.Get(filePath);
			yield return www.Send();
			fileJson = www.downloadHandler.text;
		}
		else
		{
			fileJson = File.ReadAllText(filePath);
		}

		questions = JsonHelper.FromJsonArray<Questions>(fileJson);
		totalQuestions = questions.Length - 1;

		ChangeQuestions();
		ShowQuestion(true);

#endif

		yield return null;
	}

	private void LoadData(string _filePath)
	{
		if (File.Exists(_filePath))
		{
			string fileJson = File.ReadAllText(_filePath);
			contents = Util.FromJsonArray<Content>(fileJson);
			contentCount = contents.Length;
			//questions = Util.FromJsonArray<Questions>(fileJson);

			textNameContent.text = contents[index].name;
			textDescriptionContent.text = contents[index].description;

		}
		else
		{
			Debug.LogError("<color=red><b>" + "ERROR: " + "</b></color>" + "No se encontro el archivo JSON. ");
		}
	}

	#endregion ----------------------------------

	
	

	public void NextContent()
    {
		index++;
		if (contentCount <= index) index = 0;

		textNameContent.text = contents[index].name;
		textDescriptionContent.text = contents[index].description;
	}

	
	
}
