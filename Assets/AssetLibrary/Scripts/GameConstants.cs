using System.Collections.Generic;
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
    public const float WaitTime = 13.0F;
    public const float RecipeComplexityScale = 1.5F;
    public const float CustomerAppearWaitTime = 6F;
    public const float AngrySpeedupFactor = 1.5F;
    #endregion

    #region Strings
    public static List<Vegetable> Vegetables = new List<Vegetable>{
        new Vegetable("Lettuce"),
        new Vegetable("Tomato"),
        new Vegetable("Cucumber"),
        new Vegetable("Pepper"),
        new Vegetable("Onion"),
        new Vegetable("Carrot")
    };
    #endregion
}

