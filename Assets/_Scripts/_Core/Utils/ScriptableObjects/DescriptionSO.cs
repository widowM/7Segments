using UnityEngine;

namespace WormGame.Core.Utils
{

    /// <summary>
    /// This is a base ScriptableObject that adds a description field.
    /// </summary>
    public class DescriptionSO : ScriptableObject
    {
        [TextArea(5, 20)]
        [SerializeField] protected string _description;
    }
}