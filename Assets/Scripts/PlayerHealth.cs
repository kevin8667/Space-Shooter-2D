using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    int _Health = 3;
    
    public void Damage()
    {
        _Health--;

        if(_Health < 1)
        {
            Destroy(gameObject);
        }
    }


}
