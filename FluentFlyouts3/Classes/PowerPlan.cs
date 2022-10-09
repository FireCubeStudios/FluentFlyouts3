using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts3.Classes
{
    public record PowerPlan(PowerPlanEnum Plan, Guid Id);

    public enum PowerPlanEnum
    {
        PowerSaver,
        Recommended,
        BetterPerformance,
        BestPerformance
    }

    public struct PowerPlanIds
    {
        public const string PowerSaver = "a1841308-3541-4fab-bc81-f71556f20b4a";
        public const string Recommended = "381b4222-f694-41f0-9685-ff5bb260df2e";
        public const string BetterPerformance = "3af9B8d9-7c97-431d-ad78-34a8bfea439f";
        public const string BestPerformance = "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c";
        public const string Recommended11 = "00000000-0000-0000-0000-000000000000";
        public const string BestPerformance11 = "ed574b5-45a0-4f42-8737-46345c09c238";
    }

    public class PowerMode
    {
        public static readonly PowerPlan PowerSaver = new PowerPlan(PowerPlanEnum.PowerSaver, new Guid(PowerPlanIds.PowerSaver));

        public static readonly PowerPlan Recommended = new PowerPlan(PowerPlanEnum.Recommended, new Guid(PowerPlanIds.Recommended));

        public static readonly PowerPlan BetterPerformance = new PowerPlan(PowerPlanEnum.BetterPerformance, new Guid(PowerPlanIds.BetterPerformance));

        public static readonly PowerPlan BestPerformance = new PowerPlan(PowerPlanEnum.BestPerformance, new Guid(PowerPlanIds.BestPerformance));
    }
}
