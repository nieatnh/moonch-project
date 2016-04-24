using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public static class stringHelper
{
    public static string addEndLines(this string text)
    {
        string res = "";
        for (int i = 0; i < text.Length; i++)
        {
            res += text[i];
            if (i > 0 && i % 52 == 0)
            {
                res += '\n';
            }

        }
        return res;
    }
}

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
    Fact storyRoboticSampleReturn = new Fact("Story", "Robotic Sample return", "While in the 1960s the USA concentrated its efforts on getting humans to the Moon, the Soviet Union\n developed a range of automatic lunar explorers.\nThe Soviet robotic programme launched three successful sample return missions(Luna 16, 20 and 24) between 1970\n and 1976.These missions collected, and returned\n to Earth, about 300 grams of lunar soil \nfrom three sites in a fully automated way.", "sampleReturn");
    Fact howFar = new Fact("Moon Facts", "How far is the moon?", "Min. 363,000, max 405,000 km", "how_far");
    Fact aplo11 = new Fact("Story", "Apollo 11 first land on the moon",  "Launch date: 16 July 1969. Return date: 24 July 1969.\nApollo 11 was the first manned mission to land on the Moon.The objective was to land a man on the Moon and return safely to Earth.\nAfter a four-day trip from Earth, the first crewed flight to the lunar surface touched down and said 'Houston, Tranquility Base here.The Eagle has landed.'".addEndLines(), "apolo11");
    Fact weighOnMoon = new Fact("Moon Facts", "How much would you weigh in the moon?", "1/6th of what you weigh on Earth", "weigthMoon");
    Fact internationalSpaceStation = new Fact("Human exploration", "People living on the moon?", "To explore the Moon and Mars in the future, we will need modules for astronauts to live and work. We will need life-support systems to clean and maintain the air and prevent dust from the surface of the Moon or Mars contaminating the habitat.".addEndLines(), "moonStation");
    Fact luna9 = new Fact("Spacecrafts", "Luna 9", "The Soviet Luna 9 spacecraft was the first to achieve a soft landing on the Moon and to transmit photographs from lunar surface back to Earth.".addEndLines(), "luna9");
    Fact howLongDay = new Fact("Moon Facts", "How long is a day in the moon?", "A lunar day is 27.3 Earth days long", "");
    Fact temperature = new Fact("Moon Facts", "Whats the temperature on the moon surface?", "Between -233°C and +123 °C", "");
    Fact howManyPeopleWalkOn = new Fact("Moon Facts", "How many people walk on the moon?", "12 astronauts during the Apollo programme (1969 – 1972)".addEndLines(), "numberPeople");
    Fact howLongDoesItTake = new Fact("Moon Facts", "How long does it take to go to the moon?", "About 3 days", "longTake");
    Fact whyLooksBigger = new Fact("Moon Facts", "Why does the moon look bigger at times?", "Due to its elliptical orbit", "hugeMoon");
    Fact waterOnMoon = new Fact("Moon Facts", "Is there water on the moon?", "Yes, water ice at the poles", "waterMoon");
    Fact cratersOn = new Fact("Moon Facts", "Why do you see so many craters on the moon?", "Craters keep their shape since there is no weather and tectonic activity".addEndLines(), "craters");


    // Use this for initialization
    void Start()
    {
        moonFactsCollection.Add(howBig);
        moonFactsCollection.Add(storyRoboticSampleReturn);
        moonFactsCollection.Add(howFar);
        moonFactsCollection.Add(aplo11);
        moonFactsCollection.Add(weighOnMoon);
        moonFactsCollection.Add(internationalSpaceStation);
        moonFactsCollection.Add(howLongDay);
        moonFactsCollection.Add(luna9);
        moonFactsCollection.Add(temperature);
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
