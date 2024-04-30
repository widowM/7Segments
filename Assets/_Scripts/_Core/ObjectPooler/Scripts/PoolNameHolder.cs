using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Variables;

public class PoolNameHolder : MonoBehaviour
{
    [SerializeField] private StringVariableSO _poolName;

    public StringVariableSO PoolName => _poolName;
}