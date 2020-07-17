using System.Collections.Generic;

public class Salad
{
    private List<Vegetable> ingredients;

    public Salad()
    {
        ingredients = new List<Vegetable>();
    }

    public void AddIngredient(Vegetable newIngredient)
    {
        ingredients.Add(newIngredient);
    }

    public bool CompareTo(Salad comparable)
    {
        foreach (var vegetable in ingredients)
        {
            if (!comparable.ingredients.Contains(vegetable)) return false;
        }
        return true;
    }
}