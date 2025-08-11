namespace TaskManagementApplication.Entities
{
    public class RequiredField
    {
        public string Name { get; set; } = string.Empty;
        public Type Type { get; private set; }
        private object? _value;

        public RequiredField(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public T? GetValue<T>() => (T?)_value;

        public void SetValue<T>(T value)
        {
            if (value is not null && !Type.IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException($"Value type {typeof(T)} is not assignable to {Type}");
            _value = value;
        }
    }

}
