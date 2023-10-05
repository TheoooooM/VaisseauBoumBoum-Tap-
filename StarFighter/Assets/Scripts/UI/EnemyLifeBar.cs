using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeBar : MonoBehaviour
{
    [SerializeField] private Image lifeBar;
    [SerializeField] private Image shieldBar;

    private void Update()
    {
        transform.LookAt(PlayerController.instance.transform.position);
    }

    public void UpdateStats(int currentLife, int currentShield, int maxLife)
    {
        UpdateLife(currentLife, maxLife);
        UpdateShield(currentShield, maxLife);
    }
    public void UpdateLife(int currentLife, int maxLife) => lifeBar.fillAmount = (float)currentLife / maxLife;
    public void UpdateShield(int currentShield, int maxLife) => shieldBar.fillAmount = Mathf.Clamp((float)currentShield / maxLife,0,1);
}
