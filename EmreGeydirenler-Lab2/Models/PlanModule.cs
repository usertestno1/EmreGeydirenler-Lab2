namespace EmreGeydirenler_Lab2.Models
{
    // Junction table to resolve the Many-to-Many relationship between SubscriptionPlan and Module.
    public class PlanModule
    {
        // Composite Primary Keys will be configured in DbContext using Fluent API
        public int PlanId { get; set; }
        public virtual required SubscriptionPlan SubscriptionPlan { get; set; }

        public int ModuleId { get; set; }
        public virtual required Module Module { get; set; }
    }
}
