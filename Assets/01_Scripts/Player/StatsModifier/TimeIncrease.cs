using UnityEngine;

public class TimeIncrease : MonoBehaviour
{
    [SerializeField] private int timeIncrease;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.upgradeSFX);
            UIManager.Instance.Upgrade();
            TimerUI.TimerIncrease(timeIncrease);
            Destroy(gameObject);
        }
    }
}
