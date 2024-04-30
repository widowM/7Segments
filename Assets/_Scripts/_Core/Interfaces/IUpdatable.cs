using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An interface for objects that can be updated via the UpdateManager
namespace WormGame.Core
{
    public interface IUpdatable
    {
        void UpdateMe();
    }
}