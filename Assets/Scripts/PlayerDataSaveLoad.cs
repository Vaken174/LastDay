using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSaveLoad : MonoBehaviour
{
    [SerializeField] private Indicators _indicators;
    [SerializeField] private Player_Movement _player_Movement;
    [SerializeField] private InventoryManager _inventoryManager;
    

    public void SavePlayer()
    {
        BinarySavingSystem.SavePlayer(_indicators,_player_Movement, _inventoryManager);
    }

    public void LoadPlayer()
    {
        PlayerData data = BinarySavingSystem.LoadPlayer();

        _indicators.healthAmount = data.health;
        _indicators.waterAmount = data.water;
        _indicators.foodAmount = data.food;
        
        _player_Movement.transform.position =
            new Vector3(data.position[0], data.position[1], data.position[2]); 
    }
    
}
