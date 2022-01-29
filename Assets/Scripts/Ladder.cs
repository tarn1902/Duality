using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DavidFDev.Tweening;

[RequireComponent(typeof(BoxCollider))]
public sealed class Ladder : Transformation
{
    #region Fields

    private readonly List<GameObject> _ladder = new List<GameObject>();

    private BoxCollider _collider;

    #endregion

    #region Properties

    public override Form TransformationForm => Form.Ladder;

    [field: SerializeField]
    public GameObject LadderTop { get; private set; }

    [field: SerializeField]
    public GameObject LadderPiece { get; private set; }

    [field: SerializeField]
    public GameObject LadderBottom { get; private set; }

    [field: SerializeField]
    public int MaxLength { get; private set; } = 15;

    [field: SerializeField]
    public float LadderPieceHeight { get; private set; }

    [field: SerializeField]
    public LayerMask SolidLayer { get; private set; }

    #endregion

    #region Methods

    protected override void OnTransformationEnabled()
    {
    }

    protected override void OnTransformationDisabled()
    {
    }

    protected override void OnAbilityEnabled()
    {
        // Check for initial valid placement area

        // Place ladder 
        PlaceLadder(MousePlayer.transform.position);
    }

    protected override void OnAbilityDisabled()
    {
        RemoveLadder();
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    private void PlaceLadder(Vector3 pos)
    {
        // Place top piece
        AddToLadder(LadderTop, pos);

        // Place rungs
        do
        {
            AddToLadder(LadderPiece, pos);
            pos.y -= 0.5f;
        } while (!IsSolid(pos, out _) && _ladder.Count < MaxLength);

        // Place bottom piece
        pos.y += 0.5f;
        AddToLadder(LadderBottom, pos);

        FitCollider();
        _collider.enabled = true;

        MousePlayer.IsMovementDisabled = true;
        transform.localEulerAngles = Vector3.zero;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void RemoveLadder()
    {
        while (_ladder.Any())
        {
            RemoveFromLadder(_ladder.Last());
        }

        _collider.enabled = false;

        MousePlayer.IsMovementDisabled = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void AddToLadder(GameObject prefab, Vector3 pos)
    {
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.SetActive(true);
        _ladder.Add(obj);

        // Animate
        obj.transform.TweenScale(Vector3.zero, obj.transform.localScale, 0.4f, Ease.ElasticOut);
    }

    private void RemoveFromLadder(GameObject instance)
    {
        _ladder.Remove(instance);

        // Animate destruction
        instance.transform.TweenScale(instance.transform.localScale, Vector3.zero, 0.3f, Ease.QuadIn, true, null, () => Destroy(instance));
    }

    private void FitCollider()
    {
        Vector3 center = _collider.center;
        Vector3 size = _collider.size;
        center.y = _ladder.Count * LadderPieceHeight * -0.5f + 4f;
        size.y = _ladder.Count * LadderPieceHeight;
        _collider.center = center;
        _collider.size = size;
    }

    private bool IsSolid(Vector3 pos, out Collider result)
    {
        return (result = Physics.OverlapSphere(pos, 0.4f, SolidLayer.value).FirstOrDefault()) != null;
    }

    #endregion
}
