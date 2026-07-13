using UnityEngine;
public class DayNightManager : MonoBehaviour
{
    [Header("白天、夜晚天空盒材质拖入")]
    public Material daySky;
    public Material nightSky;
    private bool isDay = true;

    //UI按钮调用的公开方法
    public void ToggleDayNight()
    {
        isDay = !isDay;
        RenderSettings.skybox = isDay ? daySky : nightSky;
        DynamicGI.UpdateEnvironment();
    }
}
