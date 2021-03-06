﻿// Json Mapping Automatically Generated By JsonToolkit Library for C#
// Diego Trinciarelli 2011
// To use this code you will need to reference Newtonsoft's Json Parser, downloadable from codeplex.
// http://json.codeplex.com/
// 
using System;
using Newtonsoft.Json;

namespace App.Web1.Code
{
    [Serializable]
    public class RandomUserResults
    {

        public Result[] Results;

        //Empty Constructor
        public RandomUserResults() { }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static RandomUserResults FromJson(string json)
        {
            return JsonConvert.DeserializeObject<RandomUserResults>(json);
        }
    }


    [Serializable]
    public class Name
    {

        public string Title;
        public string First;
        public string Last;

        //Empty Constructor
        public Name() { }

    }


    [Serializable]
    public class Location
    {

        public string Street;
        public string City;
        public string State;
        public string Zip;

        //Empty Constructor
        public Location() { }

    }


    [Serializable]
    public class User
    {

        public string Gender;
        public Name Name;
        public Location Location;
        public string Email;
        public string Username;
        public string Password;
        public string Salt;
        public string Md5;
        public string Sha1;
        public string Sha256;
        public string Registered;
        public string Dob;
        public string Phone;
        public string Cell;
        public string SSN;
        public string Picture;

        //Empty Constructor
        public User() { }

    }


    [Serializable]
    public class Result
    {
        [JsonProperty(PropertyName = "user")]
        public User User;
        public string Seed;
        public string Version;

        //Empty Constructor
        public Result() { }

    }

}