using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

public class Dice
{
    private List<(int, int, int)> listOfDice;

    public List<(int, int, int)> ListOfDice { get { return listOfDice; } private set { listOfDice = value; } }

    public Dice(string diceString)
    {
        listOfDice = new List<(int, int, int)> ();
        ParseDiceString(diceString);
    }

    private void ParseDiceString(string diceString)
    {
        //Match = entire substring that matches regex, contains Groups stored in GroupCollection
        //Group = captured sections of matched substring- number determined by going left to right
        //First group of GroupCollection is the entire match, 1 and on are the various capture groups
        //In this case, outer () captures the entire dice string, Group 0 and 1 are the same string
        Regex diceCaptureRegex = new Regex(@"(((\d+)d(\d+))((\+|-)(\d+))?)+", RegexOptions.None);
        MatchCollection diceMatches = diceCaptureRegex.Matches(diceString);

        if (diceMatches.Count == 0)
        {
            Debug.LogError("This Dice String was not formatted properly!");
        }

        //For each dice string
        foreach (Match match in diceMatches.Cast<Match>())
        {
            //Debug.Log("Match: "+match.ToString());
            GroupCollection matchGroups = match.Groups;

            //Group 3 = number of rolls, just obtain Group string and parse it
            //Group 4 = number of sides, Group 5 = +/-number (modifier)
            int.TryParse(matchGroups[3].Value, out int rollsResult);
            int numberOfRolls = rollsResult;
            int.TryParse(matchGroups[4].Value, out int sidesResult);
            int numberOfSides = sidesResult;
            int.TryParse(matchGroups[5].Value, out int modifierResult);
            int modifier = modifierResult;
            //Debug.Log("Number of Rolls: " + numberOfRolls + " Number of Sides: " + numberOfSides + " Modifier: " + modifier);
            listOfDice.Add((numberOfRolls, numberOfSides, modifierResult));
        }
    }

    public int RollDice()
    {
        int total = 0;
        foreach((int, int, int) die in  listOfDice)
        {
            total += RollDie(die.Item1, die.Item2, die.Item3);
        }

        return total;
    }

    public static int RollDie(int rolls, int sides, int modifier)
    {
        int total = 0;

        for(int i = 0; i < rolls; i++)
        {
            //Range = [min, max), so max + 1 to include max
            total += Random.Range(1, sides + 1);
        }

        return total + modifier;
    }
}
