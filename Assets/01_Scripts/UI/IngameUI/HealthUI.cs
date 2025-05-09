using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameObject heartIcon;
    [SerializeField] private GameObject hurtIcon;

    private RectTransform lifePanel;
    private PlayerHealth _playerHealth;
    private List<Image> _healthSlots = new List<Image>();

    private void Start()
    {
        //Gets the components
        TryGetComponent(out lifePanel);

        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerHealth outPlayer))
        {
            _playerHealth = outPlayer;
        }
        
        HeartDisplay(_playerHealth.maxHealth);
    }

    private void FixedUpdate()
    {
        if (_playerHealth==null && SceneManager.GetActiveScene().name=="DungeonGeneration")
        {
            if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerHealth outPlayer))
            {
                _playerHealth = outPlayer;
                HeartDisplay(_playerHealth.maxHealth);
            }
        }
    }

    private void HeartDisplay(int heartCount)
    {
        //Empty out list
        _healthSlots.Clear();
        
        //Creates the list of health slots
        _healthSlots = new List<Image>(_playerHealth.maxHealth);
        GameObject newSlot;
        int rowIncrease=0;
        int columnIncrease=1;
        
        for (int i = 0; i < heartCount; i++)
        {
            if (i % 5 == 0)
            {
                rowIncrease++;
                columnIncrease = 1;
            }
            else
            {
                columnIncrease++;
            }
            
            newSlot = Instantiate(heartIcon, lifePanel);
            newSlot.transform.position = new Vector3(columnIncrease * 100f, rowIncrease * 100f, 1f);
            newSlot.name=("HeartSlot " + (i));
            
            _healthSlots.Add(newSlot.GetComponent<Image>());
        }
    }

    private void AddMaxHeart(int heartCount)
    {
        HeartDisplay(heartCount);
    }

    public void LowerHealth(int damage, int currentHealth)
    {
        int newHealth = currentHealth - damage;
        
        if (newHealth < 0)
        {
            newHealth = 0;
        }
            
        for (int i = currentHealth; i > (newHealth); i--)
        {
            _healthSlots[i-1].sprite = hurtIcon.GetComponent<Image>().sprite;
            _healthSlots[i-1].color = Color.white;
        }
    }
    
    public void IncreaseHealth(int heal, int currentHealth)
    {
        int newHealth = currentHealth - heal;
        
        if (newHealth < 0)
        {
            newHealth = 0;
        }
        
        for (int i = currentHealth; i > (newHealth); i--)
        {
            _healthSlots[i-1].sprite = heartIcon.GetComponent<Image>().sprite;
            _healthSlots[i-1].color = Color.red;
        }
    }
}
