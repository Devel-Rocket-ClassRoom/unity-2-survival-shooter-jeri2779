using UnityEngine;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI waveText;
    //public TextMeshProUGUI ammoText;
    public GameObject gameOverUI;
     
    public TextMeshProUGUI scoreText;
    
    


    private void Awake()
    {
        Instance = this;

        // Start()보다 먼저 실행 보장 → PlayerShooter.Start()의 UpdateUI()가 이후에 올바르게 갱신
        
        SetScoreText(0);
       
        SetActiveGameOverUi(false);
    }











    // Start is called once before the first execution of Update after the MonoBehaviour is created

 

    public void SetScoreText(int score)
    {
        scoreText.text = $"Score: {score}";

    }

  

    public void SetActiveGameOverUi(bool active)
    {
        gameOverUI.SetActive(active);

    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

