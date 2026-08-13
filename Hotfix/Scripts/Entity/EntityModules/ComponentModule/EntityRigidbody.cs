using UnityEngine;

public abstract class EntityRigidbody2DData : SceneEntityData<EntityRigidbody2D>
{
    private Vector3 _MoveRigibodyPos;

    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
        _MoveRigibodyPos = default;
    }
    public void AddForce(Vector2 force, ForceMode2D mode)
    {
        if (!_IsLoadSuccess)
            return;
        _SceneEntity.AddForce(force, mode);
    }
    public void Sleep()
    {
        if (!_IsLoadSuccess)
            return;
        _SceneEntity.Sleep();
    }
    public void MoveRigibody(Vector3 worldPos)
    {
        _MoveRigibodyPos = worldPos;
        if (!_IsLoadSuccess)
        {
            SetPosition(worldPos);
            return;
        }
        _SceneEntity.MoveRigibody(worldPos);
    }

}
public abstract class EntityRigidbody2D : SceneEntity
{
    [SerializeField] private Rigidbody2D _Rigidbody2D = null;

    public override void LoadCompeletion()
    {
        base.LoadCompeletion();
        _Rigidbody2D.position = EntityMgr.Instance.GetEntityWorldPos(_EntityID);
    }

    public void AddForce(Vector2 force, ForceMode2D mode)
    {
        _Rigidbody2D.AddForce(force, mode);
    }
    public void Sleep()
    {
        _Rigidbody2D.Sleep();
    }
    public void MoveRigibody(Vector3 worldPos)
    {
        _Rigidbody2D.MovePosition(worldPos);
    }
    public override void SetPosition(in Vector3 worldPos)
    {
        // _Rigidbody2D.MovePosition(_EntityRigidbodyData.MoveRigibodyPos);
    }
    protected override void Update()
    {
        base.Update();
        EntityMgr.Instance.SetEntityWorldPos(_EntityID, transform.position);
    }
}
