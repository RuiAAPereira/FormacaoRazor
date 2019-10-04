﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormacaoRazor.Models.Marcacoes
{
    [Table("Calendarios")]
    public partial class Calendario
    {
        public int DateKey { get; set; }
        public DateTime Date { get; set; }
        public byte Day { get; set; }
        public string DaySuffix { get; set; }
        public byte Weekday { get; set; }
        public string WeekDayName { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayText { get; set; }
        public byte DowinMonth { get; set; }
        public short DayOfYear { get; set; }
        public byte WeekOfMonth { get; set; }
        public byte WeekOfYear { get; set; }
        public byte IsoweekOfYear { get; set; }
        public byte Month { get; set; }
        public string MonthName { get; set; }
        public byte Quarter { get; set; }
        public string QuarterName { get; set; }
        public int Year { get; set; }
        public string Mmyyyy { get; set; }
        public string MonthYear { get; set; }
        public DateTime FirstDayOfMonth { get; set; }
        public DateTime LastDayOfMonth { get; set; }
        public DateTime FirstDayOfQuarter { get; set; }
        public DateTime LastDayOfQuarter { get; set; }
        public DateTime FirstDayOfYear { get; set; }
        public DateTime LastDayOfYear { get; set; }
        public DateTime FirstDayOfNextMonth { get; set; }
        public DateTime FirstDayOfNextYear { get; set; }
    }
}
