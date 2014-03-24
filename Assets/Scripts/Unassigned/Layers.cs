using UnityEngine;
using System.Collections;

public class Layers : MonoBehaviour {
    public static int GroceriesMask = (1 << LayerMask.NameToLayer("Groceries"));
    public static int CashRegisterMask = (1 << LayerMask.NameToLayer("CashRegister"));
    public static int ExitTriggerMask = (1 << LayerMask.NameToLayer("ExitTrigger"));
}
