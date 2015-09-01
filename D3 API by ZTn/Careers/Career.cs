﻿using System;
using System.IO;
using System.Runtime.Serialization;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.Annotations;
using ZTn.BNet.D3.Artisans;
using ZTn.BNet.D3.Helpers;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Progresses;

namespace ZTn.BNet.D3.Careers
{
    [DataContract]
    public class Career : D3Object
    {
        #region >> Fields

        [DataMember(Name = "battleTag"), UsedImplicitly]
        private string SBattleTag
        {
            set { BattleTag = new BattleTag(value); }
            get { return BattleTag.Id; }
        }

        [IgnoreDataMember]
        public BattleTag BattleTag = new BattleTag("undefined#0000");

        [DataMember(Name = "guildName")]
        public string GuildName;

        [DataMember(Name = "heroes")]
        public HeroSummary[] Heroes;

        [DataMember(Name = "lastHeroPlayed")]
        public string LastHeroPlayed;

        [DataMember(Name = "lastUpdated")]
        protected long SLastUpdated
        {
            set { LastUpdated = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(value).ToLocalTime(); }
            get { return LastUpdated.Second - new DateTime(1970, 1, 1, 0, 0, 0, 0).Second; }
        }

        [IgnoreDataMember]
        public DateTime LastUpdated;

        #region >> Artisans

        [DataMember(Name = "blacksmith")]
        public CareerArtisan Blacksmith;

        [DataMember(Name = "jeweler")]
        public CareerArtisan Jeweler;

        [DataMember(Name = "mystic")]
        public CareerArtisan Mystic;

        [DataMember(Name = "blacksmithHardcore")]
        public CareerArtisan BlacksmithHardcore;

        [DataMember(Name = "jewelerHardcore")]
        public CareerArtisan JewelerHardcore;

        [DataMember(Name = "mysticHardcore")]
        public CareerArtisan MysticHardcore;

        [DataMember(Name = "blacksmithSeason")]
        public CareerArtisan BlacksmithSeason;

        [DataMember(Name = "jewelerSeason")]
        public CareerArtisan JewelerSeason;

        [DataMember(Name = "mysticSeason")]
        public CareerArtisan MysticSeason;

        [DataMember(Name = "blacksmithSeasonHardcore")]
        public CareerArtisan BlacksmithSeasonHardcore;

        [DataMember(Name = "jewelerSeasonHardcore")]
        public CareerArtisan JewelerSeasonHardcore;

        [DataMember(Name = "mysticSeasonHardcore")]
        public CareerArtisan MysticSeasonHardcore;

        #endregion

        [DataMember(Name = "kills")]
        public CareerKills Kills;

        [DataMember(Name = "highestHardcoreLevel")]
        public int HighestHardcoreLevel;

        [DataMember(Name = "timePlayed")]
        public ClassTimePlayed TimePlayed;

        [DataMember(Name = "fallenHeroes")]
        public HeroSummary[] FallenHeroes;

        [DataMember(Name = "paragonLevel")]
        public int ParagonLevel;

        [DataMember(Name = "paragonLevelHardcore")]
        public int ParagonLevelHardcore;

        [DataMember(Name = "paragonLevelSeason")]
        public int ParagonLevelSeason;

        [DataMember(Name = "paragonLevelSeasonHardcore")]
        public int ParagonLevelSeasonHardcore;

        [DataMember(Name = "progression")]
        public CareerProgress Progression;

        [DataMember(Name = "seasonalProfiles")]
        public SeasonalProfiles SeasonalProfiles;

        #endregion

        public static Career CreateFromBattleTag(BattleTag battleTag)
        {
            return D3Api.GetCareerFromBattleTag(battleTag);
        }

        public static Career CreateFromJSonStream(Stream stream)
        {
            return stream.CreateFromJsonStream<Career>();
        }

        public static Career CreateFromJSonString(string json)
        {
            return json.CreateFromJsonString<Career>();
        }
    }
}