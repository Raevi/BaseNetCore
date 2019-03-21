using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Configuration
{
    [Table("Items", Schema = "Configuration")]
    public class ConfigurationItem
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
