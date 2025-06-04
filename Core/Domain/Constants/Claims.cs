namespace Domain.Constants
{
    public static class Claims
    {
        // User Management
        public const string CanViewUsers = "users.view";
        public const string CanCreateUsers = "users.create";
        public const string CanEditUsers = "users.edit";
        public const string CanDeleteUsers = "users.delete";

        // Role Management
        public const string CanViewRoles = "roles.view";
        public const string CanCreateRoles = "roles.create";
        public const string CanEditRoles = "roles.edit";
        public const string CanDeleteRoles = "roles.delete";
        public const string CanAssignRoles = "roles.assign";

        // Subscription Management
        public const string CanViewSubscriptions = "subscriptions.view";
        public const string CanCreateSubscriptions = "subscriptions.create";
        public const string CanEditSubscriptions = "subscriptions.edit";
        public const string CanCancelSubscriptions = "subscriptions.cancel";

        // Payment Management
        public const string CanViewPayments = "payments.view";
        public const string CanProcessPayments = "payments.process";
        public const string CanRefundPayments = "payments.refund";

        // Trainer Management
        public const string CanViewTrainers = "trainers.view";
        public const string CanCreateTrainers = "trainers.create";
        public const string CanEditTrainers = "trainers.edit";
        public const string CanDeleteTrainers = "trainers.delete";
        public const string CanAssignTrainers = "trainers.assign";

        // Workout Plan Management
        public const string CanViewWorkoutPlans = "workoutplans.view";
        public const string CanCreateWorkoutPlans = "workoutplans.create";
        public const string CanEditWorkoutPlans = "workoutplans.edit";
        public const string CanDeleteWorkoutPlans = "workoutplans.delete";

        // Diet Plan Management
        public const string CanViewDietPlans = "dietplans.view";
        public const string CanCreateDietPlans = "dietplans.create";
        public const string CanEditDietPlans = "dietplans.edit";
        public const string CanDeleteDietPlans = "dietplans.delete";

        // Reports
        public const string CanViewReports = "reports.view";
        public const string CanExportReports = "reports.export";

        // System
        public const string CanViewSystemLogs = "system.logs.view";
        public const string CanManageSystemSettings = "system.settings.manage";
    }
}
