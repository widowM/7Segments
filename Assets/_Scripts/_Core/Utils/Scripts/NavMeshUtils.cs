using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;

namespace WormGame.Core.Utils
{
    public class NavMeshUtils : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _navMeshSurface2D;

        public void BuildNavMeshAsync()
        {
            _navMeshSurface2D.BuildNavMeshAsync();
        }
    }
}