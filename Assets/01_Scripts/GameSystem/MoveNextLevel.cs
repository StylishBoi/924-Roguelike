using UnityEngine;

public class MoveNextLevel : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    
    private bool _playerInRange;
    private UIManager _uIManager;
    private int _currentLevel;
    
    private void Start()
    {
        _currentLevel = 1;
        _playerInRange = false;
        
        visualCue.SetActive(false);
        
        if(GameObject.FindGameObjectWithTag("UIManager").TryGetComponent(out UIManager uiManager))
        {
            _uIManager = uiManager;
        }
    }

    void Update()
    {
        if (_playerInRange)
        {
            visualCue.SetActive(true);
            
            //Press button to access next floor
            if (InputManager.Interact)
            {
                ScoreUI.Instance.ScoreUpdate(25);
                _currentLevel++;
                AudioManager.Instance.PlaySfx(AudioManager.Instance.floorChangeSFX);
                GameManager.FloorReached++;
                _uIManager.ChangeLevelText();
                SceneController.Instance.ChangeFloor();
                
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }
}