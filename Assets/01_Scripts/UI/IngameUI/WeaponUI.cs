using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Image coolDownIcon;
    
    private PlayerShoot _playerShoot;

    private static bool canShoot;

    private void Start()
    {
        //Gets the components
        if (GameObject.FindGameObjectWithTag("ShootPlacement").TryGetComponent(out PlayerShoot playerShoot))
        {
            _playerShoot = playerShoot;
        }
    }

    private void Update()
    {
        if (canShoot)
        {
            coolDownIcon.fillAmount += 2 / _playerShoot.timeBetweenFiring * Time.deltaTime;
            if (coolDownIcon.fillAmount >= 1)
            {
                canShoot = false;
            }
        }
        else
        {
            coolDownIcon.fillAmount = 0;
        }

        
    }
    
    public static void ActivateCooldown()
    {
        canShoot = true;
    }

}
