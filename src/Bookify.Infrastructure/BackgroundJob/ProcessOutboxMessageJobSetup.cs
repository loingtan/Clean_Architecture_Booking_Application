using Bookify.Infrastructure.Outbox;
using Microsoft.Extensions.Options;
using Quartz;

namespace Bookify.Infrastructure.BackgroundJob;
public class ProcessOutboxMessageJobSetup(IOptions<OutboxOptions> outboxOptions) : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(ProcessOutboxMessagesJob);

        options.AddJob<ProcessOutboxMessagesJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure.ForJob(jobName)
                .WithSimpleSchedule(schedule =>
                    schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()));
    }
}
