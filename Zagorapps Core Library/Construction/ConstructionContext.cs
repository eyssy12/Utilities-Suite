namespace Zagorapps.Core.Library.Construction
{
    using System.Collections.Generic;

    public class ConstructionContext : IContext
    {
        private readonly IDictionary<string, object> contextParameters;

        public ConstructionContext()
        {
            this.contextParameters = new Dictionary<string, object>();
        }

        public void AddValue(string parameterName, object value)
        {
            if (!this.contextParameters.ContainsKey(parameterName))
            {
                this.contextParameters.Add(parameterName, value);
            }
        }

        public T GetValue<T>(string parameterName)
        {
            if (this.contextParameters.ContainsKey(parameterName))
            {
                object value = this.contextParameters[parameterName];

                try
                {
                    return (T)value;
                }
                catch
                {
                }
                
            }

            return default(T);
        }
    }
}