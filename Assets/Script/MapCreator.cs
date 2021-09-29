using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private SpaceShipHandler _spaceShipHandler;
    public GameObject[] MapsType;
    public GameObject SpaceShip;
    private Vector3 NextSpawnPoint;
    public List<GameObject> MapsPart = new List<GameObject>();
    private Random _random = new Random();
    private int MapsPartInMap = 6;

    public void StartGame()
    {
        Vector3 tmpVector = SpaceShip.transform.position;
        NextSpawnPoint = new Vector3(tmpVector.x, tmpVector.y, tmpVector.z + 20);
        CreatMap();
        CreatMap();
        _spaceShipHandler.SpaceShipControl();//start control safine
        StartCoroutine(MapCheker());//start chek kardan masir
    }
    
    IEnumerator MapCheker() //chek kardan fasele baraye ejad masir
    {
        while (true){
            if (Vector3.Distance(SpaceShip.transform.position, NextSpawnPoint) < 70)
                CreatMap();
            yield return new WaitForFixedUpdate();
        }    
    }
    
    void CreatMap() //ejad masir
    {
        int MapsTypeIndex = _random.Next(0, MapsType.Length);
        GameObject tmp = Instantiate(MapsType[MapsTypeIndex], NextSpawnPoint, Quaternion.identity);
        NextSpawnPoint = tmp.transform.GetChild(1).transform.position;
        MapsPart.Add(tmp.gameObject);
        if (MapsPart.Count > MapsPartInMap)
        {
            Destroy(MapsPart[0]);
            MapsPart.Remove(MapsPart[0]);
        }
    }
}
