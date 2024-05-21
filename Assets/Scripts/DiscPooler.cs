using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscPooler : MonoBehaviour
{
    public static DiscPooler Instance;
    [SerializeField] private int poolSize;
    [SerializeField] private Disc discPrefab;
    
    private List<Disc> _discs = new List<Disc>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Disc newDisc = Instantiate(discPrefab, transform);
            newDisc.gameObject.SetActive(false);
            _discs.Add(newDisc);
        }
    }
    
    public Disc GetDisc()
    {
        Disc disc = _discs.FirstOrDefault(d => !d.gameObject.activeInHierarchy);
        if (disc == null)
        {
            disc = Instantiate(discPrefab, transform);
            _discs.Add(disc);
        }
        disc.gameObject.SetActive(true);
        return disc;
    }
    
    public void ReturnDisc(Disc disc)
    {
        disc.gameObject.SetActive(false);
        _discs.Add(disc);
    }
}
