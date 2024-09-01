using Editor.CardEditor;
using UnityEditor;
using UnityEngine;

namespace Editor.CostCalculator
{
    public class CostCalculatorWindow : EditorWindow
    {
        public CostCalculatorSettings calculationSettings;
        
        // Constants
        private const string Window_Title = "Card Cost Calculator";
        private const bool Is_Utility_Window = false;
        private const string submitButtonText = "Calculate";
        private const string loadButtonText = "Load";
        private const float Label_Height = 30;
        private const float Rect_Width = 300;

        // Class Variables
        private CardSO _loadedCard = null;
        private string _message = "";
        private string _keywordSumString;

        // GUI Variables
        private Rect _cardInfoRect;
        private Rect _calculationRect;
        private Rect _messageRect;
        private Rect _buttonRect;
    
        [MenuItem("Tools/Utilities/Cost Calculator")]
        public static void ShowWindow()
        {
            CostCalculatorWindow window =
                (CostCalculatorWindow)GetWindow(typeof(CostCalculatorWindow), Is_Utility_Window, Window_Title);
            // Set window position
            window.position = new Rect(50, 50, 300, 500);
            window.Show();
        }

        private void OnGUI()
        {
            SetupAreaRects();
            DrawCardObjectField();
            DrawCardInfoArea();
            //DrawCalculationArea();
            DrawMessageBox();
            DrawControlButtons();
        }

        private void SetupAreaRects()
        {
            // Initialize Card Info rect
            _cardInfoRect = new Rect(position.width * 0.10f, 50,position.width * 0.5f, position.height * 0.5f );
            // Initialize Calc area rect
            //_calculationRect = new Rect(position.width * 0.10f,_cardInfoRect.y + _cardInfoRect.height + 10,position.width * 0.5f,position.height * 0.1f);
            // Initialize Message area rect
            _messageRect = new Rect(position.width * 0.10f, _cardInfoRect.y + _cardInfoRect.height + 10,position.width * 0.5f,position.height * 0.10f);
            // Initialize Button area rect
            _buttonRect = new Rect(position.width * 0.10f, _messageRect.y + _messageRect.height + 5, position.width * 0.5f, position.height * 0.20f);
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
            EditorGUILayout.BeginHorizontal();
            GUI.Label(new Rect(_cardInfoRect.x, _cardInfoRect.y - 40, position.width * 0.333f, 20), "Loaded Card:");
            // Object Field for loading a card
            _loadedCard = EditorGUI.ObjectField(new Rect(_cardInfoRect.x + 110, _cardInfoRect.y - 40, position.width * 0.333f, 20), _loadedCard,
                typeof(CardSO), false) as CardSO;
            EditorGUILayout.EndHorizontal();
        }
        private void DrawCardInfoArea()
        {
            GUILayout.BeginArea(_cardInfoRect);
            // Show values from the loaded card
            DrawLabel($"Card Name: {_loadedCard?.CardName}");
            DrawLabel($"Card Type: {_loadedCard?.CardType}");
            DrawLabel($"Attack: {_loadedCard?.Attack.StatValue}");
            DrawLabel($"Explore: {_loadedCard?.Explore.StatValue}");
            DrawLabel($"Focus: {_loadedCard?.Focus.StatValue}");
            DrawLabel($"Hit Points: {_loadedCard?.HitPoints.StatValue}");
            DrawLabel($"Speed: {_loadedCard?.Speed.StatValue}");
            DrawLabel($"Upgrade Slots: {_loadedCard?.UpgradeSlots.StatValue}");
            DrawLabel($"Keywords: {_keywordSumString}");
            GUILayout.EndArea();
        }

        private void DrawCalculationArea()
        {
            // 600w * 200h
            // Display Card Weight Values for the loaded card.
            GUILayout.BeginArea(_calculationRect);
            //GUILayout.Space(50);
            DrawLabel($"Calculation: ");
            GUILayout.EndArea();
        }

        private void DrawMessageBox()
        {
            GUILayout.BeginArea(_messageRect);
            EditorGUILayout.LabelField(_message,  EditorStyles.whiteLargeLabel, GUILayout.Height(_messageRect.height));
            GUILayout.EndArea();
        }

        private void DrawControlButtons()
        {
            GUILayout.BeginArea(_buttonRect);
            GUILayout.BeginHorizontal(GUILayout.Width(position.width * 0.33f),GUILayout.ExpandHeight(true));
            // Calculate Cost Button
            // 200w * 60h
            if (GUILayout.Button(submitButtonText, GUILayout.Width(position.width * 0.33f), GUILayout.Height(_buttonRect.height * 0.25f)))
            {
                RunCalculation();
            }
            /*GUILayout.Space(50);
            if (GUILayout.Button(loadButtonText, GUILayout.Width(150)))
            {
                // Load
            }*/
        
            // 200w space
        
            // Load button
            // 200w * 60h
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        
        private void DrawLabel(string text) 
        {
            EditorGUILayout.LabelField(text, EditorStyles.whiteLargeLabel, GUILayout.Height(Label_Height));
        }

        private void RunCalculation()
        {
            // Call method on Settings to run calculation.
            // Display Message "Cost: n"
            float cost = 0.0f;
            _message = $"Calculating cost for {_loadedCard?.CardName}";
            if (!ReferenceEquals(_loadedCard, null))
            {
                CostCalculator calculator = new CostCalculator(_loadedCard.WeightData,_loadedCard.GetCardStats(), _loadedCard.Keywords);
                ;
               cost = calculator.NormalizeCost();
            }

            _keywordSumString = _loadedCard?.GetKeywordsSumString();
            _message = $"Cost is {cost}";
        }
    }
}


