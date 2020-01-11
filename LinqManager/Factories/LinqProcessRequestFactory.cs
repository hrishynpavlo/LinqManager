using LinqManager.Exceptions;
using System;
using LinqManager.Constants;

namespace LinqManager.Factories
{
    public abstract class LinqProcessRequestFactory
    {
        private void Validate(LinqProccesRequest request) 
        {
            if (request == null)
                throw new LinqManagerException(ErrorMessages.RequestValidationError, new ArgumentNullException(nameof(request)));

            if (request.FilterBy == null)
                throw new LinqManagerException(ErrorMessages.RequestValidationError, new ArgumentNullException(nameof(request.FilterBy)));

            if (request.SortBy == null)
                throw new LinqManagerException(ErrorMessages.RequestValidationError, new ArgumentNullException(nameof(request.SortBy)));
        }

        protected abstract LinqProccesRequest BuildRequest();

        public LinqProccesRequest CreateRequest() {
            try
            {
                var request = BuildRequest();
                Validate(request);

                return request;
            }
            catch (LinqManagerException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw new LinqManagerException(ErrorMessages.RequestFactoryError, ex);
            }
        }
    }
}