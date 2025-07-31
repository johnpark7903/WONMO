using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon };
    public Type type;
    public int value;
    
      // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 10 * Time.deltaTime);
    }
}
