using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Scripts.Domains.Models;

namespace App.Scripts.Domains.Services
{
    public class GameManager : MonoBehaviour
    {
        private Player _player;

        public GameManager(Player player)
        {
            _player = player;
        }

        public void IncreaseScore(int amount)
        {
            _player.Score += amount;
        }
        
        
        
    }
}


