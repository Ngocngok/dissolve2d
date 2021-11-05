using Sirenix.OdinInspector;
using UnityEngine;

public class AutoDestroy : HCMonobehavior
{
    [SerializeField]
    public float timeOut = 0.5f;

    public TYPE_DESTROY typeDestroy = TYPE_DESTROY.DISABLE;

    [ShowIf("typeDestroy", TYPE_DESTROY.DESPAWN)]
    public POOL Pool;

    float timeStart;

    private void OnEnable()
    {
        timeStart = Time.time;
    }

    public override void OnSpawned()
    {
        timeStart = Time.time;
    }

    void Update()
    {
        if (Time.time - timeStart > timeOut)
            Action();
    }

    public void Action()
    {
        if (typeDestroy == TYPE_DESTROY.DISABLE)
            gameObject.SetActive(false);
        else if (typeDestroy == TYPE_DESTROY.DESPAWN)
            gameObject.Despawn(Pool);
        else if (typeDestroy == TYPE_DESTROY.DESTROY)
            Destroy(gameObject);
    }
}

public enum TYPE_DESTROY
{
    DISABLE,
    DESPAWN,
    DESTROY
}