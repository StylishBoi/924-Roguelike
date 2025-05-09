using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] Material flashMaterial;
    
    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;
    private Coroutine _flashRoutine;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
        flashMaterial=new Material(flashMaterial);
    }
    
    public void Flash(float flashDuration)
    {
        if (_flashRoutine != null)
        {
            StopCoroutine(_flashRoutine);
        }
        _flashRoutine = StartCoroutine(FlashRoutine(flashDuration));
    }
    
    private IEnumerator FlashRoutine(float flashDuration)
    {
        _spriteRenderer.material = flashMaterial;
        
        yield return new WaitForSeconds(flashDuration);
        
        _spriteRenderer.material = _originalMaterial;
        
        _flashRoutine = null;
    }
}
