using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreEffect : MonoBehaviour
{
    private float _effectTimer;
    private TextMeshProUGUI _effectText;
    
    private Color _baseColor;

    void Start()
    {
        //Gets the components
        if (TryGetComponent(out _effectText)) {}
        
        _effectText.text = ScoreUI.Instance.mostRecentScore.ToString(); 
        
        _baseColor = _effectText.color;
    }
    void Update()
    {
        transform.localPosition += new Vector3(0f, -100f, 0f)*Time.deltaTime;
        _effectText.color -= new Color(_baseColor.r, _baseColor.g, _baseColor.b, 0.5f) * Time.deltaTime;
        
        _effectTimer += Time.deltaTime;
        if (_effectTimer >= 2f)
        {
            Destroy(this.gameObject);
        }
    }
}
