using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    private List<int> frameCounts;
    private string filePath;
    public List<Root> frames;
    public List<int> personIds = new List<int>();
    public IEnumerable<Root> roots;
    public int currentFrameCount;
    public Spawner spawner = new Spawner();
    public PersonObject personObject = new PersonObject();

    private void Start()
    {
        ReadJson();
    }

    public void ReadJson()
    {
        filePath = $"{Application.persistentDataPath} /Applicant-test.idf";
        roots = getRoots(filePath);
        frameCounts = roots.Select(x => x.FrameCount).ToList();
        frames = roots.ToList();
        foreach (Root frame in frames)
        {
            foreach (Person person in frame.Persons)
            {
                if (!personIds.Contains(person.Id))
                {
                    personIds.Add(person.Id);
                }
            }
        }
        spawner.SpawnPersons();
    }
    
    public static IEnumerable<Root> getRoots(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path to json file cannot be null or whitespace.", nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Could not find json file to parse!", path);
        }

        foreach (string line in File.ReadLines(path).Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            Root root = JsonConvert.DeserializeObject<Root>(line);

            yield return root;
        }
    }
    
    private int GetCurrentFrameCount()
    {
        return Time.frameCount;
    }
    
    private void UpdateGame(int currentFrameCount)
    {
        Root currentFrameData = getRoots($"{Application.persistentDataPath}/Applicant-test.idf")
            .FirstOrDefault(x => x.FrameCount == currentFrameCount);
        
        if (currentFrameData != null)
        {
            personObject.UpdatePersonPositions(currentFrameData.Persons,currentFrameData.Ball);
        }
    }
    
    private void Update()
    {
        if (frameCounts != null && frameCounts.Count > 0)
        {
            currentFrameCount = GetCurrentFrameCount() + 1519819;
            if (frameCounts.Contains(currentFrameCount))
            {
                UpdateGame(currentFrameCount);
            }
        }
    }
}

[System.Serializable]
public class Ball
{
    public List<double> Position { get; set; }
}

[System.Serializable]
public class Root
{
    [JsonProperty("FrameCount")]
    public int FrameCount { get; set; }
    [JsonProperty("Persons")]
    public List<Person> Persons { get; set; }
    [JsonProperty("Ball")]
    public Ball Ball { get; set; }
}

[System.Serializable]
public class Person
{
    public int Id { get; set; }
    public List<double> Position { get; set; }
    public int TeamSide { get; set; }
}
