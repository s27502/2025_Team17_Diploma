using UnityEngine;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] _resolutions;

    void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResIndex = 0;
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < _resolutions.Length; i++)
        {
            double refresh = _resolutions[i].refreshRateRatio.value;
            string option = $"{_resolutions[i].width} x {_resolutions[i].height}"; 
            

            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height &&
                Approximately(refresh, Screen.currentResolution.refreshRateRatio.value))
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int index)
    {
        Resolution res = _resolutions[index];

        Screen.SetResolution(
            res.width,
            res.height,
            Screen.fullScreenMode,
            res.refreshRateRatio
        );
    }
    private bool Approximately(double a, double b, double tolerance = 0.5)
    {
        return System.Math.Abs(a - b) < tolerance;
    }
    
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    
}