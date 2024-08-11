using UnityEditor;

namespace Editor.CostCalculator
{
    public class CostCalculatorWindow : EditorWindow
    {
        // Constants
    
        // Class Variables
    
        // GUI Variables
    
        [MenuItem("Tools/Utilities/Cost Calculator")]
        public static void ShowWindow()
        {
            CostCalculatorWindow window = (CostCalculatorWindow)EditorWindow.GetWindow(typeof(CostCalculatorWindow));
            window.Show();
        }

        private void OnGUI()
        {
            // Draw Main Area
            // Draw  Card Object Field
            // Draw Card Info Area
            // Draw Calculation Area
            // Draw Control Buttons
        
        }

        /// <summary>
        /// Draws the main area of the CostCalculatorWindow.
        /// </summary>
        /// <remarks>
        /// This method is responsible for drawing the main area of the CostCalculatorWindow.
        /// It draws a rectangle with dimensions of 700 width and 700 height.
        /// </remarks>
        private void DrawMainArea()
        {
            // Draw Main Area Rect 
            // 700w * 700h
        }

        /// <summary>
        /// Draws the card object field in the CostCalculatorWindow.
        /// </summary>
        /// <remarks>
        /// This method is responsible for drawing the card object field in the CostCalculatorWindow.
        /// It allows the user to select a card object from the Unity editor.
        /// </remarks>
        /// <param name="/*START_USER_CODE*/parameterName/*END_USER_CODE*/">/*START_USER_CODE*/The parameter description/*END_USER_CODE*/</param>
        private void DrawCardObjectField()
        {
            // Object Field for loading a card
        }
        private void DrawCardInfoArea()
        {
            // 600w * 250h
            // Show values from the loaded card
        }

        private void DrawCalculationArea()
        {
            // 600w * 200h
            // Display Card Weight Values for the loaded card.
        }

        private void DrawMessageBox()
        {
            // Label for messages
        }

        private void DrawControlButtons()
        {
            // Calculate Cost Button
            // 200w * 60h
        
            // 200w space
        
            // Load button
            // 200w * 60h
        }
    }
}


