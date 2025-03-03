using NaughtyAttributes;
using UnityEngine;
using static GridEnemy;

public class GridSleepingEnemy : GridObject
{
    GameObject _enemy;
    [SerializeField] private MovementType _robotMovementType;
    bool IsVertical() { return _robotMovementType == MovementType.Vertical; }
    bool IsHorizontal() { return _robotMovementType == MovementType.Horizontal; }
    [SerializeField, ShowIf(nameof(IsVertical))] private VerticalInitialDir _verticalInitialDirection;
    [SerializeField, ShowIf(nameof(IsHorizontal))] private HorizontalInitialDir _horizontalInitialDirection;
    private SpriteRenderer _skin;
    [SerializeField] private Sprite _spriteVertical, _spriteHorizontal;

    protected override void Setup()
    {
        base.Setup();
        _skin = GetComponent<SpriteRenderer>();
        if (IsVertical()) { _skin.sprite = _spriteVertical; } else {  _skin.sprite = _spriteHorizontal; }
        SetImpassable();
        _enemy = Resources.Load<GameObject>("GDTools Prefabs/Grid Objects/Enemy");
        Oxygen.OnUnderOxygenThreshold += WakeYoAssUp;
    }

    public void TransferMovementParams(MovementType movement, VerticalInitialDir dir)
    {
        _robotMovementType = movement;
        _verticalInitialDirection = dir;
    }

    public void TransferMovementParams(MovementType movement, HorizontalInitialDir dir)
    {
        _robotMovementType = movement;
        _horizontalInitialDirection = dir;
    }

    void WakeYoAssUp()
    {
        GridEnemy temp = Instantiate(_enemy, transform.position, Quaternion.identity).GetComponent<GridEnemy>();
        if (IsVertical()) { temp.TransferMovementParams(_robotMovementType, _verticalInitialDirection); }
        else { temp.TransferMovementParams(_robotMovementType, _horizontalInitialDirection); }
        Oxygen.OnUnderOxygenThreshold -= WakeYoAssUp;
        Destroy(gameObject);
    }
}
