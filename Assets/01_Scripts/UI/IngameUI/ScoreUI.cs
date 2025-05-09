using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance;
    public static int TotalScore = 0;
    
    public int mostRecentScore = 0;
    
    private TextMeshProUGUI _scoreText;
    [SerializeField] GameObject _scoreEffect;
    [SerializeField] Transform _scorePosition;

    private void Start()
    {
        //Gets the components
        if(TryGetComponent(out _scoreText)){}
        
        //Verify if the instance exist or makes it
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        //Initialize the score
        TotalScore = 0;
    }

    public void ScoreUpdate(int score)
    {
        TotalScore += score;
        mostRecentScore=score;
        _scoreText.text = TotalScore.ToString();
        Instantiate(_scoreEffect, new Vector3(_scorePosition.position.x, _scorePosition.position.y, _scorePosition.position.z), Quaternion.identity, transform);
    }
}
