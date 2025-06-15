#pragma warning disable CS1591 // Disable XML doc warnings since this class is manually documented

// Required namespaces
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Newsletters.Emails; // For our EmailService class
using Jellyfin.Plugin.Newsletters.Configuration; // For accessing the PluginConfiguration
using MediaBrowser.Model.Tasks; // Jellyfin's task scheduling system
using Microsoft.Extensions.Logging; // Logging support

namespace Jellyfin.Plugin.Newsletters.ScheduledTasks
{
    /// <summary>
    /// This class defines the scheduled task that will send newsletters via email.
    /// It uses the EmailService (our MailKit wrapper) and runs periodically.
    /// </summary>
    public class EmailNewsletterTask : IScheduledTask
    {
        private readonly PluginConfiguration _config; // Contains SMTP settings and email content
        private readonly ILogger<EmailNewsletterTask> _logger; // For logging status or errors

        /// <summary>
        /// Constructor called by Jellyfin when the task is created.
        /// Dependencies are automatically injected by Jellyfin.
        /// </summary>
        public EmailNewsletterTask(PluginConfiguration config, ILogger<EmailNewsletterTask> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Name displayed in Jellyfin's Scheduled Tasks list.
        /// </summary>
        public string Name => "Email Newsletter";

        /// <summary>
        /// Description displayed below the task name.
        /// </summary>
        public string Description => "Email Newsletters";

        /// <summary>
        /// Category under which the task is grouped in Jellyfin's admin panel.
        /// </summary>
        public string Category => "Newsletters";

        /// <summary>
        /// Unique identifier for this task. Used internally.
        /// </summary>
        public string Key => "EmailNewsletters";

        /// <summary>
        /// Defines how often this task runs by default — every 168 hours = once per week.
        /// </summary>
        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            yield return new TaskTriggerInfo
            {
                Type = TaskTriggerInfo.TriggerInterval,
                IntervalTicks = TimeSpan.FromHours(168).Ticks
            };
        }

        /// <summary>
        /// Main logic for the task — this is what runs when the task triggers.
        /// </summary>
        public async Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
        {
            // If user cancels the task (e.g. Jellyfin is shutting down), this safely stops it
            cancellationToken.ThrowIfCancellationRequested();

            // Report 0% progress at start
            progress.Report(0);

            try
            {
                // Create an instance of our MailKit-powered email service
                var emailService = new EmailService(_config, _logger);

                // Send the newsletter using configured values
                bool result = await emailService.SendEmail(
                    _config.ToAddr,      // Who to send it to
                    _config.Subject,     // Subject line
                    _config.Body         // HTML content of the email
                );

                // Log success or failure
                if (result)
                {
                    _logger.LogInformation("EmailNewsletterTask: Newsletter sent successfully.");
                }
                else
                {
                    _logger.LogError("EmailNewsletterTask: Failed to send newsletter.");
                }
            }
            catch (Exception ex)
            {
                // Catch and log any unexpected errors
                _logger.LogError(ex, "EmailNewsletterTask: Exception occurred.");
            }

            // Report 100% progress to let Jellyfin know the task finished
            progress.Report(100);
        }
    }
}
