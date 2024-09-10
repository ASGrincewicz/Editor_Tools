using Editor.CardData;
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
        private const string CalculateButtonText = "Calculate";
        private const string EditButtonText = "Edit Card";
        private const float Label_Height = 30;
        private const float Rect_Width = 300;

        // Class Variables
        private CardSO LoadedCardData { get; set; } = null;

        private string Message { get; set; } = "";
        private string KeywordSumString { get; set; }

        // GUI Variables
        private Rect CardInfoRect { get; set; }
        private Rect MessageRect{ get; set; }
        private Rect ButtonRect{get; set; }
    
        [MenuItem("Tools/Utilities/Cost Calculator")]
        public static void ShowWindow()
        {
            CostCalculatorWindow window =
                (CostCalculatorWindow)GetWindow(typeof(CostCalculatorWindow), Is_Utility_Window, Window_Title);
            
            window.position = new Rect(50, 50, 300, 500);
            window.Show();
        }

        private void OnGUI()
        {
            SetupAreaRects();
            DrawCardObjectField();
            DrawCardInfoArea();
            DrawMessageBox();
            DrawControlButtons();
        }

        private void SetupAreaRects()
        {
            CardInfoRect = new Rect(position.width * 0.10f, 50,position.width * 0.5f, position.height * 0.5f );
          
            MessageRect = new Rect(position.width * 0.10f, CardInfoRect.y + CardInfoRect.height + 10,position.width * 0.5f,position.height * 0.10f);
           
            ButtonRect = new Rect(position.width * 0.10f, MessageRect.y + MessageRect.height + 5, position.width * 0.5f, position.height * 0.20f);
        }

        
         private void DrawCardObjectField()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.Label(new Rect(CardInfoRect.x, CardInfoRect.y - 40, position.width * 0.333f, 20), "Loaded Card:");
            LoadedCardData = EditorGUI.ObjectField(new Rect(CardInfoRect.x + 110, CardInfoRect.y - 40, position.width * 0.333f, 20), LoadedCardData,
                typeof(CardSO), false) as CardSO;
            EditorGUILayout.EndHorizontal();
        }
        private void DrawCardInfoArea()
        {
            GUILayout.BeginArea(CardInfoRect);
            // Show values from the loaded card
            DrawLabel($"Card Name: {LoadedCardData?.CardName}");
            DrawLabel($"Card Type: {LoadedCardData?.CardType}");
            DrawLabel($"Attack: {LoadedCardData?.Attack.StatValue}");
            DrawLabel($"Explore: {LoadedCardData?.Explore.StatValue}");
            DrawLabel($"Focus: {LoadedCardData?.Focus.StatValue}");
            DrawLabel($"Hit Points: {LoadedCardData?.HitPoints.StatValue}");
            DrawLabel($"Speed: {LoadedCardData?.Speed.StatValue}");
            DrawLabel($"Upgrade Slots: {LoadedCardData?.UpgradeSlots.StatValue}");
            DrawLabel($"Keywords: {KeywordSumString}");
            GUILayout.EndArea();
        }

        public void OpenInCostCalculatorWindow(CardSO card)
        {
            LoadedCardData = card;
            RunCalculation();
        }

        private void DrawMessageBox()
        {
            GUILayout.BeginArea(MessageRect);
            EditorGUILayout.LabelField(Message,  EditorStyles.whiteLargeLabel, GUILayout.Height(MessageRect.height));
            GUILayout.EndArea();
        }

        private void DrawControlButtons()
        {
            GUILayout.BeginArea(ButtonRect);
            GUILayout.BeginHorizontal(GUILayout.Width(position.width * 0.33f),GUILayout.ExpandHeight(true));
            
            DrawRunCalculationButton();
            DrawOpenInCardEditorButton();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawRunCalculationButton()
        {
            if (GUILayout.Button(CalculateButtonText, GUILayout.Width(position.width * 0.25f), GUILayout.Height(ButtonRect.height * 0.25f)))
            {
                RunCalculation();
            }
        }

        private void DrawOpenInCardEditorButton()
        {
            if (GUILayout.Button(EditButtonText, GUILayout.Width(position.width * 0.25f), GUILayout.Height(ButtonRect.height * 0.25f)))
            {
                CardEditor.CardEditorWindow instance = EditorWindow.GetWindow<CardEditor.CardEditorWindow>();
                instance.OpenCardInEditor(LoadedCardData);
            }
        }
        private void DrawLabel(string text) 
        {
            EditorGUILayout.LabelField(text, EditorStyles.whiteLargeLabel, GUILayout.Height(Label_Height));
        }

        private void RunCalculation()
        {
           
            float cost = 0.0f;
            Message = $"Calculating cost for {LoadedCardData?.CardName}";
            if (!ReferenceEquals(LoadedCardData, null))
            {
               CostCalculator calculator = new(calculationSettings,LoadedCardData.WeightData,LoadedCardData.GetCardStats(), LoadedCardData.Keywords);
                ;
               cost = calculator.NormalizeCost();
            }
            KeywordSumString = LoadedCardData?.GetKeywordsSumString();
            Message = $"Cost is {cost}";
            AssignCostToCard((int)cost);
        }

        private void AssignCostToCard(int cost)
        {
            LoadedCardData.CardCost = cost;
        }
    }
}


