using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Vaultex.Database;

namespace Vaultex.Configuration.Models
{
    public class SettingDbo : BaseDbo
    {
        public Guid? ParentUuid { get; set; }

        [ForeignKey(nameof(ParentUuid))]
        public virtual SettingDbo? Parent { get; internal set; }
        public virtual List<SettingDbo>? Children { get; internal set; }
        public string Type { get; set; }

        [Column("SubType")]
        public string BaseType { get; set; }
        public JsonDocument JsonConfig { get; set; }
    }
}
