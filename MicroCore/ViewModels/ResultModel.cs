using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MicroCore.ViewModels
{
    public class ResultModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultModel{T}"/> class.
        /// </summary>
        public ResultModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultModel{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="message">The message.</param>
        public ResultModel(T data, string message)
        {
            Data = data;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultModel{T}"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public ResultModel(string errorMessage)
        {
            AddError(errorMessage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultModel{T}"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public ResultModel(List<string> errorMessage)
        {
            errorMessage.ForEach(x => AddError(x));
        }

        /// <summary>
        /// Gets or sets the validation errors.
        /// </summary>
        /// <value>The validation errors.</value>
        public List<ValidationResult> ValidationErrors { get; set; } = new List<ValidationResult>();

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified column name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>System.String.</returns>
        public string this[string columnName]
        {
            get
            {
                var validatioResult =
                    ValidationErrors.FirstOrDefault(r => r.MemberNames.FirstOrDefault() == columnName);
                return validatioResult == null ? string.Empty : validatioResult.ErrorMessage;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has error.
        /// </summary>
        /// <value><c>true</c> if this instance has error; otherwise, <c>false</c>.</value>
        public bool HasError
        {
            get
            {
                if (ValidationErrors.Count > 0) return true;

                return false;
            }
        }

        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> GetErrorMessages()
        {
            return ValidationErrors.Select(c => c.ErrorMessage).ToList();
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void AddError(string error)
        {
            ValidationErrors.Add(new ValidationResult(error));
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        public void AddError(ValidationResult validationResult)
        {
            ValidationErrors.Add(validationResult);
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        public void AddError(IEnumerable<ValidationResult> validationResults)
        {
            ValidationErrors.AddRange(validationResults);
        }
    }
}
