using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class PortalMonolith : MonoBehaviour
    {
        private MonolithEnemy _monolith;
        [SerializeField] private MeshRenderer portalRenderer;
        [SerializeField] private Animator animator;
        
        private Color _baseColor;
        
        private void Start()
        {
            _baseColor = portalRenderer.material.color;
        }

        public void InitLaunch()
        {
            portalRenderer.material.color = Color.red;
        }

        public void Launch()
        {
            GameObject missilGO = PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.Missile, transform.position, quaternion.identity);
            //missilGO.transform.LookAt(transform.position + transform.forward, transform.up);
            var missile = missilGO.GetComponent<Missile>();
            missile.SetTarget(PlayerController.instance.gameObject);
            
            portalRenderer.material.color = _baseColor;
            
        }

        void GetDestroy()
        {
            _monolith.DestroyPortal(this);
            animator.SetTrigger("Desactive");
            portalRenderer.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) GetDestroy();
        }
    }
}