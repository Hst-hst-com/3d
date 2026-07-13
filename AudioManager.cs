using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer mixer;
    public AudioSource bgmSource;

    // 专门播放音效的 AudioSource
    public AudioSource sfxSource;

    // 默认收集音效（可选）
    public AudioClip defaultCollectSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ⭐ 加载音量设置
        if (SaveManager.Instance != null && SaveManager.Instance.HasSaveData())
        {
            var saveData = SaveManager.Instance.GetSaveData();
            if (saveData != null)
            {
                SetBgmVolume(saveData.bgmVolume);
                SetSfxVolume(saveData.sfxVolume);
                Debug.Log($"🔊 恢复音量设置：BGM={saveData.bgmVolume}, SFX={saveData.sfxVolume}");
            }
        }
    }

    // ========== 原有功能 ==========

    public void SetBgmVolume(float sliderValue)
    {
        float db = sliderValue > 0.001f ? 20 * Mathf.Log10(sliderValue) : -80;
        mixer.SetFloat("BgmVol", db);

        // ⭐ 保存音量设置
        if (SaveManager.Instance != null)
        {
            var data = SaveManager.Instance.GetSaveData();
            if (data != null)
            {
                data.bgmVolume = sliderValue;
                SaveManager.Instance.SaveGame();
            }
        }
    }

    public void SetSfxVolume(float sliderValue)
    {
        float db = sliderValue > 0.001f ? 20 * Mathf.Log10(sliderValue) : -80;
        mixer.SetFloat("SfxVol", db);

        // ⭐ 保存音量设置
        if (SaveManager.Instance != null)
        {
            var data = SaveManager.Instance.GetSaveData();
            if (data != null)
            {
                data.sfxVolume = sliderValue;
                SaveManager.Instance.SaveGame();
            }
        }
    }

    public void ToggleBGM(bool isOpen)
    {
        if (isOpen)
            bgmSource.Play();
        else
            bgmSource.Pause();
    }

    // ========== 新增功能 ==========

    /// <summary>
    /// 播放一次性音效
    /// </summary>
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlaySFX: 音效为空！");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogWarning("PlaySFX: sfxSource 未设置！");
            return;
        }

        sfxSource.PlayOneShot(clip, volumeScale);
    }

    /// <summary>
    /// 播放默认收集音效
    /// </summary>
    public void PlayCollectSound()
    {
        if (defaultCollectSound != null)
        {
            PlaySFX(defaultCollectSound);
        }
        else
        {
            Debug.LogWarning("未设置默认收集音效！");
        }
    }
}