using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

class RandomUtil
{
    static readonly System.Random random = new System.Random();

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
        waitTime = Mathf.Clamp(waitTime, 1F, GameConstants.CustomerAppearWaitTime);
        return waitTime;
    }

    //Adapted from
    //https://www.csharp-console-examples.com/loop/c-shuffle-list/
    public static void Shuffle<T>(IList<T> list)
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

    public static float GenerateFloat()
    {
        return (float)random.NextDouble();
    }

    public static Vector3 GenerateNonOccupiedPosition()
    {
        float extent = Camera.main.orthographicSize;
        float aspect = Camera.main.aspect;
        float halfHeight = extent;
        float halfWidth = aspect * extent;
        float minX = -halfWidth;
        float maxX = halfWidth;
        float minY = -halfHeight;
        float maxY = halfHeight;
        minX += 1.5F;
        maxX -= 1.5F;
        minY += 1.5F;
        maxY -= 2.5F;
        var players = Object.FindObjectsOfType<PlayerBehaviour>();
        float x = (float)random.NextDouble();
        x *= maxX * 2;
        x += minX;
        float y = (float)random.NextDouble();
        y *= maxY * 2;
        y += minY;
        bool collision1 = true, collision2 = true;
        while (collision1 || collision2)
        {
            foreach (var player in players)
            {
                var playerPos = player.transform.position;
                var playerMinX = playerPos.x - 1F;
                var playerMaxX = playerPos.x + 1F;
                var playerMinY = playerPos.y - 1F;
                var playerMaxY = playerPos.y + 1F;
                switch (player.playerNumber)
                {
                    case 1:
                        if (collision1)
                        {
                            if (x >= playerMinX && x <= playerMaxX)
                            {
                                x = TryResolveCollision(minX, maxX);
                            }
                            else if (y >= playerMinY && y <= playerMinY)
                            {
                                y = TryResolveCollision(minY, maxY);
                            }
                            else
                            {
                                collision1 = false;
                            }
                        }
                        break;
                    case 2:
                        if (collision2)
                        {
                            if (x >= playerMinX && x <= playerMaxX)
                            {
                                x = TryResolveCollision(minX, maxX);
                            }
                            else if (y >= playerMinY && y <= playerMinY)
                            {
                                y = TryResolveCollision(minY, maxY);
                            }
                            else
                            {
                                collision2 = false;
                            }
                        }
                        break;
                }
            }
        }
        return new Vector3(x, y, -2);
    }

    private static float TryResolveCollision(float min, float max)
    {
        float output = (float)random.NextDouble();
        output *= max * 2;
        output += min;
        return output;
    }
}
    