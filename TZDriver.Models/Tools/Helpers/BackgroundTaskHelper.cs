using System;
using System.Linq;
using System.Threading.Tasks;
using TZDriver.Models.Tools.Utilities;
using Windows.ApplicationModel.Background;

namespace TZDriver.Models.Tools.Helpers
{
    /// <summary>
    /// Provides helper methods for registering and unregistering background tasks.
    /// </summary>
    public class BackgroundTaskHelper
    {
        /// <summary>
        /// Registers a background task with the specified taskEntryPoint, name, trigger,
        /// and condition (optional).
        /// </summary>
        /// <param name="taskEntryPoint">Task entry point for the background task.</param>
        /// <param name="taskName">A name for the background task.</param>
        /// <param name="trigger">The trigger for the background task.</param>
        /// <param name="condition">Optional parameter. A conditional event that must be true for the task to fire.</param>
        /// <returns>The registered background task.</returns>
        public static async Task<BackgroundTaskRegistration> RegisterBackgroundTask(String taskEntryPoint, String name, IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            BackgroundTaskRegistration task = null;
            try
            {
                // Get permission for a background task from the user. If the user has already answered once,
                // this does nothing and the user must manually update their preference via PC Settings.
                BackgroundAccessStatus backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

                switch (backgroundAccessStatus)
                {
                    // BackgroundTask is allowed
                    case BackgroundAccessStatus.AlwaysAllowed:
                    case BackgroundAccessStatus.AllowedSubjectToSystemPolicy:
                        var builder = new BackgroundTaskBuilder();
                        builder.Name = name;
                        builder.TaskEntryPoint = taskEntryPoint;
                        builder.SetTrigger(trigger);

                        if (condition != null)
                        {
                            builder.AddCondition(condition);

                            // If the condition changes while the background task is executing then it will be canceled.
                            builder.CancelOnConditionLoss = true;
                        }

                        task = builder.Register();
                        break;

                    default:
                        Toast.ShowToast("You have denied Background access to this app");
                        break;
                }
            }
            catch (Exception ex)
            {
                Toast.ShowToast(ex.ToString());
            }

            return task;
        }

        public static bool IsTaskRegistered(String taskName)
        {
            var taskRegistered = false;
            var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(kvp => kvp.Value.Name == taskName);

            if (entry.Value != null)
                taskRegistered = true;

            return taskRegistered;
        }

        /// <summary>
        /// Unregisters the background task with the specified name.
        /// </summary>
        /// <param name="taskName">The name of the background task to unregister.</param>
        public static void UnregisterBackgroundTask(string taskName)
        {
            BackgroundTaskRegistration.AllTasks.First(task =>
                taskName.Equals(task.Value.Name)).Value.Unregister(cancelTask: true);
        }

        public static async Task<bool> IfAccessNotDeniedAsync()
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.DeniedByUser)
                return false;
            else
                return true;
        }
    }
}
