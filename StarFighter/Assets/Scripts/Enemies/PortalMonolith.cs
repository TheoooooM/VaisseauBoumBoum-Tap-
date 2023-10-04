using System;
using Unity.Mathematics;
using UnityEngine;

namespace Enemies
{
    public class PortalMonolith : MonoBehaviour
    {
        private MonolithEnemy _monolith;

        private MeshRenderer _renderer;
        private Color _baseColor;
        
        private void Start()
        {
            _baseColor = _renderer.material.color;
        }

        public void InitLaunch()
        {
            _renderer.material.color = Color.red;
        }

        public void Launch()
        {
            GameObject missil = PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.Missile, transform.position, quaternion.identity);
            missil.transform.LookAt(transform.position + transform.forward, transform.up);
            _renderer.material.color = _baseColor;
        }

        void GetDestroy()
        {
            _monolith.DestroyPortal(this);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) GetDestroy();
        }
    }
}