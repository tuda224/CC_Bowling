﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CC_Bowling
{
    public static class Level4
    {
        public class Round
        {
            public int First { get; set; }
            public int? Second { get; set; } = null;

            public int Points { get; set; }
        }

        public static void Run()
        {
            var input = ReadInput();
            var splittedInput = input.Split(':');
            var points = splittedInput[1].Split(',');

            var roundsPoints = new SortedList<int, Round>();
            var roundCounterForLoop = 1;
            for (int i = 0; i < points.Length; i++)
            {
                var currentRound = new Round();
                currentRound.First = Int32.Parse(points[i]);
                if (currentRound.First != 10)
                {
                    try
                    {
                        // could be empty in last round
                        if (Int32.TryParse(points[i + 1], out int secondCount))
                            currentRound.Second = secondCount;
                    }
                    catch
                    {
                        // nothing to do
                    }

                    i++; // for second round
                }

                roundsPoints.Add(roundCounterForLoop, currentRound);
                roundCounterForLoop++;
            }

            CalculatePoints(roundsPoints);
            PrintPoints(roundsPoints, Int32.Parse(splittedInput[0]));
        }

        private static string ReadInput()
        {
            Console.WriteLine("Provide path to input file:");
            var filePath = Console.ReadLine();
            var input = File.ReadAllText(filePath);
            return input;
        }

        private static void CalculatePoints(SortedList<int, Round> rounds)
        {
            for (int i = 1; i <= rounds.Count; i++)
            {
                // handle strike -> 10 in first round
                if (rounds[i].First == 10)
                {
                    rounds[i].Points = 10 + rounds[i + 1].First +
                        (rounds[i + 1].Second != null ? rounds[i + 1].Second.Value : rounds[i + 2].First);
                    continue;
                }
                // handle spare -> 10 in both rounds
                if (rounds[i].First + rounds[i].Second == 10)
                {
                    rounds[i].Points = 10 + rounds[i + 1].First;
                    continue;
                }
                // normal points
                rounds[i].Points = rounds[i].First + rounds[i].Second.Value;
            }
        }

        private static void PrintPoints(SortedList<int, Round> rounds, int numberOfRounds)
        {
            var summedUp = 0;
            var resultText = string.Empty;
            for (int i = 1; i <= numberOfRounds; i++)
            {
                summedUp += rounds[i].Points;
                resultText += $"{summedUp},";
            }
            Console.WriteLine(resultText.Substring(0, resultText.Length - 1));
        }
    }
}