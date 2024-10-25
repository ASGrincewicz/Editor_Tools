using Editor.CardData;
using Editor.Channels;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.CostCalculator
{
    public class CostCalculatorWindow : EditorWindow
    {
        [SerializeField] private EditorWindowChannel _editorWindowChannel;
        public CostCalculatorSettings calculationSettings;
        
        
        // Constants
        private const string Window_Title = "Card Cost Calculator";
        private const bool Is_Utility_Window = false;
        private const string CalculateButtonText = "Calculate";
        private const string EditButtonText = "Edit Card";
        private const string CloseButtonText = "Close";
        private const float Label_Height = 30;
        private const float Rect_Width = 300;
        
        private static CostCalculatorWindow _costCalculatorWindow;

        // Class Variables
        private CardDataSO LoadedCardDataData { get; set; } = null;

        private string Message { get; set; } = "";
        private string KeywordSumString { get; set; }

        // GUI Variables
        private Rect CardInfoRect { get; set; }
        private Rect MessageRect{ get; set; }
        private Rect ButtonRect{get; set; }
    
        [MenuItem("Tools/Utilities/Cost Calculator")]
        public static void Init()
        {
            _costCalculatorWindow =
                (CostCalculatorWindow)GetWindow(typeof(CostCalculatorWindow), Is_Utility_Window, Window_Title);
            
            _costCalculatorWindow.position = new Rect(50, 50, 300, 500);
            _costCalculatorWindow.Show();
        }

        private void OnEnable()
        {
            _editorWindowChannel.OnCostCalculatorWindowRequested += OpenCostCalculatorWindow;
        }

        private void OnDisable()
        {
            _editorWindowChannel.OnCostCalculatorWindowRequested -= OpenCostCalculatorWindow;
        }

        private void OpenCostCalculatorWindow(CardDataSO cardData)
        {
            Init();
            LoadedCardDataData = cardData;
            RunCalculation();
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
           
            ButtonRect = new Rect(position.width * 0.10f, MessageRect.y + MessageRect.height + 5, position.width * 0.75f, position.height * 0.20f);
        }

        
         private void DrawCardObjectField()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.Label(new Rect(CardInfoRect.x, CardInfoRect.y - 40, position.width * 0.333f, 20), "Loaded Card:");
            LoadedCardDataData = EditorGUI.ObjectField(new Rect(CardInfoRect.x + 110, CardInfoRect.y - 40, position.width * 0.333f, 20), LoadedCardDataData,
                typeof(CardDataSO), false) as CardDataSO;
            EditorGUILayout.EndHorizontal();
        }
        private void DrawCardInfoArea()
        {
            GUILayout.BeginArea(CardInfoRect);
            DrawLabel($"Card Name: {LoadedCardDataData?.CardName}");
            DrawLabel($"Card Type: {LoadedCardDataData?.CardType}");
            DrawLabel($"Attack: {LoadedCardDataData?.Attack.StatValue}");
            DrawLabel($"Explore: {LoadedCardDataData?.Explore.StatValue}");
            DrawLabel($"Focus: {LoadedCardDataData?.Focus.StatValue}");
            DrawLabel($"Hit Points: {LoadedCardDataData?.HitPoints.StatValue}");
            DrawLabel($"Speed: {LoadedCardDataData?.Speed.StatValue}");
            DrawLabel($"Upgrade Slots: {LoadedCardDataData?.UpgradeSlots.StatValue}");
            DrawLabel($"Keywords: {KeywordSumString}");
            GUILayout.EndArea();
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
            DrawCloseButton();
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
               _editorWindowChannel.RaiseCardEditorWindowRequestedEvent(LoadedCardDataData);
            }
        }

        private void DrawCloseButton()
        {
            if (GUILayout.Button(CloseButtonText, GUILayout.Width(position.width * 0.25f),
                    GUILayout.Height(ButtonRect.height * 0.25f)))
            {
                _costCalculatorWindow.Close();
            }
        }
        private void DrawLabel(string text) 
        {
            EditorGUILayout.LabelField(text, EditorStyles.whiteLargeLabel, GUILayout.Height(Label_Height));
        }

        private void RunCalculation()
        {
           ErrorHandler.TryToGetCard(LoadedCardDataData);
            float cost = 0.0f;
            Message = $"Calculating cost for {LoadedCardDataData?.CardName}";
            CostCalculator calculator = new(calculationSettings,LoadedCardDataData.WeightData,LoadedCardDataData.GetCardStats(), LoadedCardDataData.Keywords);
            cost = calculator.NormalizeCost();
            KeywordSumString = LoadedCardDataData?.GetKeywordsSumString();
            Message = $"Cost is {cost}";
            AssignCostToCard((int)cost);
        }

        private void AssignCostToCard(int cost)
        {
            LoadedCardDataData.CardCost = cost;
        }
    }
}


