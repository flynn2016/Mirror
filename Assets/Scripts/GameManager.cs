using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterManager main;
    public CharacterManager bear;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        bear.OnUpdate();
        main.OnUpdate();
    }
}
