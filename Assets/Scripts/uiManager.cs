using UnityEngine;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI waveText;
    //public TextMeshProUGUI ammoText;
    public GameObject gameOverUI;
     
    public TextMeshProUGUI scoreText;

    public AudioMixer audioMixer;




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
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }
    public void SetSoundMute(bool isMute)
    {
        AudioListener.pause = isMute;
    }
}

