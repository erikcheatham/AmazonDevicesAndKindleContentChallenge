using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonTest
{
    class Program
    {
        public static string ERROR = "";
        public static string STDIN = @"0.15
                            4
                            Item1 Item2 0.2
                            Item2 Item3 0.1
                            Item4 Item5 0.3
                            Item5 Item6 0.4";
        public static string STDOUTCheck = @"Item4";
        public static string STDOUT = "";

        public static string[] lineInput { get; set; }

        public static double aThreshold { get; set; }
        public static int aRelationshipCount { get; set; }

        public static List<string> itemsForffinityThresholdChecking { get; set; }
        public static List<string[]> itemsPassedAffinityThreshold { get; set; }
        public static string[] clusterItemization { get; set; }

        public static double affinityProbability { get; set; }

        public static List<string[]> clusterChecking { get; set; }
        
        public static Dictionary<string,double> itemAffinities { get; set; }

        static void Main(string[] args)
        {
            //Each index contains each line from STDIN
            lineInput = STDIN.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            //First line is affintity threshold
            aThreshold = aThreshold.GetAffinityThreshold(lineInput, 1, ERROR);

            //Second line is affinity count of relationships
            aRelationshipCount = aRelationshipCount.GetAffinityRelationshipCount(lineInput, 2, ERROR);

            itemsForffinityThresholdChecking = new List<string>();

            //Start on third index for cluster comparison
            for (int i = 2; i < aRelationshipCount + 2; i++)
            {
                var line = lineInput[i].Replace("\r","").Trim().ToString();
                itemsForffinityThresholdChecking.Add(line);
            }

            itemsPassedAffinityThreshold = new List<string[]>();

            //Get only useful items to check
            foreach (string line in itemsForffinityThresholdChecking)
            {
                clusterItemization = line.Split(' ');
                affinityProbability = affinityProbability.GetAffinityProbability(clusterItemization, 3, ERROR);
                if (affinityProbability > aThreshold)
                {
                    itemsPassedAffinityThreshold.Add(line.Split(' '));
                }
            }

            clusterChecking = new List<string[]>();
            clusterChecking = itemsPassedAffinityThreshold;

            itemAffinities = new Dictionary<string, double>();

            //Run clusterChecking
            foreach (string[] line in itemsPassedAffinityThreshold)
            {
                string item1 = line[0];
                string item2 = line[1];

                foreach (string[] lineChecking in clusterChecking)
                {
                    if (lineChecking[0] == lineChecking[1])
                    {
                        itemAffinities.Add(lineChecking[0], double.Parse(lineChecking[2]));
                    }

                    if (lineChecking.Contains(item1))
                    {
                        itemAffinities.Add(lineChecking[0], double.Parse(lineChecking[2]));
                    }

                    if (lineChecking.Contains(item2))
                    {
                        itemAffinities.Add(lineChecking[1], double.Parse(lineChecking[2]));
                    }
                }
            }

            STDOUT = itemAffinities.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            if (!string.IsNullOrEmpty(ERROR))
            {
                STDOUT = ERROR;
            }
            Console.WriteLine();
            Console.WriteLine(STDOUT);
        }
    }

    public static class ExtensionMethods
    {
        public static double GetAffinityThreshold(this double aThreshold, string[] input, int itemPlusOne, string ERROR)
        {
            if (double.TryParse(input[itemPlusOne - 1], out aThreshold))
            {
                return aThreshold;
            }
            else
            {
                ERROR = "AFFINITY THRESHOLD CONVERSION ISSUE!";
                return 0.0;
            }
        }

        public static int GetAffinityRelationshipCount(this int aRelationshipCount, string[] input, int itemPlusOne, string ERROR)
        {
            if (int.TryParse(input[itemPlusOne - 1], out aRelationshipCount))
            {
                return aRelationshipCount;
            }
            else
            {
                ERROR = "AFFINITY RELATIONSHIP COUNT CONVERSION ISSUE!";
                return 0;
            }
        }

        public static double GetAffinityProbability(this double affinityProbability, string[] input, int itemPlusOne, string ERROR)
        {
            if (double.TryParse(input[itemPlusOne - 1], out affinityProbability))
            {
                return affinityProbability;
            }
            else
            {
                ERROR = "AFFINITY PROBABILITY CONVERSION ISSUE!";
                return 0;
            }
        }
    }
}
