using Microsoft.Extensions.Logging;
using System.Text.Json;
using Vaultex.Configuration.Models;
using Vaultex.Repositories;
using Vaultex.Settings;
using Vaultex.Settings.Interfaces;

namespace Vaultex.Configuration.Client
{
    public class SettingsClient : ISettingsClient
    {
        private readonly ILogger<SettingsClient> _logger;
        private readonly ISettingRepository _settingRepository;
        private readonly ISettingCreator _settingCreator;

        public SettingsClient(ILogger<SettingsClient> logger, ISettingRepository settingRepository, ISettingCreator settingCreator)
        {
            _logger = logger;
            _settingRepository = settingRepository;
            _settingCreator = settingCreator;
        }

        public Task<int> BulkCreateAsync<T>(IEnumerable<T> settings) where T : Setting, new()
        {
            throw new NotImplementedException();
        }

        public async Task<int> BulkSaveSettings<T>(IEnumerable<T> settings) where T : Setting, new()
        {
            //Convert all Settings to SettingDbos and save to DB
            var entities = settings.Select(CreateEntityFromSetting);
            var result = await _settingRepository.BulkCreateAsync(entities);

            return result;
        }

        public Task<int> BulkUpdateAsync<T>(IEnumerable<T> settings) where T : Setting, new()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAsync<T>(T setting) where T : Setting
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll<T>(Guid? parentUuid = null) where T : Setting, new()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById<T>(Guid id) where T : Setting, new()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetSettingsByType<T>(Type t) where T : Setting, new()
        {
            var entites = await _settingRepository.GetByType(t);
            var settings = entites.Select(e => CreateSettingFromDbo<T>(e));

            return settings;
        }

        public async Task<bool> SaveSetting<T>(T setting) where T : Setting, new()
        {
            //using var dbContext = await _dbFactory.CreateDbContextAsync();
            var dbo = CreateEntityFromSetting(setting);
            return await _settingRepository.CreateAsync(dbo);
        }

        public Task<bool> UpdateAsync<T>(T entity)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<T>> GetSettingsByBase<T>() where T : Setting
        {
            //IEnumerable<T> settings = [];
            //using (var dbContext = await _dbFactory.CreateDbContextAsync())
            //{

            //    IQueryable<SettingDbo> query = from s in dbContext.Settings
            //                                   where s.BaseType == typeof(T).Name
            //                                   select s;
            //    var list = await query.ToListAsync();
            //    settings = list.Select(CreateSettingWithBase<T>).Where(x => x != null)!;
            //}
            var entities = await _settingRepository.GetByBaseType(typeof(T));
            return entities.Select(CreateSettingWithBase<T>).Where(e => e != null)!;
        }

        private T CreateSettingFromDbo<T>(SettingDbo dbo) where T : Setting, new()
        {
            return _settingCreator.CreateSetting<T>(dbo.Uuid, dbo.ParentUuid, dbo.JsonConfig);
        }

        private T? CreateSettingWithBase<T>(SettingDbo dbo) where T : Setting
        {
            try
            {
                var setting = _settingCreator.CreateSetting(dbo.Type, dbo.Uuid, dbo.ParentUuid, dbo.JsonConfig);
                if (setting is T settingBaseType)
                {
                    return settingBaseType;
                }
            }
            catch
            {

                return null;
            }

            return null;
        }

        private SettingDbo CreateEntityFromSetting<T>(T setting) where T : Setting
        {
            var jsonConfig = Setting.ConvertToJsonString(setting);
            var jsonDoc = JsonDocument.Parse(jsonConfig);
            return new SettingDbo
            {
                Uuid = setting.Uuid,
                ParentUuid = setting.ParentUuid,
                Type = setting.SettingType,
                BaseType = setting.SettingSubType,
                JsonConfig = jsonDoc
            };
        }



    }
}
