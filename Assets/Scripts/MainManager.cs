using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI BestScoreText;
    public TextMeshProUGUI ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    public string m_Name;
    public string m_BestName;
    private int m_Points;
    public int m_Score;
    public int m_BestScore;
    
    private bool m_GameOver = false;

    private void Awake()
    {
        MenuManager.Instance.LoadBestScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Name = MenuManager.Instance.userName;
        m_BestScore = MenuManager.Instance.bestScore;
        m_BestName = MenuManager.Instance.bestName;
        if ( m_BestScore > 0 )
        {
            BestScoreText.text = "Best score : " + m_BestName + " : " + m_BestScore;
        } else if ( m_BestScore == 0 )
        {
            BestScoreText.text = "Best score : " + m_BestScore;
        }
        

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        
        m_GameOver = true;
        GameOverText.SetActive(true);
        m_Score = m_Points;
        if ( m_Score > m_BestScore )
        {
            m_BestName = m_Name;
            MenuManager.Instance.bestName = m_BestName;
            m_BestScore = m_Score;
            MenuManager.Instance.bestScore = m_BestScore;
        }

        MenuManager.Instance.SaveBestScore();
    }
}
