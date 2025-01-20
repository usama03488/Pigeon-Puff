using UnityEngine;
using System.Collections;

public class Highscores : MonoBehaviour {

	const string privateCode = "hg4mLqCo4UmNdtXHFDYuugFqoG6a5UAkGLQzvwidHsiw";
	const string publicCode = "65dc411c8f40bbbe889b155c";
	const string webURL = "http://dreamlo.com/lb/";

	DisplayHighscores highscoreDisplay;
	public Highscore[] highscoresList;
	public static Highscores instance;
	
	void Awake() {
		highscoreDisplay = GetComponent<DisplayHighscores> ();
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}

		// Assign this instance as the singleton instance
		instance = this;
		DontDestroyOnLoad(this);

	}

	public static void AddNewHighscore(string username, int score) {
		instance.StartCoroutine(instance.UploadNewHighscore(username,score));
	}

public 	IEnumerator UploadNewHighscore(string username, int score) {
        if (FirebaseGame.instance.User != null)
        {
			FirebaseGame.instance.upload_Score(score);
			WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
			yield return www;

			if (string.IsNullOrEmpty(www.error))
			{
				print("Upload Successful");
				DownloadHighscores();
			}
			else
			{
				print("Error uploading: " + www.error);
			}
		}
		
	}

	public void DownloadHighscores() {
		StartCoroutine("DownloadHighscoresFromDatabase");
	}

	IEnumerator DownloadHighscoresFromDatabase() {
		WWW www = new WWW(webURL + publicCode + "/pipe/");
		yield return www;
		
		if (string.IsNullOrEmpty (www.error)) {
			FormatHighscores (www.text);
			highscoreDisplay.OnHighscoresDownloaded(highscoresList);
		}
		else {
			print ("Error Downloading: " + www.error);
		}
	}

	void FormatHighscores(string textStream) {
		string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		highscoresList = new Highscore[entries.Length];

		for (int i = 0; i <entries.Length; i ++) {
			string[] entryInfo = entries[i].Split(new char[] {'|'});
			string username = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			highscoresList[i] = new Highscore(username,score);
		//	print (highscoresList[i].username + ": " + highscoresList[i].score);
		}
	}

}

public struct Highscore {
	public string username;
	public int score;

	public Highscore(string _username, int _score) {
		username = _username;
		score = _score;
	}

}
