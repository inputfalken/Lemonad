using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using MvcValidation.ApiModels;
using MvcValidation.Models;

namespace MvcValidation.Controller {
    [Route("people")]
    public class PersonController : Microsoft.AspNetCore.Mvc.Controller {
        [HttpPost]
        [Route("eitherSummarized")]
        public IActionResult EitherValidationSummarized([FromBody] PersonPostApiModel model) {
            var result = ApiValidation(model)
                .Map(x => new PersonModel {FirstName = x.FirstName, LastName = x.LastName})
                .Flatten(LastNameAppService, x => new PersonPostApiError {Message = x.Message, Model = model})
                .FlatMap(FirstNameAppService, x => new PersonPostApiError {Message = x.Message, Model = model});
            return result.Match<IActionResult>(BadRequest, Ok);
        }

        private static Result<PersonPostApiModel, PersonPostApiError> ApiValidation(PersonPostApiModel model) {
            var either = model.ToResult<PersonPostApiModel, PersonPostApiError>();
            var apiValidation = new List<Result<PersonPostApiModel, PersonPostApiError>> {
                either.Filter(x => x.Age > 10,
                    () => new PersonPostApiError {Message = "Age needs to be more than 10", Model = model}),
                either.Flatten(x => ValidateName(x.FirstName),
                    s => new PersonPostApiError {Message = s, Model = model}),
                either.Flatten(x => ValidateName(x.LastName), s => new PersonPostApiError {Message = s, Model = model})
            };

            var errors = apiValidation
                .Errors()
                .Select(x => x.Message)
                .ToArray();
            return errors.Any()
                ? (Result<PersonPostApiModel, PersonPostApiError>) new PersonPostApiError {
                    Errors = errors,
                    Message = "Invalid Api Validation.",
                    Model = model
                }
                : model;
        }

        public static Result<SuccessModel, ErrorModel> LastNameAppService(PersonModel person) =>
            person.LastName == "Bar"
                ? (Result<SuccessModel, ErrorModel>) new SuccessModel {Count = 4711}
                : new ErrorModel {Message = "Expected a 'Bar'"};

        public static Result<SuccessModel, ErrorModel> FirstNameAppService(PersonModel person) =>
            person.FirstName == "Foo"
                ? (Result<SuccessModel, ErrorModel>) new SuccessModel {Count = 4711}
                : new ErrorModel {Message = "Expected a 'Foo'"};

        private static Result<string, string> ValidateName(string name) {
            return name.ToResult<string, string>()
                .IsErrorWhen(string.IsNullOrWhiteSpace, () => "Name cannot be empty.")
                .Filter(s => s.All(char.IsLetter), () => "Name can only contain letters.")
                .Filter(s => char.IsUpper(s[0]), () => "Name must start with capital letter.");
        }
    }
}