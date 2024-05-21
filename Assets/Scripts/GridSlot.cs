using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public int row, column;
    private Disc _disc;
    public void SetRowColumn(int i, int j)
    {
        row = j;
        column = i;
    }
    public void SetDisc(Disc disc)
    {
        _disc = disc;
    }

    public CurrentPlayer GetPlayer()
    {
        return _disc == null ? CurrentPlayer.None : _disc.GetPlayer();
    }
}
