using System.Collections.Generic;
using Constants;
using Managers;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Slot[] slots;
    [SerializeField] private GridSlot gridSlotPrefab;
    [SerializeField] private List<GridSlot> gridSlots = new List<GridSlot>();
    [SerializeField] private List<Disc> activeDiscs = new List<Disc>();
    [SerializeField] private GameObject boardBottom;
    [SerializeField] private GameObject separators;
    [SerializeField] private float spawnHeight = 1.5f;
    [SerializeField] private float cooldown = 0;
    [SerializeField] private int previousColumn = -1;

    [Header("Grid Settings")]
    [SerializeField] private int rows = 6;
    [SerializeField] private int columns = 7;
    [SerializeField] private float discHeightMultiplier = 0.265f;
    [SerializeField] private float discWidthMultiplier = 0.33f;
    [SerializeField] private float boardHeight = 1.03f;
    [SerializeField] private float boardWidth = 1;

    [Header("Materials")]
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material yellowMaterial;

    public static Board Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        spawnHeight = slots[0].GetGuideHeight();
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (var i = 0; i < columns; i++)
        {
            for (var j = 0; j < rows; j++)
            {
                var position = new Vector3(i * discWidthMultiplier - boardWidth, j * discHeightMultiplier + boardHeight, 0);
                var gridSlotInstance = Instantiate(gridSlotPrefab, position, Quaternion.identity, transform);
                gridSlotInstance.SetRowColumn(i, j);
                gridSlots.Add(gridSlotInstance);
            }
        }
    }

    public Vector3 GetGridPosition(int column, int row)
    {
        if (row <= rows && column <= columns)
        {
            var gridSlot = gridSlots.Find(g => g.column == column && g.row == row);
            if (gridSlot != null)
            {
                return gridSlot.transform.position;
            }
        }
        Debug.Log($"Invalid row or column: {row}, {column}");
        return Vector3.zero;
    }

    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    public void EndGame()
    {
        Invoke(nameof(ShootDiscsInAir), 1f);
    }

    public void ShootDiscsInAir()
    {
        separators.SetActive(false);
        foreach (var disc in activeDiscs)
        {
            disc.Shoot();
        }
        Invoke(nameof(ReturnDiscs), 2f);
    }

    private void ReturnDiscs()
    {
        foreach (var disc in activeDiscs)
        {
            disc.Reset();
        }
    }

    public Slot[] GetSlots() => slots;

    public void ShowSpawnGuide(int column)
    {
        if (cooldown > 0.01f || previousColumn == column) return;

        if (previousColumn != -1)
        {
            slots[previousColumn].SetGuide(false);
        }

        if (!slots[column].isFull)
        {
            slots[column].SetGuide(true);
        }

        previousColumn = column;
    }

    public void DropDisc(int column, Disc disc)
    {
        if (slots[column].isFull) return;

        slots[column].SetGuide(false);
        activeDiscs.Add(disc);
        slots[column].AddDisc(disc);
        cooldown = 0.5f;

        var lastSlot = gridSlots.Find(g => g.column == column && g.row == slots[column].discs.Count - 1);
        CheckForWin(lastSlot);
    }

    public float GetSpawnHeight() => spawnHeight;

    public Material GetMaterial(CurrentPlayer player) => player == CurrentPlayer.Player1 ? redMaterial : yellowMaterial;

    public void SetGridSlot(int column, int discsCount, Disc disc)
    {
        var gridSlot = gridSlots.Find(g => g.column == column && g.row == discsCount);
        gridSlot?.SetDisc(disc);
    }

    private void CheckForWin(GridSlot lastSlot)
    {
        var lastPlayer = GameManager.Instance.GetCurrentPlayer();
        if (lastPlayer == CurrentPlayer.None) return;

        if (CheckDirection(lastSlot, 1, 0, lastPlayer) || // Horizontal
            CheckDirection(lastSlot, 0, 1, lastPlayer) || // Vertical
            CheckDirection(lastSlot, 1, 1, lastPlayer) || // Diagonal /
            CheckDirection(lastSlot, 1, -1, lastPlayer))  // Diagonal \
        {
            Win(lastPlayer);
        }
    }

    private void Win(CurrentPlayer player)
    {
        GameManager.Instance.EndGame(player);
    }

    private bool CheckDirection(GridSlot startSlot, int rowIncrement, int colIncrement, CurrentPlayer player)
    {
        int count = 1;
        count += CountInDirection(startSlot, rowIncrement, colIncrement, player);
        count += CountInDirection(startSlot, -rowIncrement, -colIncrement, player);
        return count >= 4;
    }

    private int CountInDirection(GridSlot startSlot, int rowIncrement, int colIncrement, CurrentPlayer player)
    {
        int count = 0;
        int currentRow = startSlot.row + rowIncrement;
        int currentColumn = startSlot.column + colIncrement;

        while (IsValidSlot(currentRow, currentColumn) &&
               gridSlots.Find(slot => slot.row == currentRow && slot.column == currentColumn).GetPlayer() == player)
        {
            count++;
            currentRow += rowIncrement;
            currentColumn += colIncrement;
        }

        return count;
    }

    private bool IsValidSlot(int row, int column) => row >= 0 && row < rows && column >= 0 && column < columns;
}
