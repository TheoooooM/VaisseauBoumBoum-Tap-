public class PlayerBehavior : EnemyBehavior
{
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

    public override bool IsPlayer()
    {
        return true;
    }

    protected override void UpdateStats()=> UIManager.Instance.UpdateStats(life,shield, maxLife);

    protected override void Destroy()
    {
        //TODO : Game over
        Destroy(gameObject);
    }
}
