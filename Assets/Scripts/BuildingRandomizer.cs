using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRandomizer : MonoBehaviour
{
    public List<GameObject> Buildings;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Buildings[Random.Range(0, Buildings.Count)], transform);
    }
}
