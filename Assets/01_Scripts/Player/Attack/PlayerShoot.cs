using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    
    [SerializeField] public float timeBetweenFiring;

    public static Action OnCoolDown;
    
    public bool _canFire;
    public float _timer;
    
    private Camera _mainCam;
    private Vector3 _mousePos;
    
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("MainCamera").TryGetComponent(out Camera _camera))
        {
            _mainCam = _camera;
        }
    }
    void Update()
    {
        //Setup the mouse direction
        _mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = _mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        //Increase timer when unable to shoot
        if (!_canFire)
        {
            _timer += Time.deltaTime;
            if (_timer > timeBetweenFiring)
            {
                _canFire = true;
                _timer = 0;
            }
        }
            
        //Shoot
        if (InputManager.Attack && _canFire)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.shootSFX);
            Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            _canFire = false;
            WeaponUI.ActivateCooldown();
        }
    }
}
