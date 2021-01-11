using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public enum Sex
{
    Male,
    Female
}

public class NameSet
{
    public string given;
    public string surname;
    public NameSet(string given, string surname)
    {
        this.given = given;
        this.surname = surname;
    }
    public override string ToString()
    {
        return given + " " + surname;
    }
}

public static class RandomName
{

    private static string filePath = "Assets/JSON/names.json";

    [Serializable]
    class NameList
    {
        public string[] boys;
        public string[] girls;
        public string[] last;

        public NameList()
        {
            boys = new string[] { };
            girls = new string[] { };
            last = new string[] { };
        }
    }

	static NameList nameList = null;

    public static NameSet Generate()
    {
        Sex sex = (Sex)UnityEngine.Random.Range(0, 2);
        return Generate(sex);
    }

    public static NameSet Generate(Sex sex)
    {
        if (nameList == null)
        {
            CreateNameList();
        }

        string given;
        if (sex == Sex.Male)
        {
            given = nameList.boys[UnityEngine.Random.Range(0, nameList.boys.Length)];
        }
        else
        {
            given = nameList.girls[UnityEngine.Random.Range(0, nameList.girls.Length)];
        }
        string surname = nameList.last[UnityEngine.Random.Range(0, nameList.last.Length)];
        return new NameSet(given, surname);
    }

    private static void CreateNameList()
	{
        StreamReader reader = new StreamReader(filePath);
        string jsonString = reader.ReadToEnd();

        nameList = JsonUtility.FromJson<NameList>(jsonString);
    }
}