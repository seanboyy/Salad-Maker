using System.Collections.Generic;
using System.Text;

public class Salad : ScoreObject
{
    private readonly List<Vegetable> ingredients;

    public Salad() : base("")
    {
        ingredients = new List<Vegetable>();
    }

    public Salad(Salad other) : base("")
    {
        ingredients = new List<Vegetable>();
        foreach (var ingredient in other.ingredients)
        {
            AddIngredient(ingredient);
        }
    }

    public void AddIngredient(Vegetable newIngredient)
    {
        ingredients.Add(newIngredient);
        DoUpdateName();
    }

    public bool CompareTo(Salad comparable)
    {
        bool didNotFindMatch = false;
        foreach (var vegetable in ingredients)
        {
            if (!comparable.ingredients.Contains(vegetable)) return false;
        }
        return !didNotFindMatch;
    }

    private void DoUpdateName()
    {
        StringBuilder stringBuilder = new StringBuilder("Chopped ");
        foreach (var vegetable in ingredients)
        {
            stringBuilder.Append(vegetable.name + ", ");
        }
        string newName = stringBuilder.ToString();
        name = newName.Substring(0, newName.Length - 2);
    }

    public int GetIngredientCount()
    {
        return ingredients.Count;
    }
}