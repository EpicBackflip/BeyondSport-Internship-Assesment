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
    private int redTeam = 2;
    
    // here i'm setting the color based on the teamside of the players
    public void SetColor(List<Person> persons)
    {
        for (int i = 0; i < persons.Count; i++)
        {
            if (spawner.personObjectList[i] != null)
            {
                if (persons[i].TeamSide == redTeam)
                {
                    spawner.personObjectList[i].GetComponent<MeshRenderer>().material = redColor;
                }
                else
                {
                    spawner.personObjectList[i].GetComponent<MeshRenderer>().material = blueColor;
                }
            }
        }
    }

    // here i'm setting the ball position to be that of the ball from the json 
    // and doing the same for the players. I've tried using rigidbodies for the movement but they were very laggy and didn't work very well.
    public void UpdatePersonPositions(List<Person> persons,Ball ball)
    {
        Vector3 ballPosition = new Vector3((float)ball.Position[0], (float)ball.Position[1],(float)ball.Position[2]);
        ballprefab.transform.position = ballPosition;
        for (int i = 0; i < persons.Count; i++)
        {
            if (spawner.personObjectList[i] != null)
            {
                Vector3 newPosition = new Vector3((float)persons[i].Position[0], (float)persons[i].Position[1], (float)persons[i].Position[2]);
                spawner.personObjectList[i].transform.position = newPosition;
            }
        }
    }
}
