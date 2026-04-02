using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // 오디오 믹서 제어용
using UnityEngine.SceneManagement; // 씬 전환용

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject settingsPanel;

    // 1. 음악 볼륨 (슬라이더 이벤트에 연결)
    public void SetMusicVolume(float volume)
    {
        // 오디오 믹서는 로그 스케일(-80dB ~ 20dB)을 사용하므로 변환이 필요합니다.
        audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    // 2. 효과음 볼륨
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }

    // 3. 사운드 온/오프 (토글)
    public void SetMute(bool isMute)
    {
        AudioListener.pause = isMute;
        // 비유: 전체 스피커의 전원을 끄는 것과 같습니다.
    }

    // 4. 게임 재개
    public void ResumeGame()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f; // 멈췄던 시간을 다시 흐르게 합니다.
    }

    // 5. 게임 나가기
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터용
#else
        Application.Quit(); // 빌드된 게임용
#endif
    }
}