using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WormGame/WeaponSO", fileName = "WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private string _weaponName;
    [SerializeField] private int _id;

    public string WeaponName => _weaponName;
    public int Id => _id;
}
