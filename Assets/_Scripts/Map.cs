using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Map : MonoBehaviour
{
    [SerializeField] private List<AccessPoint> _accessPoints;
    [SerializeField] private int _difficulty = 1;

    // Start is called before the first frame update
    void Start()
    {
        var shuffled = ShuffleAccessPoints(_accessPoints);
        var taken = shuffled.Take(_difficulty).ToList();

        for (int i = 0; i < taken.Count; i++)
        {
            if (i + 1 < taken.Count)
            {
                taken[i].Activate(taken[i + 1]);
                taken[i].ShowActivation(i);
            } else
            {
                taken[i].Activate();
                taken[i].ShowActivation(taken.Count);
            }
            taken[i].ObjectToRemove = GameManager.Instance._objectsToRemove[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<AccessPoint> ShuffleAccessPoints(List<AccessPoint> list)
    {
        var listCopy = list;
        var random = new Random();
        List<AccessPoint> newShuffledList = new();
        var listcCount = newShuffledList.Count;
        for (int i = 0; i < list.Count; i++)
        {
            var randomElementInList = random.Next(0, listCopy.Count);
            newShuffledList.Add(listCopy[randomElementInList]);
            listCopy.Remove(listCopy[randomElementInList]);
        }
        return newShuffledList;
    }
}
