using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerBehavior : EnemyBehavior
{
    private float shieldTime;
    [SerializeField] private VolumeProfile shieldBreak;
    [SerializeField] private AnimationCurve shieldBreakCurve;
    protected override void Start()
    {
        base.Start();
        UIManager.Instance.UpdateStats(life,shield, maxLife);
    }

    public override void Hit(int damage = 1, bool overflow = false)
    {
        base.Hit(damage, overflow);
        UIManager.Instance.UpdateStats(life,shield, maxLife);
    }

    private void FixedUpdate()
    {
        if (shield <= 0 && shieldTime < 70)
        {
            ShieldBreakFeedback();
            
        }
        if (shield > 0) shieldTime = 0;
    }

    public void ShieldBreakFeedback()
    {
        GameManager.instance.volumeManager.profile = shieldBreak;
        GameManager.instance.volumeManager.weight = shieldBreakCurve.Evaluate(shieldTime/70);
        shieldTime++;
    }
    public override bool IsPlayer()
    {
        return true;
    }

    protected override void UpdateStats()=> UIManager.Instance.UpdateStats(life,shield, maxLife);

    protected override void Destroy()
    {
        UIManager.Instance.GameOver();
    }
}
