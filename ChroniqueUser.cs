using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChroniqueBot
{
    [Obfuscation, XmlRoot]
    public class ChroniqueUser
    {
       
        public ChroniqueUser(ulong userId, string username)
        {
            Username = username;
            Id = userId;
            Credits = 1000;
            RollDelay = 0L;
            RollDelay2 = 0L;
            StatsDelay = 0L;
            UpdateDelay = 0L;
            Dexterity = 0L;
            Power = 0L;
            Luck = 0L;
            inNeedResponseState = false;
            hasExtraLuck = false;
            isRegistered = false;
            StartTrial = 0L;
        }



        public string Username
        {
            get;
            set;
        }


        public ulong Id
        {
            get;
            set;
        }


        public long Credits
        {
            get;
            set;
        }

        public long RollDelay
        {
            get;
            set;
        }

        public long RollDelay2
        {
            get;
            set;
        }

        public long StatsDelay
        {
            get;
            set;
        }


        public long UpdateDelay
        {
            get;
            set;
        }


        public long Power
        {
            get;
            set;
        }

        public long Luck
        {
            get;
            set;
        }

        public long Dexterity
        {
            get;
            set;
        }

        public long Stamina
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }
        /// <summary>
        /// Trials DM
        /// </summary>
        public bool inNeedResponseState
        {
            get;
            set;
        }


        public bool hasExtraLuck
        {
            get;
            set;
        }


        public bool isRegistered
        {
            get;
            set;
        }


        public long StartTrial
        {
            get;
            set;
        }

        public int TrialResult
        {
            get;
            set;
        }

        public long LastTrialDate
        {
            get;
            set;
        }
        public long Exp
        {
            get;
            set;
        }
    }
}
