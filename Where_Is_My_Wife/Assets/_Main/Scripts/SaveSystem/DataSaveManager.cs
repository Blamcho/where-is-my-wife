
        public T GetData<T>(string key)
        {
            try
            {
                if (savedData.TryGetValue(key, out var value))
                    return (T)Convert.ChangeType(value, typeof(T));
            }
            catch 
            {
                return default;
            }

            return default;
        }
