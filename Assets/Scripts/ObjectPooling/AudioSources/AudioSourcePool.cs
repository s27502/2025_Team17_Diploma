namespace ObjectPooling.AudioSources
{
    public class AudioSourcePool : ObjectPool
    {
        public AudioSourcePool(IObjectFactory factory, int maxSize) : base(factory, maxSize)
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
}