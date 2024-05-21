using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int column;
    [SerializeField] private Renderer guideRenderer;
    [SerializeField] public List<Disc> discs = new List<Disc>();
    public bool isFull;
    private Board _board;

     private void Start()
     {
         _board = GetComponentInParent<Board>();
     }

     public float GetGuideHeight()
    {
        return guideRenderer.transform.position.y;
    }

    public void SetGuide(bool state)
    {
        guideRenderer.material = _board.GetMaterial(GameManager.Instance.GetCurrentPlayer());
        guideRenderer.enabled = state;
    }

    public void AddDisc(Disc disc)
    {
        discs.Add(disc);
        disc.SetColumnRow(column, discs.Count - 1);
        _board.SetGridSlot(column, discs.Count - 1, disc);
        if(discs.Count > 5) isFull = true;
    }
}
