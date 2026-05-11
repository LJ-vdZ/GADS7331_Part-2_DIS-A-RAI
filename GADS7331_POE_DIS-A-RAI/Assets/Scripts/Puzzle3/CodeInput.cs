using UnityEngine;

public class CodeInput : MonoBehaviour
{
    private string currentInput = "";
    private ZeroGravityZone currentZone;

    private void Update()
    {
        if (currentZone == null) return;

        foreach (char c in Input.inputString)
        {
            if (char.IsDigit(c))
            {
                currentInput += c;
                Debug.Log("Code input: " + currentInput);

                if (currentInput.Length >= currentZone.GetCode().Length)
                {
                    if (currentInput == currentZone.GetCode())
                    {
                        Debug.Log("Correct Code! Gravity Restored.");
                        currentZone.RestoreGravity();
                        currentZone = null;
                        currentInput = "";
                    }
                    else
                    {
                        Debug.Log("Wrong Code");
                        currentInput = "";
                    }
                }
            }
        }
    }

    public void StartCodeInput(ZeroGravityZone zone)
    {
        currentZone = zone;
        currentInput = "";
        Debug.Log("Enter the code to restore gravity...");
    }
}
