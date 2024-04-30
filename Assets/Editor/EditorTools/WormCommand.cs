using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame
{
    public class WormCommand
    {
        public GameObject connectableSegment;
        public List<GameObject> disconnectedChain;

        public WormCommand(GameObject connectableSegment, List<GameObject> disconnectedChain)
        {
            this.connectableSegment = connectableSegment;
            this.disconnectedChain = disconnectedChain;
        }
    }
}