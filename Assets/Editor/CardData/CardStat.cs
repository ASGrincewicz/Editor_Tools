using Editor.Utilities;
using UnityEngine;

namespace Editor.CardData
{
    [System.Serializable]
    public struct CardStat
    {
        //Backing Fields
        [SerializeField] private StatNames _statName;
        [SerializeField] private int _statValue;
        [SerializeField] private string _statDescription;

        //Properties
        public StatNames RawStatName => _statName;
        public string StatName
        {
            get { return _statName.GetDescription(); }
        }

        public int StatValue
        {
            get
            {
                return _statValue;
            }
            set
            {
                _statValue = value;
            }
        }

       public string StatDescription
        {
            get
            {
                return _statDescription;
            }
        } 

        public CardStat(StatNames statName, int statValue = 0, string statDescription = null)
        {
            _statName = statName;
            _statValue = statValue;
            _statDescription = statDescription;
        }

        public void SetStatValue(int value)
        {
            StatValue = value;
        }
    }
}