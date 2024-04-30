using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;
using WormGame.Variables;

namespace WormGame.Core.Utils
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private StringVariableSO _damagePopupPoolTag;
        public void DestroyGO()
        {
            string str = _damagePopupPoolTag.Value;
            ObjectPooler.Instance.ReturnObject(transform.parent.gameObject, in str);
        }
    }
}