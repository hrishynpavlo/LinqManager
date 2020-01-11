using LinqManager.Enums;
using System;
using LinqManager.Constants;
using System.Collections.Generic;

namespace LinqManager.Exceptions
{
    public class LinqManagerException: Exception
    {
        public ExceptionTypes Type { get; private set; } = ExceptionTypes.Unhandled;
        public Dictionary<string, IReadOnlyList<string>> ValidationErrors { get; private set; } = new Dictionary<string, IReadOnlyList<string>>();

        public LinqManagerException(string message, Exception ex): base(message, ex) 
        {
            SetType(message);
        }

        public LinqManagerException(string message, Exception ex, Dictionary<string, IReadOnlyList<string>> validationErrors)
            :base(message, ex)
        {
            Type = ExceptionTypes.MismatchProperty;
            ValidationErrors = validationErrors;
        }

        private void SetType(string message)
        {
            if (message == ErrorMessages.RequestValidationError)
                Type = ExceptionTypes.RequestValidationError;

            else if (message == ErrorMessages.RequestFactoryError)
                Type = ExceptionTypes.RequestFactoryError;

            else if (message == ErrorMessages.ProcessError)
                Type = ExceptionTypes.ProcessError;

            else if (message == ErrorMessages.UnsupportedMethod)
                Type = ExceptionTypes.UnsupportedMethod;

            else if (message == ErrorMessages.MismatchProperty)
                Type = ExceptionTypes.MismatchProperty;
        }
    }
}