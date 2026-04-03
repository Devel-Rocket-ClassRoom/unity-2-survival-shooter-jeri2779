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
    public GameObject settingsUI;

    public TextMeshProUGUI scoreText;

    public AudioMixer audioMixer;

    public Button quitButton;
    public Button resumeButton;

    public Slider volumeSlider;
    public Slider sfxSlider;

    public Toggle muteToggle;

    private bool isPaused = false;




    private void Awake()
    {
        Instance = this;

        // Start()보다 먼저 실행 보장 → PlayerShooter.Start()의 UpdateUI()가 이후에 올바르게 갱신

        SetScoreText(0);

        SetActiveGameOverUi(false);

        if (settingsUI != null) settingsUI.SetActive(false);
    }
    private void Start()
    {
        quitButton.onClick.AddListener(OnClickQuit);
        resumeButton.onClick.AddListener(ResumeGame);
        volumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        muteToggle.onValueChanged.AddListener(SetSoundMute);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }


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
        Time.timeScale = 1f; // 게임 재시작 시 시간 정상화
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetMusicVolume(float volume)
    {

        float val = Mathf.Max(0.0001f, volume);
        float dbValue = Mathf.Log10(val) * 20;
        bool isSuccess = audioMixer.SetFloat("MusicVol", dbValue);

       
    }

    public void SetSFXVolume(float volume)
    {
        float val = Mathf.Max(0.0001f, volume);
        audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }
    public void SetSoundMute(bool isMute)
    {
        AudioListener.pause = isMute;
    }





    private void ToggleSettings()
    {
        isPaused = !isPaused;
        settingsUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        // 마우스 커서 보이기/숨기기
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ResumeGame()
    {
        isPaused = false;
        settingsUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        //추후 커서락으로 전환 고려
    }

    public void OnClickQuit()
    {

        UnityEditor.EditorApplication.isPlaying = false;//에디터내 종료 
        Application.Quit();//실제 빌드 종료
        Debug.Log("Quit Game");

    }
}
   

