using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public const float WaitTime = 15.0F;
    public const float RecipeComplexityScale = 1.5F;
    #endregion

    #region Strings
    public static List<string> Vegetables = new List<string>{ "Lettuce", "Tomato", "Cucumber", "Pepper", "Onion", "Carrot" };
    #endregion
}

