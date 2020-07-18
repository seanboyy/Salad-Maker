using System.Collections.Generic;
using UnityEngine;

class RandomUtil
{
    static System.Random random = new System.Random();

    public static List<Vegetable> CreateRandomCombination()
    {
        var randomIngredientCount = random.Next(1, 4);
        var veggieCombo = new List<Vegetable>();
        var veggieCopy = new List<Vegetable>(GameConstants.Vegetables);
        Shuffle(veggieCopy);
        for (int i = 0; i < randomIngredientCount; ++i)
        {
            veggieCombo.Add(veggieCopy[i]);
        }
        return veggieCombo;
    }

    public static float GetRandomWaitTime()
    {
        float scale = (float)random.NextDouble();
        float waitTime = GameConstants.CustomerAppearWaitTime * scale;
        waitTime = Mathf.Clamp(waitTime, 0.5F, GameConstants.CustomerAppearWaitTime);
        return waitTime;
    }

    //Adapted from
    //https://www.csharp-console-examples.com/loop/c-shuffle-list/
    private static void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
    