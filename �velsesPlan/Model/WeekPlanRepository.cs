namespace ØvelsesPlan.Model
{
    public class WeekPlanRepository
    {
        public WeekPlan GetCurrentWeekPlan()
        {
            return new WeekPlan(DanishClaendar.CurrentWeek);
        }

        public WeekPlan CreateWeekPlanFor(int week)
        {
           return new WeekPlan(week);
        }
    }
}