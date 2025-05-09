using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Music")]
    [SerializeField] public AudioClip menuOST;
    [SerializeField] public AudioClip scoreOST;
    [SerializeField] public AudioClip gameOST;
    
    [Header("Sounds")]
    [SerializeField] public AudioClip buttonSFX;
    [SerializeField] public AudioClip shootSFX;
    [SerializeField] public AudioClip enemyHitSFX;
    [SerializeField] public AudioClip playerHitSFX;
    [SerializeField] public AudioClip healthSFX;
    [SerializeField] public AudioClip upgradeSFX;
    [SerializeField] public AudioClip enemyDeathSFX;
    [SerializeField] public AudioClip playerDeathSFX;
    [SerializeField] public AudioClip floorChangeSFX;
    [SerializeField] public AudioClip slimeExplosionSFX;
    [SerializeField] public AudioClip batProjectileSFX;
    [SerializeField] public AudioClip highscoreSFX;
    
    public static AudioManager Instance;
    
    void Awake()
    {
        musicSource.loop = true;
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayBGM(AudioClip clip)
    {
        //Stops the background music
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        //Allows for SFX to be played in other scripts
        sfxSource.PlayOneShot(clip);
    }
}
