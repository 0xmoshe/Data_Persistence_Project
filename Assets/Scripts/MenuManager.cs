using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public TextMeshProUGUI bestScoreText;
    public GameObject userNameInput;

    public string userName;
    public string bestName;
    public int bestScore;

    private void Awake()
    {
        if ( Instance != null )
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadBestScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (bestScore != 0 )
        {
            bestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        } else if ( bestScore == 0 )
        {
            bestScoreText.text = "Best Score : " + bestScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit(); // original code to quit Unity
    }

    public void SubmitName(string arg0)
    {
        userName = arg0;
    }

    [System.Serializable]
    class SaveData
    {
        public string bestName;
        public int bestScore;

    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.bestName = bestName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestName = data.bestName;
            bestScore = data.bestScore;
        }
    }
}
