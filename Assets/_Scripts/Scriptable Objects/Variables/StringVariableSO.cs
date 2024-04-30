using UnityEngine;

namespace WormGame.Variables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/Variables/StringVariable", fileName = "StringVariable")]
    public class StringVariableSO : ScriptableObject
    {
        [SerializeField] private string _value;

        public string Value
        {
            get { return _value; }
        }
    }
}
