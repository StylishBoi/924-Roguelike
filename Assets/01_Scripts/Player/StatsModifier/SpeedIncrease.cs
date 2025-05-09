using UnityEngine;

public class SpeedIncrease : MonoBehaviour
{
    [SerializeField] [Range(1,3)] private int speedIncrease = 1;
    private void IncreaseStats()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.upgradeSFX);
        UIManager.Instance.Upgrade();
        ScoreUI.Instance.ScoreUpdate(5);
        PlayerStats.SeekSpeed+=speedIncrease;
        UIManager.Instance.ChangeSpeedText();
        
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IncreaseStats();
        }
    }
}
