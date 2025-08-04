using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject LoadingPanel;
    public Image LoadingBar;

    [SerializeField]
    private bool _isLoadingPanelActive = false;
    public bool IsLoadingPanelActive
    {
        get
        {
            return _isLoadingPanelActive;
        }
        set
        {
            _isLoadingPanelActive = value;
            LoadingPanel.SetActive(_isLoadingPanelActive);
        }
    }

    [SerializeField] [Range(0, 1)]
    private float _loadingValue;
    public float LoadingValue
    {
        get
        {
            return _loadingValue;
        }
        set
        {
            _loadingValue = Mathf.Clamp01(value);
            LoadingBar.fillAmount = _loadingValue;
        }
    }

    void OnValidate()
    {
        LoadingBar.fillAmount = _loadingValue;
        LoadingPanel.SetActive(_isLoadingPanelActive);
    }
}
