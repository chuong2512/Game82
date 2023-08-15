using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameController<TGameState> : BaseGameController where TGameState : GameState
{
    protected TGameState gameState;
    
    protected void SaveGameState()
    {
        gameState.SaveGameState();
    }

    protected override void BoosterExecute<T>(T target)
    {
        SaveGameState();

        bool boosterProceeded = true;

        switch (boosterType)
        {
            case BoosterType.ClearBrick when target is NumberedBrick brick:
                ClearBrick.Execute(field, GetCoords(brick), OnBoostersComplete);
                break;
        }
        
        BoostersController.Instance.OnBoosterProceeded(boosterProceeded);
    }
}

public abstract class BaseGameController : MonoBehaviour
{
    public event Action GameOver = delegate { };

    [Header("Base fields")] 
    public Vector2Int bricksCount;
    public RectTransform fieldTransform;
    public NumberedBrick brickPrefab;

    public SoundCollection soundCollection;

    [Range(0f, 1f)] public float coinProbability;

    public Animator fieldAnimator;

    protected NumberedBrick[,] field;
    
    protected bool isBoosterSelected;
    protected BoosterType boosterType;
    protected string destroyVfx = "DestroyVFX";

    public int HighlightSortingOrder { get; set; }

    protected abstract void StartGame();
    
    protected abstract void SaveGame();

    protected void OnGameOver()
    {
        GameOver.Invoke();
    }

    /// <summary>
    /// Basic puzzles algorithms
    /// </summary>
    protected virtual IEnumerable<Vector2Int> GetAdjacentCoords(Vector2Int coords)
    {
        List<Vector2Int> adjacent = new List<Vector2Int>();

        Vector2Int up = new Vector2Int(coords.x, coords.y + 1);
        if (up.y < field.GetLength(1))
            adjacent.Add(up);

        Vector2Int down = new Vector2Int(coords.x, coords.y - 1);
        if (down.y >= 0)
            adjacent.Add(down);

        Vector2Int left = new Vector2Int(coords.x - 1, coords.y);
        if (left.x >= 0)
            adjacent.Add(left);

        Vector2Int right = new Vector2Int(coords.x + 1, coords.y);
        if (right.x < field.GetLength(0))
            adjacent.Add(right);

        return adjacent;
    }

    protected IEnumerable<Vector2Int> GetAdjacentAreaCoords(Vector2Int coords)
    {
        List<Vector2Int> adjacent = GetAdjacentCoords(coords) as List<Vector2Int>;

        Vector2Int upLeft = new Vector2Int(coords.x - 1, coords.y + 1);
        if (upLeft.x >= 0 && upLeft.y < field.GetLength(1))
            adjacent.Add(upLeft);

        Vector2Int upRight = new Vector2Int(coords.x + 1, coords.y + 1);
        if (upRight.x < field.GetLength(0) && upRight.y < field.GetLength(1))
            adjacent.Add(upRight);

        Vector2Int downLeft = new Vector2Int(coords.x - 1, coords.y - 1);
        if (downLeft.x >= 0 && downLeft.y >= 0)
            adjacent.Add(downLeft);

        Vector2Int downRight = new Vector2Int(coords.x + 1, coords.y - 1);
        if (downRight.x < field.GetLength(0) && downRight.y >= 0)
            adjacent.Add(downRight);

        return adjacent;
    }

    protected virtual Vector2 GetBrickPosition(Vector2 coords)
    {
        Vector2 brickSize = GetBrickSize();
        RectTransform brickTransform = brickPrefab.GetComponent<RectTransform>();

        Vector2 brickPosition = Vector2.Scale(coords, brickSize);
        brickPosition += Vector2.Scale(brickSize, brickTransform.pivot);

        return brickPosition;
    }

    protected Vector2 GetBrickSize()
    {
        Rect rect = fieldTransform.rect;
        Vector2 brickSize = new Vector2
        {
            x = rect.width / bricksCount.x,
            y = rect.height / bricksCount.y
        };

        return brickSize;
    }

    protected Vector2 GetWorldBrickSize()
    {
        Vector3[] worldCorners = new Vector3[4];
        fieldTransform.GetWorldCorners(worldCorners);
        Vector2 brickSize = new Vector2
        {
            x = (worldCorners[2].x - worldCorners[0].x) / bricksCount.x,
            y = (worldCorners[2].y - worldCorners[0].y) / bricksCount.y
        };

        return brickSize;
    }

    protected Vector2Int BrickPositionToCoords(Vector3 position, Vector2 pivot)
    {
        Vector3[] worldCorners = new Vector3[4];
        fieldTransform.GetWorldCorners(worldCorners);

        Vector2 brickSize = GetWorldBrickSize();

        Vector2 localPoint = position - worldCorners[0] - Vector3.Scale(brickSize, pivot);
        Vector2 coords = localPoint / brickSize;

        return Vector2Int.RoundToInt(coords);
    }
    
    /// <summary>
    /// Last chance implementation
    /// </summary>

    public virtual void LastChance(LastChance lastChance)
    {
        switch (lastChance.LastChanceType)
        {
            case LastChanceType.Numbers:
                ClearNumbers.Execute(field, lastChance.MaxNumber, OnLastChanceCompleted);
                break;
            case LastChanceType.CrossLines:
                ClearCrossLines.Execute(field, OnLastChanceCompleted);
                break;
            case LastChanceType.LinesHorizontal:
                int coordY = (bricksCount.y - lastChance.LinesCount) / 2;
                ClearHorizontalLines.Execute(field, coordY, lastChance.LinesCount, OnLastChanceCompleted);
                break;
            case LastChanceType.LinesVertical:
                int coordX = (bricksCount.x - lastChance.LinesCount) / 2;
                ClearVerticalLines.Execute(field, coordX, lastChance.LinesCount, OnLastChanceCompleted);
                break;
            case LastChanceType.Explosion:
                var coords = new Vector2Int(bricksCount.x / 2, bricksCount.y / 2);
                AnimateDestroy(coords, OnLastChanceCompleted);
                return;
        }
    }

    protected abstract void OnLastChanceCompleted();

    protected void AnimateDestroy(Vector2Int coords, Action onComplete)
    {
        field.DestroyBrick(coords, null);

        IEnumerable<Vector2Int> adjacentCoords = GetAdjacentAreaCoords(coords);
        this.DelayedCall(0.25f, () => Explosion.Execute(field, adjacentCoords, null));
        
        SpawnDestroyAnimation(coords, onComplete);
    }

    /// <summary>
    /// Boosters basic implementation
    /// </summary>
    
    public virtual void HighlightBoosterTarget(BoosterType type, bool active)
    {
        isBoosterSelected = active;
        boosterType = type;

        switch (type)
        {
            case BoosterType.ClearBrick:
                HighlightBricks(active);
                break;
        }
    }

    protected virtual void HighlightField(bool active)
    {
        SetSortingOrder(fieldTransform.gameObject, active);
    }

    protected virtual void HighlightBricks(bool active)
    {
        for (int x = 0; x < bricksCount.x; x++)
        {
            for (int y = 0; y < bricksCount.y; y++)
            {
                if(field[x, y] == null) continue;

                SetSortingOrder(field[x, y].gameObject, active);
            }
        }
    }
    
    protected void SetSortingOrder(GameObject obj, bool active)
    {
        if (!obj.TryGetComponent(out SortingOrderApplier applier))
            applier = obj.AddComponent<SortingOrderApplier>();
                
        if (active)
            applier.SetSortingOrder(HighlightSortingOrder);
        else
            applier.Hide();
    }

    protected void OnHighlightedTargetClick<T>(T target)
    {
        if (!isBoosterSelected) return;

        soundCollection.GetSfx(SoundId.Destroying).Play();

        BoosterExecute(target);
    }

    protected abstract void BoosterExecute<T>(T target);

    protected virtual void OnBoostersComplete()
    {
        SaveGame();
    }
    
    protected Vector2Int GetCoords(Brick brick)
    {
        Vector2 pivot = brick.GetComponent<RectTransform>().pivot;
        Vector2Int coords = BrickPositionToCoords(brick.transform.position, pivot);

        return coords;
    }
    
    void SpawnDestroyAnimation(Vector2Int coords, Action onComplete)
    {
        GameObject vfx = Resources.Load<GameObject>(destroyVfx);
        vfx = Instantiate(vfx, fieldTransform);
        
        var rectTransform = vfx.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.anchoredPosition = GetBrickPosition(coords);

        Vector2 brickSize = GetBrickSize();
        Vector2 delta = GetBrickSize() - brickSize;
        brickSize *= 3;
        brickSize += delta * 2;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, brickSize.x);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, brickSize.y);

        this.DelayedCall(1f, () =>
        {
            Destroy(vfx);
            onComplete?.Invoke();
        });
    }
}