using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

public class DataLoader : MonoBehaviour
{
    private List<int> frameCounts;
    private string filePath;
    public List<Root> frames;
    public List<int> personIds = new List<int>();
    public IEnumerable<Root> roots;
    public Spawner spawner = new Spawner();
    public List<Person> persons = new List<Person>();
    [FormerlySerializedAs("personObject")] public Movement movement = new Movement();
    private int frameCountStart = 1519819;

    private void Start()
    {
        ReadJson();
    }

    // Reading out the json file and putting everything that  i need in variables
    //after that im looping through each frame after which i loop through all
    //the persons to assign the id values to a list and the objects them self to a list as well
    // i tried just skipping the personIds but for some reason unity keeps crashing which i don't understand as it is kind of redundant
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
                    persons.Add(person);
                }
            }
        }
        // here i'm spawning the players and setting their color 
        spawner.SpawnPersons();
        movement.SetColor(persons);
    }
    
    // here i'm pretty much doing the usual json file stuff basicly a function to get the frames out of the json and putting them in a variable
    // i was using system.utilities before but switched to newtonsoft as i read everywhere on stackoverflow that it works a lot better.
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
    // getting the current frametime to add the start of the json framecount to since it starts at 1519819 instead of 0
    private int GetCurrentFrameCount()
    {
        return Time.frameCount+ frameCountStart;
    }
    
    // here i update the game based on the framecount, however this is highly unoptimal as i'm retrieving the entire json for each player every frame.
    // most of my work has gone into trying to find a way around it by initialising  the currentframa data in the start function and looping over it here however i was unable to get it to work.
    // i've also tried saving all the positions and looping through those but that also didn't work.
    private void UpdateGame(int currentFrameCount)
    {
        Root currentFrameData = getRoots($"{Application.persistentDataPath}/Applicant-test.idf")
            .FirstOrDefault(x => x.FrameCount == currentFrameCount);
        
        if (currentFrameData != null)
        {
            movement.UpdatePersonPositions(currentFrameData.Persons,currentFrameData.Ball);
        }
    }
    // setting up the altered update to use the frames of the json
    private void Update()
    {
        if (frameCounts != null && frameCounts.Count > 0)
        {
            if (frameCounts.Contains(GetCurrentFrameCount()))
            {
                UpdateGame(GetCurrentFrameCount());
            }
        }
    }
}

// data holders
public class Person
{
    public int Id { get; set; }
    public List<double> Position { get; set; }
    public int TeamSide { get; set; }
}

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

