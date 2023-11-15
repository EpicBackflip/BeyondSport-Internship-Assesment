using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int PersonId;
    public Spawner spawner;
    public Material blueColor;
    public Material redColor;
    public GameObject ballprefab;
    private DataLoader _dataLoader;

    public void SetColor(List<Person> persons)
    {
        for (int i = 0; i < persons.Count; i++)
        {
            if (spawner.personList[i] != null)
            {
                if (persons[i].TeamSide == 2)
                {
                    spawner.personList[i].GetComponent<MeshRenderer>().material = redColor;
                }
                else
                {
                    spawner.personList[i].GetComponent<MeshRenderer>().material = blueColor;
                }
            }
        }
    }

    public void UpdatePersonPositions(List<Person> persons,Ball ball)
    {
        Vector3 ballPosition = new Vector3((float)ball.Position[0], (float)ball.Position[1],(float)ball.Position[2]);
        ballprefab.transform.position = ballPosition;
        for (int i = 0; i < persons.Count; i++)
        {
            if (spawner.personList[i] != null)
            {
                Vector3 newPosition = new Vector3((float)persons[i].Position[0], (float)persons[i].Position[1], (float)persons[i].Position[2]);
                spawner.personList[i].transform.position = newPosition;
            }
        }
    }
}
