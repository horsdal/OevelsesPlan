namespace ØvelsesPlan.Model
{
    public interface WeekPlanRepository
    {
        WeekPlan CreateWeekPlanFor(int week);
        WeekPlan GetWeekPlanFor(int week);
    }
}