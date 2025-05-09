using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Screen Effect")]
    [SerializeField] private Image resultFlash;
    [SerializeField] private float timeToFade;
    [SerializeField] private TextMeshProUGUI levelText;
    
    [Header("Fade Color")]
    [SerializeField] private Color hurt;
    [SerializeField] private Color healed;
    [SerializeField] private Color baseUpgrade;
    
    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI damageStatText;
    [SerializeField] private TextMeshProUGUI speedStatText;
    
    [Header("UI GameObject")]
    [SerializeField] private GameObject resultScreen;
    [SerializeField] private GameObject fullScreenFlash;

    public static UIManager Instance;
    
    private GameObject _ui;
    private Color _currentColor;
    
    private bool _effectFade;
    private bool _fadeIn;
    private bool _fadeOut;
    
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _ui = transform.gameObject;
        _ui.SetActive(true);
        
        //Deactivate the temporary UI elements
        resultScreen.SetActive(false);
        fullScreenFlash.SetActive(false);
    }

    void Update()
    {
        if (_effectFade)
        {
            resultScreen.SetActive(true);
            _currentColor=resultFlash.color;
            
            if (_currentColor.a >= 0)
            {
                _currentColor.a -= Time.deltaTime;
                
                if (_currentColor.a <= 0)
                {
                    _effectFade = false;
                    resultScreen.SetActive(false);
                }
                resultFlash.color = _currentColor;
            }
        }
    }

    public void HideUI()
    {
        _ui.SetActive(false);
    }
    
    public void ShowUI()
    {
        _ui.SetActive(true);
    }

    public void Hurt()
    {
        resultFlash.color = hurt;
        _effectFade = true;
    }
    public void Healed()
    {
        resultFlash.color = healed;
        _effectFade = true;
    }
    
    public void Upgrade()
    {
        resultFlash.color = baseUpgrade;
        _effectFade = true;
    }
    
    public void ChangeLevelText()
    {
        levelText.text = GameManager.FloorReached.ToString();
    }
    
    public void ChangeAttackText()
    {
        damageStatText.text = PlayerStats.SeekDamage.ToString();
    }
    public void ChangeSpeedText()
    {
        speedStatText.text = (PlayerStats.SeekSpeed-5).ToString();
    }
}
