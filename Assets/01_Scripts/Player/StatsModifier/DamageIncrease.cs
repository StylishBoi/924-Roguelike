using UnityEngine;

public class DamageIncrease : MonoBehaviour
{
    [SerializeField] [Range(1,3)] private int damageIncrease = 1;

    private void IncreaseStats()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.upgradeSFX);
        UIManager.Instance.Upgrade();
        ScoreUI.Instance.ScoreUpdate(5);
        PlayerStats.SeekDamage+=damageIncrease;
        UIManager.Instance.ChangeAttackText();
        
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
