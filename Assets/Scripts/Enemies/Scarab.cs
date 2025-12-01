namespace Enemies
{
    public class Scarab : Enemy
    {
        protected override void Attack()
        {
            MoveToPlayer();
        }

        protected override void Idle()
        {
            base.Idle();
        }
    }
}