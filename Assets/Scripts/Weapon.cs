using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 銃撃が出来るようにする関数
    public void CanShoot()
    {
        GameState.canShoot = true;
    }
}
