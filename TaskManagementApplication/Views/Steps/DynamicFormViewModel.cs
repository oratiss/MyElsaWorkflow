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
        CheckBoxList
    }

    public class DynamicField
    {
        public string Name { get; set; } = default!;
        public string Label { get; set; } = default!;
        public FieldType Type { get; set; }
        public object? Value { get; set; }  // for pre-filling
        public List<string>? Options { get; set; } // only for dropdown

        public bool IsDiasabledOnView { get; set; }
    }
}
