using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;


namespace KTouch.Controls.Model {
    class Contact : IDataErrorInfo {

        /// <summary>
        /// Creates an empty Contact
        /// </summary>
        /// <returns></returns>
        public static Contact CreateNewContact() {
            return new Contact() { 
                Name = null,
                Surname = null,
                Email = null,
                Phone = null,
            };
        }

        /// <summary>
        /// Creates a new Contact with given parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static Contact CreateContact(string name, string surname, string email, string preferences) {
            return new Contact {
                Name = name,
                Surname = surname,
                Email = email, 
                Preferences = preferences,
            };
        }

        /// <summary>
        /// Creates a new Contact with given parameters and phone
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static Contact CreateContactWithPhone(string name, string surname, string email, string phone, string preferences) {
            return new Contact {
                Name = name,
                Surname = surname,
                Email = email,
                Phone = phone,
                Preferences = preferences,
            };
        }

        /// <summary>
        /// Protected constructor
        /// </summary>
        protected Contact() { }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Preferences { get; set; }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object
        /// </summary>
        string IDataErrorInfo.Error { get { return null; } }

        /// <summary>
        /// Gets the error message for the property with the given name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string IDataErrorInfo.this[string propertyName] {
            get { return this.GetValidationError(propertyName); }
        }

        /// <summary>
        /// Determines whether this contact is valid in order to send it to the databases
        /// </summary>
        public bool IsValid {
            get {
                foreach (string property in ValidatedProperties)
                    if (GetValidationError(property) != null) {
                        return false;
                    }
                return true;
            }
        }

        /// <summary>
        /// Properties that have to be validated
        /// </summary>
        static readonly string[] ValidatedProperties = { "Name", "Surname", "Email", "Phone", "Preferences" };

        /// <summary>
        /// Validates all the 4 properties imported to database
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string GetValidationError(string propertyName) {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;
            string error = null;
            switch (propertyName) {
                case "Name" :
                    error = this.ValidateName();
                    break;
                case "Surname" :
                    error = this.ValidateSurname();
                    break;
                case "Email" :
                    error = this.ValidateEmail();
                    break;
                case "Phone" :
                    error = this.ValidatePhone();
                    break;
                case "Preferences" :
                    error = this.ValidatePreferences();
                    break;
                default :
                    Debug.Fail("Unexpected property being validated on Customer: " + propertyName);
                    break;
            }
            return error;
        }

        /// <summary>
        /// Name validation
        /// </summary>
        /// <returns></returns>
        string ValidateName() {
            if (IsStringMissing(this.Name)) {
                return "Name is mandatory";
            }
            return null;
        }

        /// <summary>
        /// Surname validation
        /// </summary>
        /// <returns></returns>
        string ValidateSurname() {
            if (IsStringMissing(this.Surname)) {
                return "Surname is mandatory";
            }
            return null;
        }

        /// <summary>
        /// Email validation
        /// </summary>
        /// <returns></returns>
        string ValidateEmail() {
            if (IsStringMissing(this.Email)) {
                return "E-mail is mandatory";
            } else if (!IsValidEmailAddress(this.Email)) {
                return "Invalid e-mail address";
            }
            return null;
        }

        /// <summary>
        /// Phone validation
        /// </summary>
        /// <returns></returns>
        string ValidatePhone() {
            if (!IsStringMissing(this.Phone))
                if(!IsValidPhone(this.Phone))
                    return "Invalid phone number";
            return null;
        }

        /// <summary>
        /// Preferences validation
        /// </summary>
        /// <returns></returns>
        string ValidatePreferences() {
            if (IsStringMissing(this.Preferences))
                return "None of the documents is selected";
            return null;
        }
        /// <summary>
        /// Returns true if 'value' string is neither empty nor contains only white spaces
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static bool IsStringMissing(string value) {
            return String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Returns true if 'email' string matches the given patter of an e-mail address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        static bool IsValidEmailAddress(string email) {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Returns true if 'phone' string matches the given patter of an telephone number
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        static bool IsValidPhone(string phone) {
            string pattern = @"^(\+33\s?[1-9]|\(?0[1-9]\)?)\s?\d{4}\s?\d{4}$";
            return Regex.IsMatch(phone, pattern, RegexOptions.IgnoreCase);
        }
    }
}
