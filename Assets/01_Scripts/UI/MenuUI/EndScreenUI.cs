using TMPro;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textFloors;
    [SerializeField] private TextMeshProUGUI textTotalScore;

    [SerializeField] private TextMeshProUGUI textHighscore;
        
    [SerializeField] private GameObject newHighscoreText;
    
    void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Instance.scoreOST);
        
        textScore.text = "Score : " + GameManager.BaseScore;
        textFloors.text = "Floor Reached : " + GameManager.FloorReached;
        textTotalScore.text = "Total Score : " + GameManager.BaseScore + " X " + GameManager.FloorReached + " = " + GameManager.TotalScore;
        
        textHighscore.text = "Highscore : " + GameManager.Highscore;

        if (GameManager.NewHighScore)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.highscoreSFX);
            newHighscoreText.SetActive(true);
        }
    }

    public void StartNewGame()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        newHighscoreText.SetActive(false);
        GameManager.GameStart();
    }
    
    public void MoveMainMenu()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        newHighscoreText.SetActive(false);
        GameManager.MainMenu();
    }
    
    //1 - Add enemy panel
    //2 - Upload to Github
}
