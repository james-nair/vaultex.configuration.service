using Vaultex.Service.Shared.Interfaces;
using Vaultex.Settings.Definitions;

namespace Vaultex.Configuration.Service.Jobs
{
    public class TestJob : IJob
    {
        private readonly ILogger _logger;
        private readonly TestJobSetting _setting;

        public TestJob(ILogger<TestJob> logger, TestJobSetting setting)
        {
            _logger = logger;
            _setting = setting;
        }
        public async Task Run(CancellationToken ct)
        {

            await Task.Run(() =>
            {
                _logger.LogInformation($"Test Job was run, with the message: {_setting.Message}");
            }, ct);
        }
    }
}
