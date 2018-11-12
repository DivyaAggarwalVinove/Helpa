using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Helpa.Models
{
    public class JobPostModel
    {
        public int UserId { get; set; }

        public bool BookMark { get; set; }

        public int JobId { get; set; }

        public bool IsDraft { get; set; }

        public string JobTitle { get; set; }

        public string MinPrice { get; set; }

        public string MaxPrice { get; set; }

        public string JobDescription { get; set; }

        public string Status { get; set; }

        public DateTime CreateDate { get; set; }

        public int RemainingDays { get; set; }

        public int JobServicesId { get; set; }

        public string JobServicesName { get; set; }

        public string HelperType { get; set; }

        public string LocationType { get; set; }

        public string JobType { get; set; }

        public string LocationName { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public List<ScopeModel> JobScope { get; set; }

        public List<Receiver> Receivers { get; set; }

        public List<LocationModel> Location { get; set; }

        public bool IsSunday { get; set; }

        public bool IsMonday { get; set; }

        public bool IsTuesday { get; set; }

        public bool IsWednesday { get; set; }

        public bool IsThursday { get; set; }

        public bool IsFriday { get; set; }

        public bool IsSaturday { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool IsHourly { get; set; }

        public bool IsDaily { get; set; }

        public bool IsMonthly { get; set; }

        public string MinHourlyPrice { get; set; }

        public string MaxHourlyPrice { get; set; }

        public string MinDailyPrice { get; set; }

        public string MaxDailyPrice { get; set; }

        public string MinMonthlyPrice { get; set; }

        public string MaxMonthlyPrice { get; set; }
    }

    public class Receiver
    {
        public int ReceiverGender { get; set; }

        public int ReceiverAge { get; set; }
    }
}