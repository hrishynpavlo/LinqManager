namespace LinqManager.Constants
{
    internal class ErrorMessages
    {
        public const string RequestValidationError = "Error appeared during validating of created linq request";
        public const string RequestFactoryError = "Error appeared during creating linq request by factory";
        public const string ProcessError = "Error appeared during processing linq reuqest";
        public const string UnsupportedMethod = " method is not supported";
        public const string MismatchProperty = "Property of dto model doesn't match to db property";
    }
}