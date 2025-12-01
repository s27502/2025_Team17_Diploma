public class DamagePopupPool : ObjectPool
{
    public DamagePopupPool(IObjectFactory factory, int maxSize)
        : base(factory, maxSize)
    {
        
    }
    
    public override IPoolableObject GetObject()
    {
        var obj = base.GetObject();
        if (obj != null)
            obj.SetPool(this);

        return obj;
    }

}