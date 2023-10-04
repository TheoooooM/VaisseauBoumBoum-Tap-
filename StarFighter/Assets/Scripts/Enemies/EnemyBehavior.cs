using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] int maxLife = 100;
    [SerializeField] int maxShield = 30;
    [FormerlySerializedAs("OnLifeShieldCooldown")]
    [Header("Shield Regenation")] 
    [SerializeField] private float OnLifeCooldown = 4f;
    [SerializeField] private float OnShieldCooldown = 1f;
    [SerializeField] private float regenCooldown = .2f;
    /*[Header("Debug")]
    [SerializeField]TextMeshProUGUI lifeText;
    [SerializeField]TextMeshProUGUI shieldText;*/

    protected int life;
    private int shield;

    private bool isShield;

    private Coroutine ShieldRegenCoroutine;

    protected virtual void Start()
    {
        life = maxLife;
        shield = maxShield;
        isShield = shield > 0;
    }

    private void Update()
    {
        // if(Input.GetKeyDown(KeyCode.H))Hit();
        //
        //
        // lifeText.text = $"Life: {life}";
        // shieldText.text = $"Shield: {shield}";
    }

    public virtual void Hit(int damage = 1)
    {
        if (isShield)
        {
            shield -= damage;
            if (shield <= 0) BreakShield();
        }
        else
        {
            life -= damage;
            if(life <= 0) Destroy();
        }
        ResetShieldRegen();
    }

    void ResetShieldRegen()
    {
        if (ShieldRegenCoroutine != null) StopCoroutine(ShieldRegenCoroutine);
        ShieldRegenCoroutine = StartCoroutine(ShieldCoolDown());
    }

    void BreakShield()
    {
        isShield = false;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    IEnumerator ShieldCoolDown()
    {
        if (!isShield) yield return new WaitForSeconds(OnLifeCooldown);
        else yield return new WaitForSeconds(OnShieldCooldown);
        isShield = true;
        shield++;
        ShieldRegenCoroutine = StartCoroutine(ShieldRegen());
    }

    IEnumerator ShieldRegen()
    {
        yield return new WaitForSeconds(regenCooldown);
        shield++;
        if (shield < maxShield) ShieldRegenCoroutine = StartCoroutine(ShieldRegen()); 
    }
}
