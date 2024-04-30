using UnityEngine;
using WormGame.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace WormGame.FactoryPattern
{
    public class RedArrowProduct : ArrowProduct
    {
        private GameObject _target;
        
        private void Awake()
        {
            ResetColor();
            _target = _wormDataSO.Head;
        }

        public override void Initialize()
        {
            // Any unique logic to this product
            gameObject.name = base._productName;

            Vector2 pos = GetSpawnPosition();
            transform.position = pos;

            Quaternion rot = SpawnUtils.GetRotationToLookAtTarget(_target.transform.position, transform.position);
            transform.rotation = rot;

            SetVelocityTowardsFacingDirection(_rb2D);
        }

        private void ResetVelocity()
        {
            _rb2D.velocity = Vector2.zero;
        }

        private void ResetRotation()
        {
            transform.rotation = Quaternion.identity;
        }

        private void ResetColor()
        {
            Color arrowColor = _arrowDataSO.ArrowColor;
            List<SpriteRenderer> spriteRends = gameObject.GetComponentsInChildren<SpriteRenderer>().ToList();

            foreach (SpriteRenderer sprite in spriteRends)
            {
                sprite.color = arrowColor;
            }
        }

        private void OnDisable()
        {
            ResetVelocity();
            ResetRotation();
        }
    }
}
