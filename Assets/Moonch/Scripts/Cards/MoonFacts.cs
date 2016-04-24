using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoonFacts : MonoBehaviour
{

    public int selectedIndex;
    List<Fact> moonFactsCollection = new List<Fact>();

    class Fact
    {
        public string title;
        public string question;
        public string answer;
        public string imagePath;

        public Fact(string title, string question, string answer, string imagePath)
        {
            this.title = title;
            this.question = question;
            this.answer = answer;
            this.imagePath = imagePath;
        }
    }
    Fact howBig = new Fact("Moon Facts", "How big is the moon?", "Moon diameter = 3,475 km \n Earth diameter = 12,742 km", "earthSize");
    Fact howFar = new Fact("Moon Facts", "How far is the moon?", "Min. 363,000, max 405,000 km", "earthSize");
    Fact howLongDay = new Fact("Moon Facts", "How long is a day in the moon?", "A lunar day is 27.3 Earth days long", "");
    Fact temperature = new Fact("Moon Facts", "Whats the temperature on the moon surface?", "Between -233°C and +123 °C", "");
    Fact weighOnMoon = new Fact("Moon Facts", "How much would you weigh in the moon?", "1/6th of what you weigh on Earth", "");
    Fact howManyPeopleWalkOn = new Fact("Moon Facts", "How many people walk on the moon?", "12 astronauts during the Apollo programme (1969 – 1972)", "");
    Fact howLongDoesItTake = new Fact("Moon Facts", "How long does it take to go to the moon?", "About 3 days", "");
    Fact whyLooksBigger = new Fact("Moon Facts", "Why does the moon look bigger at times?", "Due to its elliptical orbit", "");
    Fact waterOnMoon = new Fact("Moon Facts", "Is there water on the moon?", "Yes, water ice at the poles", "");
    Fact cratersOn = new Fact("Moon Facts", "Why do you see so many craters on the moon?", "Craters keep their shape since there is no weather and tectonic activity", "");


    // Use this for initialization
    void Start()
    {
        moonFactsCollection.Add(howBig);
        moonFactsCollection.Add(howFar);
        moonFactsCollection.Add(howLongDay);
        moonFactsCollection.Add(temperature);
        moonFactsCollection.Add(weighOnMoon);
        moonFactsCollection.Add(howManyPeopleWalkOn);
        moonFactsCollection.Add(howLongDoesItTake);
        moonFactsCollection.Add(whyLooksBigger);
        moonFactsCollection.Add(waterOnMoon);
        moonFactsCollection.Add(cratersOn);

    }

    // Update is called once per frame
    void Update()
    {
        if (selectedIndex >= 0 && selectedIndex < moonFactsCollection.Count)
        {
            Fact fact = moonFactsCollection[selectedIndex];

            Text titleText = GameObject.FindGameObjectWithTag("CardTitle").GetComponent<Text>();
            titleText.text = fact.title;

            Text questionText = GameObject.FindGameObjectWithTag("CardQuestion").GetComponent<Text>();
            questionText.text = fact.question;

            Text answerText = GameObject.FindGameObjectWithTag("CardAnswer").GetComponent<Text>();
            answerText.text = fact.answer;

            RawImage image =  GameObject.FindGameObjectWithTag("CardImage").GetComponent<RawImage>();
            image.texture = Resources.Load<Texture2D>(fact.imagePath);
        }
    }
}
