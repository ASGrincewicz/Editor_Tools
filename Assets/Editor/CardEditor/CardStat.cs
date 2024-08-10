
using UnityEngine;

namespace Editor.CardEditor
{
    [System.Serializable]
    public struct CardStat
    {
        //Backing Fields
        [SerializeField] private string _statName;
        [SerializeField] private int _statValue;
        [SerializeField] private string _statDescription;

        //Properties
        public string StatName
        {
            get { return _statName; }
            //set { _statName = value; }
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

        public CardStat(string statName, int statValue = 0, string statDescription = null)
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