using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static int BaseScore=0;
    //Needs to be one otherwise all enemies immedietaly die when starting from the dungeon scene
    public static int FloorReached=1;
    public static int TotalScore=0;
    
    public static bool NewHighScore;
    
    public static int Highscore=0;
    
    public static void GameStart()
    {
        //Reset the score stats and player stats
        BaseScore = 0;
        FloorReached = 1;
        NewHighScore=false;
        PlayerStats.StatsReset();
        
        SceneController.Instance.ChangeScene("DungeonGeneration");
    }
    
    public static void GameEnd(int score)
    {
        BaseScore=score;
        TotalScore = BaseScore * FloorReached;
        
        if (Highscore < TotalScore)
        {
            Highscore = TotalScore;
            NewHighScore = true;
        }
        SceneController.Instance.ChangeScene("EndScreen");
    }
    
    public static void MainMenu()
    {
        SceneController.Instance.ChangeScene("MainMenu");
    }
    
    public static void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
