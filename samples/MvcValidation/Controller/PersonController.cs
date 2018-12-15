using System.Linq;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Result;
using Microsoft.AspNetCore.Mvc;
using MvcValidation.ApiModels;
using MvcValidation.Models;

namespace MvcValidation.Controller {
    [Route("people")]
    public class PersonController : Microsoft.AspNetCore.Mvc.Controller {
        private static IResult<PersonPostApiModel, PersonPostApiError> ApiValidation(PersonPostApiModel model) {
            var apiValidation = model
                .ToResult(x => true, x => default(PersonPostApiError))
                .Multiple(
                    x => x.Filter(
                        y => y.Age > 10,
                        y => new PersonPostApiError {Message = "Age has to be older than '10'.", Model = model}
                    ),
                    x => x.Flatten(
                        y => ValidateName(y.FirstName),
                        s => new PersonPostApiError {Message = s, Model = model}
                    ),
                    x => x.Flatten(
                        y => ValidateName(y.LastName),
                        s => new PersonPostApiError {Message = s, Model = model}
                    )
                );

            return apiValidation.Match(Result.Value<PersonPostApiModel, PersonPostApiError>, x =>
                Result.Error<PersonPostApiModel, PersonPostApiError>(
                    new PersonPostApiError {
                        Errors = x.Select(y => y.Message).ToArray(),
                        Message = "Invalid Api Validation.",
                        Model = model
                    }
                ));
        }

        private static IResult<SuccessModel, ErrorModel> FirstNameAppService(PersonModel person) {
            return person.FirstName
                .ToResult(s => s == "Foo", x => new ErrorModel {Message = "Expected a 'Foo'."})
                .Map(x => new SuccessModel {Count = 4711});
        }

        private static IResult<SuccessModel, ErrorModel> LastNameAppService(PersonModel person) {
            return person.LastName
                .ToResult(s => s == "Bar", x => new ErrorModel {Message = "Expected a 'Bar'."})
                .Map(x => new SuccessModel {Count = 4711});
        }

        [HttpPost]
        [Route("eitherSummarized")]
        public IActionResult PostPerson([FromBody] PersonPostApiModel model) {
            var result = ApiValidation(model)
                .Map(x => new PersonModel {FirstName = x.FirstName, LastName = x.LastName})
                .Flatten(LastNameAppService, x => new PersonPostApiError {Message = x.Message, Model = model})
                .FlatMap(FirstNameAppService, x => new PersonPostApiError {Message = x.Message, Model = model});
            return result.Match<IActionResult>(BadRequest, Ok);
        }

        private static IResult<string, string> ValidateName(string name) {
            return name
                .ToResult(x => string.IsNullOrWhiteSpace(x) == false, x => "Name cannot be empty.")
                .Filter(s => s.All(char.IsLetter), y => "Name can only contain letters.")
                .Filter(s => char.IsUpper(s[0]), y => "Name must start with capital letter.");
        }
    }
}