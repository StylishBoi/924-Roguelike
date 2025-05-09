using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private static int Minute;
    private static int Second;

    [SerializeField] private int givenTime;
    
    [SerializeField] private GameObject timerEffect;
    [SerializeField] private Transform timerPosition;
    private TextMeshProUGUI _timerText;

    private static float _timer;
    public static int _bonusTime;
    private static bool _timeIncrease;
    
    void Start()
    {
        //Gets the components
        if(TryGetComponent(out _timerText)){}
        
        //Intiliaze the timer
        Minute = givenTime / 60;
        Second = givenTime % 60;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer <= 0)
        {
            Second--;
            if (Second <= 0)
            {
                
                if (Minute <= 0 && Second <= 0)
                {
                    Debug.Log("Game ended");
                    GameManager.GameEnd(ScoreUI.TotalScore);
                    //End scene
                }
                Minute--;
                Second = 59;
                
            }
            _timer=1;
        }
        _timerText.text = $"{Minute:00}:{Second:00}";

        if (_timeIncrease)
        {
            Instantiate(timerEffect, new Vector3(timerPosition.position.x, timerPosition.position.y, timerPosition.position.z), Quaternion.identity, transform);
            _timeIncrease = false;
        }
    }

    public static void TimerIncrease(int givenTime)
    {
        Second += givenTime;
        if (Second > 59)
        {
            Minute++;
            Second = (Second - 59);
        }

        _bonusTime=givenTime;
        _timeIncrease = true;
    }
}
