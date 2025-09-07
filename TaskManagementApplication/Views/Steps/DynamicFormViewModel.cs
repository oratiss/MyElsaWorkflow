using Rts.Common;

namespace TaskManagementApplication.Views.Steps
{
    public class DynamicFormViewModel
    {
        public long StepId { get; set; }
        public List<DynamicField> Fields { get; set; } = new();

    }

    public enum FieldType
    {
        Int,
        Long,
        Short,
        Decimal,
        Guid,
        Boolean,
        DateTime,
        String,
        Object,
        Dropdown,
        CheckBoxListAll,
        CheckBoxListMany
    }

    public class DynamicField
    {
        public string Name { get; set; } = default!;
        public string Label { get; set; } = default!;
        public FieldType Type { get; set; }
        public string? Value { get; set; }  // for pre-filling
        public List<string>? Options { get; set; } // only for dropdown

        public List<TypeCheckPair<string>>? CheckBoxPairs { get; set; }

        public bool IsDiasabledOnView { get; set; }
    }



}
