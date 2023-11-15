using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public DataLoader _dataLoader;
    public GameObject personprefab;
    public List<GameObject> personList = new List<GameObject>();
    public void SpawnPersons()
    {
        Vector3 position = new Vector3(0, 0, 0);
        foreach (int personId in _dataLoader.personIds)
        {
            GameObject personObject = Instantiate(personprefab, position, Quaternion.identity);
            personList.Add(personObject);
            personObject.GetComponent<Movement>().PersonId = personId;
            personObject.name = "Person" + personId;
        }
    }
}
