using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{

    public UIManager Manager;
    public Spawner spawner;
    public static GameManager m_instance;

    private int score;
    internal bool isGameOver { get; private set; }

    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                //m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        m_instance = this;
    }

  

    public void AddScore(int newScore)
    {
        if (!isGameOver)
        {
            score += newScore;
        }
        UIManager.Instance.SetScoreText(score);
    }

    public void EndGame()
    {
        isGameOver = true;
        spawner.enabled = false;
        Manager.SetActiveGameOverUi(true);
    }
}

