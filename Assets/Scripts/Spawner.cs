using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    public DataLoader _dataLoader;
    public GameObject personprefab;
    [FormerlySerializedAs("personList")] public List<GameObject> personObjectList = new List<GameObject>();
    public void SpawnPersons()
    {
        // here i'm spawning the playerobjects based on the persons from the dataloader class.
        // basicly putting in the data i retrieved from the json and put inside the personlist to put it in the gameobjects
        // i also assigned their names to be equal to the idea so they're easier to distinguish.
        Vector3 position = new Vector3(0, 0, 0);
        foreach (Person person in _dataLoader.persons)
        {
            GameObject personObject = Instantiate(personprefab, position, Quaternion.identity);
            personObjectList.Add(personObject);
            personObject.GetComponent<Movement>().PersonId = person.Id;
            personObject.name = "Person" + person.Id;
        }
    }
}
