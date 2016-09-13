using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Trigger to pick the weapon up is in the child of this script's gameObject.
/// So when dealing with the weapon, you should use the PARENT of that collider to point to the realWeapon, this script.
/// </summary>
[System.Serializable] public class V_Weapon : MonoBehaviour {
    public int ID;
    new public string name;
    public float fireRate,nextFireTime;
    public float lifeTime;
    public float range;
    public float accuracy;
    public float weight;
    public int clipSize,ammo,bullet;
    public int damage;
    public GameObject muzzleFlash;
    public GameObject pickupModel;
    public GameObject handedWeaponModel;
    public Texture customizedTexture;
    public Sprite icon;


    public List<V_UpgradeObject> upgrades;

    public enum weaponType { pistol, rifle, grenade, flash, smoke, melee, bomb };

    public weaponType type;
    public enum weaponLevel { beginner1 = 2, beginner2, pro1, pro2 };
    public weaponLevel level;
    public int _subLevel;
    
    public const int beginner1 = 500, beginner2 = 502, pro1 = 505, pro2 = 507;
    public int subLevel{
        set
        {
            _subLevel = value;
            if (_subLevel == beginner1)
            {
                level = weaponLevel.beginner1;
            }
            else if (_subLevel == beginner2)
            {
                level = weaponLevel.beginner2;
            }
            else if (_subLevel == pro1)
            {
                level = weaponLevel.pro1;
            }
            else if (_subLevel == pro2)
            {
                level = weaponLevel.pro2;
            }
        }
        get
        {
            return _subLevel;
        }
    }
    public bool isGrounded = false;
    public void OnTriggerEnter(Collider other) {
        if (other.tag == "ground")
        {
            isGrounded = true;
            if (lifeTime < 1)
            {
                Destroy(gameObject);
            }
        }
    }
    //public static bool operator ==(V_Weapon w1, V_Weapon w2)
    //{
    //    bool status = w1.ID == w2.ID;
    //    print(w1.name + ".ID : " + w2.name + ".ID");
    //    return status;
    //}
    //public static bool operator !=(V_Weapon w1, V_Weapon w2)
    //{
    //    bool status = w1.ID != w2.ID;
    //    print(w1.name + ".ID : " + w2.name + ".ID");
    //    return status;
    //}
}

[System.Serializable] public class V_UpgradeObject
{
    public GameObject model;
    public bool isLocked, isUsed;
    public Sprite icon;
    public void Use(bool use)
    {
        isUsed = use;
        model.SetActive(use);
    }
    public void Invert()
    {
        Use(!isUsed);
    }
}