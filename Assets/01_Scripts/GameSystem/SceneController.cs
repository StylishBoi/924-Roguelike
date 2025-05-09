using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [SerializeField] private Animator transitionAnimator;

    private void Awake()
    {
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
    public void ChangeScene(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }
    
    public void ChangeFloor()
    {
        StartCoroutine(TakeStairs());
    }

    IEnumerator LoadLevel(string levelName)
    {
        transitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelName);
        yield return new WaitForSeconds(0.2f);
        transitionAnimator.SetTrigger("Start");
    }
    
    IEnumerator TakeStairs()
    {
        transitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        RoomFirstDungeonGenerator.Instance.RunProceduralGeneration();
        yield return new WaitForSeconds(0.1f);
        transitionAnimator.SetTrigger("Start");
    }
}
