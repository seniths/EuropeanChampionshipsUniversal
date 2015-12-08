﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EuropeanChampionshipsUniversal.Model
{
    public class TeamPlayer
    {
        public string name { get; set; }
        public string position { get; set; }
        public int? jerseyNumber { get; set; }
        public string dateOfBirth { get; set; }
        public string nationality { get; set; }
        public string contractUntil { get; set; }
        public string marketValue { get; set; }
    }
}
