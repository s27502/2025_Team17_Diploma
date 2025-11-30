using UnityEngine;

public class DamagePopupFactory : IObjectFactory
{
    private GameObject _prefab;

    public DamagePopupFactory(GameObject prefab)
    {
        _prefab = prefab;
    }

    public IPoolableObject CreateNew()
    {
        GameObject obj = Object.Instantiate(_prefab);
        var popup = obj.GetComponent<DamagePopup>();
        return popup;
    }
}