using UnityEngine;

//天气枚举
public enum WeatherType
{
    Sunny,
    Rain,
    Snow
}

public class WeatherManager : MonoBehaviour
{
    // ⭐ 添加单例
    public static WeatherManager Instance { get; private set; }

    [Header("3个天空盒材质，从Project里拖拽赋值")]
    public Material daySkyMat;
    public Material rainSkyMat;
    public Material snowSkyMat;

    [Header("粒子物体，从Hierarchy面板拖拽赋值")]
    public GameObject rainObj;
    public GameObject snowObj;

    // ⭐ 当前天气
    public WeatherType CurrentWeather { get; private set; } = WeatherType.Sunny;

    // ⭐ 天气变化事件（GameManager 会监听这个）
    public delegate void WeatherChanged(WeatherType newWeather);
    public event WeatherChanged OnWeatherChanged;

    private void Awake()
    {
        // ⭐ 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ⭐ 获取当前天气（从 Inspector 中读取，如果没有则默认晴天）
        CurrentWeather = WeatherType.Sunny;
    }

    private void Start()
    {
        // ⭐ 确保初始状态正确
        ApplyWeather(CurrentWeather);
    }

    void Update()
    {
        //键盘按键：1‑晴天，2‑雨天，3‑雪天
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeather(WeatherType.Sunny);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeather(WeatherType.Rain);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeather(WeatherType.Snow);
        }
    }

    /// <summary>
    /// 设置天气（公开方法，UI按钮也可以调用）
    /// </summary>
    public void SetWeather(WeatherType weather)
    {
        // 如果天气没变化，不重复触发事件
        if (CurrentWeather == weather) return;

        CurrentWeather = weather;
        ApplyWeather(weather);

        // ⭐ 触发天气变化事件，通知 GameManager
        OnWeatherChanged?.Invoke(weather);

        Debug.Log($"🌤️ 天气切换为：{weather}");
    }

    /// <summary>
    /// 应用天气效果（天空盒 + 粒子）
    /// </summary>
    private void ApplyWeather(WeatherType weather)
    {
        //默认先关闭雨雪粒子
        if (rainObj != null) rainObj.SetActive(false);
        if (snowObj != null) snowObj.SetActive(false);

        switch (weather)
        {
            case WeatherType.Sunny:
                if (daySkyMat != null) RenderSettings.skybox = daySkyMat;
                break;
            case WeatherType.Rain:
                if (rainSkyMat != null) RenderSettings.skybox = rainSkyMat;
                if (rainObj != null) rainObj.SetActive(true);
                break;
            case WeatherType.Snow:
                if (snowSkyMat != null) RenderSettings.skybox = snowSkyMat;
                if (snowObj != null) snowObj.SetActive(true);
                break;
        }
        DynamicGI.UpdateEnvironment(); //立刻刷新环境
    }

    // ⭐ 快捷方法：供 UI 按钮调用
    public void SetSunny()
    {
        SetWeather(WeatherType.Sunny);
    }

    public void SetRain()
    {
        SetWeather(WeatherType.Rain);
    }

    public void SetSnow()
    {
        SetWeather(WeatherType.Snow);
    }
}
