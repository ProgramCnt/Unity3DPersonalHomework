using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
            }
            return _instance;
        }
    }

    static Player _player;
    public Player Player
    {
        get
        {
            return _player;
        }
        set
        {
            _player = value;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
