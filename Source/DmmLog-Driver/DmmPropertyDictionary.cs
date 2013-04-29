using System;
using System.Collections;
using System.Collections.Generic;

namespace DmmLogDriver {
    /// <summary>
    /// List of properties.
    /// </summary>
    public sealed class DmmPropertyDictionary : IDictionary<String, String> {

        /// <summary>
        /// Creates new instance.
        /// </summary>
        public DmmPropertyDictionary()
            : this(null, false) {
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="properties">Default properties.</param>
        public DmmPropertyDictionary(IDictionary<String, String> properties)
            : this(properties, false) {
        }


        private DmmPropertyDictionary(IDictionary<String, String> properties, Boolean makeReadOnly) {
            this.InternalDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (properties != null) {
                foreach (var item in properties) {
                    this.InternalDictionary.Add(item.Key, item.Value);
                }
                this.IsFrozen = makeReadOnly;
            }
        }


        private readonly Dictionary<String, String> InternalDictionary;
        private readonly Boolean IsFrozen;


        /// <summary>
        /// Returns readonly instance.
        /// </summary>
        public DmmPropertyDictionary AsReadOnly() {
            return new DmmPropertyDictionary(this, true);
        }


        #region IDictionary<>

        /// <summary>
        /// Adds an element with the provided key and value to the dictionary.
        /// </summary>
        /// <param name="key">Key of the element to add.</param>
        /// <param name="value">Value of the element to add.</param>
        public void Add(String key, String value) {
            if (this.IsFrozen) { throw new InvalidOperationException("Dictionary is read-only."); }
            this.InternalDictionary.Add(key, value);
        }

        /// <summary>
        /// Determines whether the dictionary contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        public Boolean ContainsKey(String key) {
            return this.InternalDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Removes the element with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        public Boolean Remove(String key) {
            if (this.IsFrozen) { throw new InvalidOperationException("Dictionary is read-only."); }
            return this.InternalDictionary.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        public Boolean TryGetValue(String key, out String value) {
            return this.InternalDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets an collection containing the keys of the dictionary.
        /// </summary>
        public ICollection<String> Keys {
            get { return this.InternalDictionary.Keys; }
        }

        ICollection<String> IDictionary<String, String>.Values {
            get { return this.InternalDictionary.Values; }
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        public String this[String key] {
            get { return this.InternalDictionary[key]; }
            set {
                if (this.IsFrozen) { throw new InvalidOperationException("Dictionary is read-only."); }
                this.InternalDictionary[key] = value;
            }
        }

        #endregion<>

        #region ICollection<>


        void ICollection<KeyValuePair<String, String>>.Add(KeyValuePair<String, String> item) {
            if (this.IsFrozen) { throw new InvalidOperationException("Dictionary is read-only."); }
            ((ICollection<KeyValuePair<String, String>>)this.InternalDictionary).Add(item);
        }

        /// <summary>
        /// Removes all items from the dictionary.
        /// </summary>
        public void Clear() {
            if (this.IsFrozen) { throw new InvalidOperationException("Dictionary is read-only."); }
            this.InternalDictionary.Clear();
        }

        Boolean ICollection<KeyValuePair<String, String>>.Contains(KeyValuePair<String, String> item) {
            return ((ICollection<KeyValuePair<String, String>>)this.InternalDictionary).Contains(item);
        }

        void ICollection<KeyValuePair<String, String>>.CopyTo(KeyValuePair<String, String>[] array, Int32 arrayIndex) {
            ((ICollection<KeyValuePair<String, String>>)this.InternalDictionary).CopyTo(array, arrayIndex);
        }

        Boolean ICollection<KeyValuePair<String, String>>.Remove(KeyValuePair<String, String> item) {
            if (this.IsFrozen) { throw new InvalidOperationException("Dictionary is read-only."); }
            return ((ICollection<KeyValuePair<String, String>>)this.InternalDictionary).Remove(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the dictionary.
        /// </summary>
        public Int32 Count {
            get { return this.InternalDictionary.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the dictionary is read-only.
        /// </summary>
        public Boolean IsReadOnly {
            get { return this.IsFrozen; }
        }

        #endregion

        #region IEnumerable<>

        /// <summary>
        /// Exposes the enumerator, which supports a simple iteration over a collection.
        /// </summary>
        public IEnumerator<KeyValuePair<String, String>> GetEnumerator() {
            return this.InternalDictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() {
            return this.InternalDictionary.GetEnumerator();
        }

        #endregion

    }
}
