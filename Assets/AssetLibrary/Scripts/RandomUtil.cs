using System;
using System.Collections.Generic;
using System.Linq;

class RandomUtil
{
    static Random random = new Random();

    public static List<string> CreateRandomCombination()
    {
        var randomIngredientCount = random.Next(1, 4);
        var veggieCombo = new List<string>();
        var veggieCopy = new List<string>(GameConstants.Vegetables);
        var rand = new Random();
        Shuffle(veggieCopy);
        for (int i = 0; i < randomIngredientCount; ++i)
        {
            veggieCombo.Add(veggieCopy[i]);
        }
        return veggieCombo;
    }

    private static void Shuffle<T>(IList<T> list)
    {
        Random rng = new Random();
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
    