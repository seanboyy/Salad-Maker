using System.Collections.Generic;
using UnityEngine;

class GameConstants
{
    #region Tags
    public const string TrashTag = "TagTrashCan";
    public const string DispenserTag = "TagVegetableDispenser";
    public const string CuttingBoardTag = "TagCuttingBoard";
    public const string CustomerTag = "TagCustomer";
    public const string PlateTag = "TagPlate";
    #endregion

    #region Numbers
    public const float ChopTime = 2.0F;
    public const float WaitTime = 20.0F;
    public const float RecipeComplexityScale = 1.5F;
    public const float CustomerAppearWaitTime = 6F;
    public const float AngrySpeedupFactor = 1.5F;
    public const float SpeedUpScalar = 1.3F;
    public const int TimeUpRate = 30;
    public const int ScorePerIngredient = 10;
    public const int PointsUpRate = 20;
    #endregion

    #region Strings
    public static List<Vegetable> Vegetables = new List<Vegetable>
    {
        new Vegetable("Lettuce"),
        new Vegetable("Tomato"),
        new Vegetable("Cucumber"),
        new Vegetable("Pepper"),
        new Vegetable("Onion"),
        new Vegetable("Carrot")
    };
    #endregion

    #region Misc
    public static Color Player1Color = new Color(1F, 0F, 0F);
    public static Color Player2Color = new Color(0F, 0F, 1F);
    #endregion
}