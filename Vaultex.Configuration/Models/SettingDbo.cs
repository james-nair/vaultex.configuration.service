using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Vaultex.Database;

namespace Vaultex.Configuration.Models
{
    public class SettingDbo : BaseDbo
    {
        public Guid? ParentUuid { get; internal set; }

        [ForeignKey(nameof(ParentUuid))]
        public virtual SettingDbo? Parent { get; internal set; }
        public virtual List<SettingDbo>? Children { get; internal set; }
        public string Type { get; internal set; }

        [Column("SubType")]
        public string BaseType { get; internal set; }
        public JsonDocument JsonConfig { get; set; }
    }
}
