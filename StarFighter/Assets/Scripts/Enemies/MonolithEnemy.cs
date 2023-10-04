using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class MonolithEnemy : EnemyBehavior
    {
        [Header("Monolith")] 
        [SerializeField] private List<PortalMonolith> portals;
        [Space]
        [SerializeField] private Vector2Int amountMissilLaunch = new(1,3);

        [Space] [SerializeField] private float launchCooldown = 10f;
        [SerializeField] float initLaunchDelay = 3f;
        [SerializeField] private Vector2 fireMissilsDelayRange = new(.2f, .5f);

        protected override void Start()
        {
            life = portals.Count;
            StartCoroutine(LaunchCooldDown());
        }

        public override void Hit(int damage = 1) { }

        public void DestroyPortal(PortalMonolith portal)
        {
            if (portals.Contains(portal)) portals.Remove(portal);
            life--;
        }

        IEnumerator LaunchMissiles()
        {
            var amountLaunch = Random.Range(amountMissilLaunch.x, amountMissilLaunch.y+1);
            var availablePortals = portals;
            if (amountLaunch > availablePortals.Count) amountLaunch = availablePortals.Count;
            List<PortalMonolith> usedPortals = new List<PortalMonolith>();
            for (int i = 0; i < amountLaunch; i++)
            {
                var index = Random.Range(0, availablePortals.Count);
                usedPortals.Add(availablePortals[index]);
                availablePortals.RemoveAt(index);
            }

            foreach (var portal in usedPortals)
            {
                portal.InitLaunch();
            }

            yield return new WaitForSeconds(initLaunchDelay);

            while (usedPortals.Count > 0)
            {
                usedPortals[^1].Launch();
                yield return new WaitForSeconds(Random.Range(fireMissilsDelayRange.x,fireMissilsDelayRange.y));
                usedPortals.RemoveAt(usedPortals.Count-1);
            }
            
            StartCoroutine(LaunchCooldDown());
        }

        IEnumerator LaunchCooldDown()
        {
            yield return new WaitForSeconds(launchCooldown);
            StartCoroutine(LaunchMissiles());
        }
    }
}